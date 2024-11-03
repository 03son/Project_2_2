using UnityEngine;

public class FlashlightSync : MonoBehaviour
{
    public Transform mainCamera; // ���� ī�޶��� Transform�� �Ҵ��� ����

    void Start()
    {
        // ���� ī�޶� �ڵ����� ã��
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        // ī�޶��� ��ġ�� ȸ���� �÷��ö���Ʈ�� ����ȭ
        if (mainCamera != null)
        {
            transform.position = mainCamera.position;
            transform.rotation = mainCamera.rotation;
        }
    }
}
