using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public float crouchHeight = 1.0f;    // �ɾ��� �� ����
    public float normalHeight = 2.0f;    // �� ���� �� ����
    public float crouchSpeed = 0.1f;     // �ɱ�� ���� ��ȯ �ӵ�
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController�� �÷��̾ �Ҵ�Ǿ� ���� �ʽ��ϴ�.");
        }
    }

    void Update()
    {
        // Control Ű�� ������ �ִ� ���� crouchHeight�� ��ȯ, ���� normalHeight�� ���ư�
        float targetHeight = Input.GetKey(KeyCode.LeftControl) ? crouchHeight : normalHeight;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchSpeed);
    }
}
