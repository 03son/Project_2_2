using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util //기능성 함수 모음
{
    //객체별로 인스펙터에서 컨포넌트를 직접 연결할 필요 없이 코드로 연결
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
        //최상위 객체가 Null일 경우
        if(go == null)
            return null;

        //recursive 재귀성을 가질 것인가?(자식의 자식 객체까지 찾을 건지)
        if (recursive == false) //직속 자식만 스캔
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
        else //recursive == true 경우 자식의 자식까지 스캔
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                { 
                    return component;
                }
            }
        }

        //못 찾았으면
        return null;
    }
}
