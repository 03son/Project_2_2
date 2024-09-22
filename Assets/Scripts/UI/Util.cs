using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util //��ɼ� �Լ� ����
{
    //��ü���� �ν����Ϳ��� ������Ʈ�� ���� ������ �ʿ� ���� �ڵ�� ����
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform != null)
        { 
            return transform.gameObject;
        }

        return null;
    }
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    { 
        T component = go.GetComponent<T>();
        if (component == null)
        { 
            component = go.AddComponent<T>();
        }

        return component;
    }
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        //�ֻ��� ��ü�� Null�� ���
        if(go == null)
            return null;

        //recursive ��ͼ��� ���� ���ΰ�?(�ڽ��� �ڽ� ��ü���� ã�� ����)
        if (recursive == false) //���� �ڽĸ� ��ĵ
        {
            for (int i = 0; i < go.transform.childCount; i++)
            { 
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                { 
                    T component = transform.GetComponent<T>();
                    if(component != null)
                        return component;
                }
            }
        }
        else //recursive == true ��� �ڽ��� �ڽı��� ��ĵ
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                { 
                    return component;
                }
            }
        }

        //�� ã������
        return null;
    }
}
