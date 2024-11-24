using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public float crouchHeight = 0.05f;    // �ɾ��� �� ����
    public float normalHeight = 0.1f;    // �� ���� �� ����
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
        float targetHeight = Input.GetKey(KeyManager.SitDown_Key) ? crouchHeight : normalHeight;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchSpeed);
    }
}
