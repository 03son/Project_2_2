using UnityEngine;

public class SetCameraParent : MonoBehaviour
{
    [SerializeField] private Transform cameraEmptyObject; // ī�޶� �ڽ����� ���� �� ������Ʈ

    void Start()
    {
        if (Camera.main != null)
        {
            // ���� ī�޶��� Transform�� �����ͼ� cameraEmptyObject�� �ڽ����� ����
            Camera.main.transform.SetParent(cameraEmptyObject);

            // ���ϴ� ��ġ�� ȸ���� ���� (�⺻���� �����ϰ� �ʹٸ� ���� ����)
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogError("Main Camera not found!");
        }
    }
}
