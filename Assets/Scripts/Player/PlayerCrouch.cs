using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public float crouchHeight = 1.0f;    // �ɾ��� �� ����
    public float normalHeight = 2.0f;    // �� ���� �� ����
    public float crouchSpeed = 0.1f;     // �ɱ�� ���� ��ȯ �ӵ�
    private CharacterController characterController;

    PlayerState playerState;
    PlayerState.playerState state;

    void Start()
    {
        playerState = GetComponent<PlayerState>();
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController�� �÷��̾ �Ҵ�Ǿ� ���� �ʽ��ϴ�.");
        }
    }

    void Update()
    {
        playerState.GetState(out state);
        if (Camera.main.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Die)
            return;

        // Control Ű�� ������ �ִ� ���� crouchHeight�� ��ȯ, ���� normalHeight�� ���ư�
        float targetHeight = Input.GetKey(KeyCode.LeftControl) ? crouchHeight : normalHeight;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchSpeed);
    }
}
