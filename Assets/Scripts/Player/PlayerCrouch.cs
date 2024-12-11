using Photon.Pun;
using UnityEngine;

public class PlayerCrouch : MonoBehaviourPun
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

        if (!photonView.IsMine)
        {
            // 로컬 플레이어가 아니면 입력 처리를 하지 않음
            enabled = false;
            return;
        }

        if (characterController == null)
        {
            Debug.LogError("CharacterController가 할당되지 않았습니다.");
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
        if (!photonView.IsMine) return; // 로컬 플레이어만 입력 처리

        // Control Ű �Է� ���� Ȯ��
        bool isCrouching = Input.GetKey(KeyManager.SitDown_Key);
        float moveSpeed = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).sqrMagnitude;

        bool isEquipped = HasAnyEquippedItem(); // 싱글톤 대신 로컬 상태 확인 // Player_Equip 싱글톤에서 상태 확인
        animator.SetBool("isEquipped", isEquipped);
        animator.SetBool("isCrouching", isCrouching && isEquipped); // 앉기 키 + 장착 여부 동시 확인

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

    // 아이템 이큅 상태 확인 메서드 (Player_Equip의 equipItem 참조)
    bool HasAnyEquippedItem()
    {
        if (!photonView.IsMine) return false; // 로컬 플레이어만 확인

        // 현재 객체에서 아이템 장착 상태를 확인
        ItemObject[] equippedItems = equipItem.GetComponentsInChildren<ItemObject>();
        bool isEquipped = equippedItems.Length > 0;

        // 로컬 상태를 네트워크에 동기화
        photonView.RPC("SyncHasEquippedItem", RpcTarget.All, isEquipped);

        return isEquipped;
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
