using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownGlassCup : MonoBehaviour
{
    public AudioClip breakSound;  // �浹 �� ����� �Ҹ�
    private bool isBroken = false;  // �ߺ� �ı� ����
    private bool audioAssigned = false;  // ������� �Ҵ�Ǿ����� Ȯ��

    void Start()
    {
        // ó���� ����� Ŭ���� �Ҵ�Ǿ����� Ȯ��
        AssignAudioClip();
    }

    // �θ� ����� �� ȣ��Ǵ� �̺�Ʈ
    void OnTransformParentChanged()
    {
        // �θ� ����� ������ ����� Ŭ���� �Ҵ�Ǿ����� �ٽ� Ȯ��
        AssignAudioClip();
    }

    // �浹 �̺�Ʈ ó��
    void OnCollisionEnter(Collision collision)
    {
        if (!isBroken)
        {
            isBroken = true;

            // breakSound�� �Ҵ�Ǿ����� Ȯ�� �� ���
            if (breakSound != null)
            {
                AudioSource.PlayClipAtPoint(breakSound, transform.position);
            }
            else
            {
                Debug.LogWarning("breakSound�� �Ҵ���� �ʾҽ��ϴ�!");
            }

            // ������ �ı�
            Destroy(gameObject, 1f);  // �Ҹ� ��� �� 1�� �� ������Ʈ �ı�
        }
    }

    // ����� Ŭ�� �Ҵ��� Ȯ���ϴ� �Լ�
    void AssignAudioClip()
    {
        if (!audioAssigned && breakSound != null)
        {
            audioAssigned = true;  // ����� Ŭ���� �Ҵ�Ǿ����� ���
        }
        else if (breakSound == null)
        {
            Debug.LogWarning("����� Ŭ���� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
