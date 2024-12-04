using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Singleton<T> : MonoBehaviourPun where T : MonoBehaviourPun
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // �ش� ������Ʈ�� ������ �ִ� ���� ������Ʈ�� ã�Ƽ� ��ȯ.
                instance = (T)FindAnyObjectByType(typeof(T));

                if (instance == null) // �ν��Ͻ��� ã�� ���� ���
                {
                    // ���ο� ���� ������Ʈ�� �����Ͽ� �ش� ������Ʈ�� �߰�.
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    // ������ ���� ������Ʈ���� �ش� ������Ʈ�� instance�� ����.
                    instance = obj.GetComponent<T>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (transform.parent != null && transform.root != null) // �ش� ������Ʈ�� �ڽ� ������Ʈ���
        {
            DontDestroyOnLoad(this.transform.root.gameObject); // �θ� ������Ʈ�� DontDestroyOnLoad ó��
        }
        else
        {
            DontDestroyOnLoad(this.gameObject); // �ش� ������Ʈ�� �� ���� ������Ʈ��� �ڽ��� DontDestroyOnLoad ó��
        }
    }
}
