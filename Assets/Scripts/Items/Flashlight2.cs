using UnityEngine;

public class Flashlight2 : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // ���� ī�޶��� Transform
    [SerializeField] private float followSpeed = 20f; // ���󰡴� �ӵ� (������ ����)
    private Quaternion currentRotation; // ���� ȸ����
    private Quaternion targetRotation; // ��ǥ ȸ����

    [SerializeField] private float shakeAmount = 30f; // ��鸲 ���� (���� ����)
    private CharacterController characterController; // �÷��̾� �̵� ���� Ȯ�ο�

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
        characterController = GetComponentInParent<CharacterController>(); // �÷��̾� �̵� ���� Ȯ��
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

         if (IsPlayerMoving())
    {
        float noiseX = Mathf.PerlinNoise(Time.time * 5f, 0.0f) - 0.5f;
        float noiseY = Mathf.PerlinNoise(0.0f, Time.time * 5f) - 0.5f;

        float adjustedShakeAmount = shakeAmount;

        if (IsPlayerRunning())
        {
            adjustedShakeAmount *= 1.5f; // �޸��� �� ��鸲 ���� ����
        }

        transform.rotation *= Quaternion.Euler(noiseX * adjustedShakeAmount, noiseY * adjustedShakeAmount, 0);
        Debug.Log("��鸲 ���� ��!");
    }
    }
    private bool IsPlayerRunning()
    {
        return Input.GetKey(KeyManager.Run_Key) && IsPlayerMoving();
    }
    private bool IsPlayerMoving()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // �̵� �Է°��� �ִ� ��� �̵� ������ ����
        return Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;
    }
}
