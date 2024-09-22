using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour
{
    //찾은 각 타임의 오브젝트 배열을 Dictionary를 사용하여 리스트로 정리
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    
    //필드에서 자동으로 찾아서 맵핑
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        //enum값들을 string 배열로 변환
        string[] names = Enum.GetNames(type);

        //Dictionary에 넣을 공간 생성
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        //for문으로 해당하는 오브젝트 맵핑
        for (int i = 0; i < names.Length; i++)
        {
            //현재 최상의 gameobject는 이 스크립트가 달려있는 캔버스의
            //하위 캑체들을 순회하면서 일치하는 object를 찾음
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }
        }
    }

    //UI에 Event 연동하는 함수 
    //클릭을 가장 많이 사용하기 때문에 기본값을 클릭으로 함
    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        { 
            case Define.UIEvent.Click:
                evt.onClickHandler -= action;
                evt.onClickHandler += action;
                break;
        }
        evt.onClickHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position;});
    }

    //Bind()를 통해 찾은 일치한 object의 인덱스의 객체를 불러와 해당 객체에 데이터로 바꿈
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
        {
            return null;
        }

        //UntiyEngine.Object 타입을 T타입으로 캐스팅
        return objects[idx] as T;
    }

    //자주 사용하는 컴포넌트를 함수화해서 사용
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
}
