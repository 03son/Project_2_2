using UnityEngine;

public class CameraModelRotationSync : MonoBehaviour
{
    [SerializeField] private Transform mainCameraTransform; // ���� ī�޶��� Transform
    [SerializeField] private Transform modelTransform; // �ֻ��� �𵨸� ������Ʈ�� Transform
    [SerializeField] private float rotationSpeed = 10f; // �𵨸��� ī�޶� ȸ���� ���󰡴� �ӵ�

    void Start()
    {
        if (mainCameraTransform == null)
        {
            // ���� ī�޶� �Ҵ���� �ʾҴٸ� �⺻������ ���� ī�޶� ����
            mainCameraTransform = Camera.main.transform;
        }

        if (modelTransform == null)
        {
            modelTransform = transform; // �ֻ��� �𵨸� ������Ʈ�� Transform ���
        }
    }

    void Update()
    {
        if (mainCameraTransform != null && modelTransform != null)
        {
            // ī�޶��� Y�� ȸ�� ���� �𵨿� �ﰢ������ ����
            Vector3 targetRotation = new Vector3(0, mainCameraTransform.eulerAngles.y, 0);
            modelTransform.eulerAngles = targetRotation;
        }
    }



}
