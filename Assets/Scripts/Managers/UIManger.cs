using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class UIManger : Singleton<UIManger>
{
    int _order = 0; //최근에 사용한 sort order 번호

    //popup창들은 stack에서 관리
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    ResourceManager resoure = new ResourceManager();

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                //하이어라키에 UI_Root라는 객체 생성
                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }
    //기존 UI와의 렌더링 우선순위 정하기
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = UI_Util.GetOrAddComponent<Canvas>(go);

        //화면 위에 직접 렌더링되는 모드
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //캔버스의 정렬 우선순위를 수동으로 관리
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else //팝업과 관련없는 스크린UI
        {
            canvas.sortingOrder = 0;
        }
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
        { 
            name = typeof(T).Name;
        }

        GameObject go = resoure.Instantiate($"UI/Scene/{name}");
        T SceneUI = UI_Util.GetOrAddComponent<T>(go);
        _sceneUI = SceneUI;

        //root 자식으로 넣음
        go.transform.SetParent(Root.transform);

        return SceneUI;
    }
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
        { 
            name = typeof(T).Name;
        }

        GameObject go = resoure.Instantiate($"UI/Popup/{name}");
        T popup = UI_Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

 
        //root 자식으로 넣음
        go.transform.SetParent(Root.transform);

        return popup;
    }
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("닫을 창 못 찾음");
        }

        ClosePopupUI();
    }
    public void ClosePopupUI()
    { 
        if(_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        resoure.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }
}
