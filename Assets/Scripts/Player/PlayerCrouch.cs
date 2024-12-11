using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public float crouchHeight = 0.125f;    // �ɾ��� �� ����
    public float normalHeight = 0.25f;    // �� ���� �� ����
    public float crouchSpeed = 0.1f;     // �ɱ�� ���� ��ȯ �ӵ�
    private CharacterController characterController;
    private Animator animator;           // Animator �߰�

    public Transform cameraTransform;    // FPS ī�޶� Transform �߰�

    private Vector3 normalCenter;        // �⺻ center �� ����
    private Vector3 crouchCenter;        // �ɾ��� �� center ��
    private float cameraYOffset;         // ī�޶��� �ʱ� Y ��ġ

    PlayerState playerState;
    PlayerState.playerState state;
    public bool isCrouching { get; private set; } = false; // 앉기 상태 변수

    void Start()
    {

        playerState = GetComponent<PlayerState>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������

        if (characterController == null)
        {
            Debug.LogError("CharacterController�� �÷��̾ �Ҵ�Ǿ� ���� �ʽ��ϴ�.");
            return;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("ī�޶� �Ҵ�Ǿ� ���� �ʽ��ϴ�.");
            return;
        }

        // �⺻ ���̿� �ɾ��� �� ���̿� �´� center �� ���
        normalCenter = characterController.center;
        crouchCenter = new Vector3(normalCenter.x, normalCenter.y - (normalHeight - crouchHeight) / 2, normalCenter.z);

        // ī�޶��� �ʱ� Y ������ ����
        cameraYOffset = cameraTransform.localPosition.y;
    }

    void Update()
    {
        if (characterController == null || animator == null || cameraTransform == null) return;

        // Control Ű �Է� ���� Ȯ��
        bool isCrouching = Input.GetKey(KeyManager.SitDown_Key);
        float moveSpeed = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).sqrMagnitude;

        // ���� ���¿��� �̵��� �ִ��� Ȯ��
        bool isMovingWhileCrouched = isCrouching && moveSpeed > 0.01f;

        // �ִϸ����� �Ķ���� ���� (isCrouching�� bool�� ���)
        animator.SetBool("isCrouching", isCrouching);
        animator.SetFloat("crouchMoveSpeed", moveSpeed);
        animator.SetBool("isMovingWhileCrouched", isMovingWhileCrouched);

        // ���̿� center ����
        float targetHeight = isCrouching ? crouchHeight : normalHeight;
        Vector3 targetCenter = isCrouching ? crouchCenter : normalCenter;

        playerState.GetState(out state);
        if (CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Die)
            return;

        // Control Ű�� ������ �ִ� ���� crouchHeight�� ��ȯ, ���� normalHeight�� ���ư�
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchSpeed);
        characterController.center = Vector3.Lerp(characterController.center, targetCenter, crouchSpeed);

        // ī�޶� ��ġ ���� (�÷��̾��� ���̿� ����)
        float targetCameraY = isCrouching ? cameraYOffset - (normalHeight - crouchHeight) : cameraYOffset;
        Vector3 cameraPosition = cameraTransform.localPosition;
        cameraPosition.y = Mathf.Lerp(cameraTransform.localPosition.y, targetCameraY, crouchSpeed);
        cameraTransform.localPosition = cameraPosition;
    }

    public void ToggleCrouch(bool crouching)
    {
        isCrouching = crouching;

        if (crouching)
        {
            Debug.Log("Player is now crouching.");
        }
        else
        {
            Debug.Log("Player is no longer crouching.");
        }
    }
}