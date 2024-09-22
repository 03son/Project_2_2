using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour
{
    //ã�� �� Ÿ���� ������Ʈ �迭�� Dictionary�� ����Ͽ� ����Ʈ�� ����
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    
    //�ʵ忡�� �ڵ����� ã�Ƽ� ����
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        //enum������ string �迭�� ��ȯ
        string[] names = Enum.GetNames(type);

        //Dictionary�� ���� ���� ����
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        //for������ �ش��ϴ� ������Ʈ ����
        for (int i = 0; i < names.Length; i++)
        {
            //���� �ֻ��� gameobject�� �� ��ũ��Ʈ�� �޷��ִ� ĵ������
            //���� Ĵü���� ��ȸ�ϸ鼭 ��ġ�ϴ� object�� ã��
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

    //UI�� Event �����ϴ� �Լ� 
    //Ŭ���� ���� ���� ����ϱ� ������ �⺻���� Ŭ������ ��
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

    //Bind()�� ���� ã�� ��ġ�� object�� �ε����� ��ü�� �ҷ��� �ش� ��ü�� �����ͷ� �ٲ�
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
        {
            return null;
        }

        //UntiyEngine.Object Ÿ���� TŸ������ ĳ����
        return objects[idx] as T;
    }

    //���� ����ϴ� ������Ʈ�� �Լ�ȭ�ؼ� ���
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
}
