using UnityEngine;

public class Flashlight2 : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // ���� ī�޶��� Transform
    [SerializeField] private float followSpeed = 1.5f; // ���󰡴� �ӵ� (������ ����)
    private Quaternion currentRotation; // ���� ȸ����
    private Quaternion targetRotation; // ��ǥ ȸ����
    private Light flashlightComponent;

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
            if (cameraTransform == null)
            {
                Debug.LogError("Main Camera�� ã�� �� �����ϴ�!");
            }
        }

        currentRotation = transform.rotation; // �ʱ� ȸ����
    }

    private void LateUpdate()
    {
        if (cameraTransform == null) return;

        // ��ǥ ȸ���� ������Ʈ
        targetRotation = cameraTransform.rotation;

        // SmoothDampó�� �ε巴�� ȸ��
        currentRotation = Quaternion.Lerp(
            currentRotation,
            targetRotation,
            Time.deltaTime * followSpeed
        );

        // ���� ȸ���� ����
        transform.rotation = currentRotation;

        // �����
        Debug.Log("���̶���Ʈ ȸ�� ����ȭ ��: " + transform.rotation.eulerAngles);


    }
}
