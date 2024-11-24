using UnityEngine;

public class CameraSync : MonoBehaviour
{
    public Transform modelTransform;  // ĳ���� ���� Transform
    public Transform cameraTransform; // ���� ī�޶��� Transform

    void Update()
    {
        if (cameraTransform != null && modelTransform != null)
        {
            // ī�޶��� Y�� ȸ���� �𵨰� ����ȭ
            Vector3 modelRotation = modelTransform.eulerAngles;
            modelRotation.y = cameraTransform.eulerAngles.y;
            modelTransform.eulerAngles = modelRotation;
        }
    }
}
