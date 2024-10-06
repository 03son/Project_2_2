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
            Debug.Log($"������ �ε� ���� : {path}");
            return null;
        }

        //���� ���� �������� ����Ǿ� ȣ�� �� �� �ٴ� (Clone)�ؽ�Ʈ ����
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
