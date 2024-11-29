using System.Collections;
using UnityEngine;

public class SetCameraParent : MonoBehaviour
{
    private Transform cameraEmptyObject;

    // Start �޼��带 IEnumerator�� �����Ͽ� �ڷ�ƾ���� ���
    IEnumerator Start()
    {
        // �ణ�� ������ �� �� �� ������Ʈ �Ҵ� �õ�
        yield return new WaitForSeconds(0.1f);

        // �� ������Ʈ�� �̸��� �������� �Ҵ� �õ� (�̸��� "Camera"�� ������Ʈ�� ã��)
        cameraEmptyObject = GameObject.Find("Camera")?.transform;

        if (cameraEmptyObject != null && Camera.main != null)
        {
            // ���� ī�޶��� Transform�� �����ͼ� cameraEmptyObject�� �ڽ����� ����
            Camera.main.transform.SetParent(cameraEmptyObject, false); // Keep world position false

            // ī�޶��� ��ġ�� ȸ���� �� ������Ʈ�� ���� ��ġ�� �����ϰ� ����
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;

            Debug.Log("Camera successfully attached to the empty object after delay.");
        }
        else
        {
            Debug.LogError("Camera or Camera Empty Object not found after delay.");
        }
    }
}
