using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class UIManger : Singleton<UIManger>
{
    int _order = 0; //�ֱٿ� ����� sort order ��ȣ

    //popupâ���� stack���� ����
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
                //���̾��Ű�� UI_Root��� ��ü ����
                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }
    //���� UI���� ������ �켱���� ���ϱ�
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = UI_Util.GetOrAddComponent<Canvas>(go);

        //ȭ�� ���� ���� �������Ǵ� ���
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //ĵ������ ���� �켱������ �������� ����
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else //�˾��� ���þ��� ��ũ��UI
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

        //root �ڽ����� ����
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

 
        //root �ڽ����� ����
        go.transform.SetParent(Root.transform);

        return popup;
    }
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("���� â �� ã��");
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
