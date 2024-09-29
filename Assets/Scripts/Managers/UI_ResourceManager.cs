using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ResourceManager
{
    public T Load<T>(string path) where T : Object
    { 
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    { 
        GameObject prefab = Load<GameObject>($"prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"프리팹 로드 실패 : {path}");
            return null;
        }

        //여러 개의 프리팹이 복사되어 호출 될 때 붙는 (Clone)텍스트 삭제
        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        { 
            return;
        }

        Object.Destroy(go);
    }
}
