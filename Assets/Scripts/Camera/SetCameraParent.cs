using UnityEngine;

public class SetCameraParent : MonoBehaviour
{
    [SerializeField] private Transform cameraEmptyObject; // ī�޶� �ڽ����� ���� �� ������Ʈ

    void Start()
    {
        if (Camera.main != null)
        {
            // ���� ī�޶��� Transform�� �����ͼ� cameraEmptyObject�� �ڽ����� ����
            Camera.main.transform.SetParent(cameraEmptyObject, false); // Keep world position false

            // ī�޶��� ��ġ�� ȸ���� �� ������Ʈ�� ���� ��ġ�� �����ϰ� ����
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;

            Debug.Log("Camera successfully attached to the empty object.");
        }
        else
        {
            Debug.LogError("Main Camera not found!");
        }
    }
}
