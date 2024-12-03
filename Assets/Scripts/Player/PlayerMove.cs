using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float crouchSpeed = 2f;   // ���� �� �̵� �ӵ�
    [SerializeField] float mouseSpeed = 8f;
    [SerializeField] Transform cameraTransform;

    [SerializeField] AudioSource walkSound;
    [SerializeField] AudioClip walkingClip;
    [SerializeField][Range(0f, 1f)] float walkVolume = 0.5f;


    private Player_Equip playerEquip;
    private CharacterController controller;
    private Vector3 velocity;
    private float gravity = -9.81f;
    private float mouseX;

    private Animator animator; // Animator �߰�
    private bool isWalking;

    PlayerState playerState;
    PlayerState.playerState state;

    void Start()
    {

        playerEquip = GetComponent<Player_Equip>();
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }

        playerState = GetComponent<PlayerState>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������

        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
        }

        if (walkSound != null && walkingClip != null)
        {
            walkSound.clip = walkingClip;
            walkSound.loop = true;
            walkSound.volume = walkVolume;
        }
    }

    void Update()
    {
        
        // Photon View Ȯ��
        playerState.GetState(out state);

        // Height �� ���� ����
      /*  if (controller.height != 0.1f)
        {
            controller.height = 0.1f;
        } */
        // Photon View Ȯ��
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            Debug.Log("Not my PhotonView, skipping Update.");
            return;
        }

        mouseSpeed = GameInfo.MouseSensitivity; // ���� ����ȭ

        // esc â�� �������� ���� ���� ������ ó��
        if (!CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Survival)
        {
          //  HandleMouseLook();
            HandleMovement();
            UpdateWalkingAnimation();
        }
        else
        {
            if (state == PlayerState.playerState.Die)
            {
                // esc â�� �������� ���� �̵� ����
                PlayerVelocity(Vector3.zero, 0f, 0f);
            }
        }

        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 1f; // 캐릭터 중심에서 약간 위로 시작
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 2.5f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
       

            if (slopeAngle > 10) // 10도 이상이면 계단으로 판단
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    animator.SetBool("isClimbingUpStairs", true);
                    animator.SetBool("isClimbingDownStairs", false);
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    animator.SetBool("isClimbingUpStairs", false);
                    animator.SetBool("isClimbingDownStairs", true);
                }
            }
            else
            {
                animator.SetBool("isClimbingUpStairs", false);
                animator.SetBool("isClimbingDownStairs", false);
            }
        }

    }




  /*  private void HandleMouseLook()
    {
        if (cameraTransform == null) return;

        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        transform.localRotation = Quaternion.Euler(0, mouseX, 0);
    } */

    private void HandleMovement()
    {
        if (controller == null || cameraTransform == null) return;

        // ���� �̵� �ӵ� ����: ���� �������� ���ο� ���� ����
        float currentSpeed = Input.GetKey(KeyManager.SitDown_Key) ? crouchSpeed : normalSpeed;

        float moveX = 0;
        float moveZ = 0;

        if (Input.GetKey(KeyManager.Front_Key)) moveZ = 1; // �� (W Ű)
        if (Input.GetKey(KeyManager.Back_Key)) moveZ = -1; // �� (S Ű)
        if (Input.GetKey(KeyManager.Left_Key)) moveX = -1; // �� (A Ű)
        if (Input.GetKey(KeyManager.Right_Key)) moveX = 1; // �� (D Ű)

        Vector3 direction = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
        direction.y = 0f;
        direction.Normalize();

        Vector3 mov = direction * currentSpeed;

        PlayerVelocity(mov, moveX, moveZ);

        // �ִϸ����Ϳ� �Ķ���� ����
        isWalking = (moveX != 0 || moveZ != 0);

        bool isHoldingItem = playerEquip != null && playerEquip.HasAnyEquippedItem();

        // �ȴ� �ִϸ��̼� ���� ��ȯ ����
        // �̵� ���⿡ ���� �ִϸ��̼� ���� ��ȯ ����
        if (isWalking)
        {
            if (moveZ > 0) // ������ �̵� ���� �� (W Ű)
            {
                if (isHoldingItem)
                {
                    animator.SetBool("isWalkingWithItem", true);
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isMovingBackward", true);
                }
                else
                {
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isWalkingWithItem", false);
                    animator.SetBool("isMovingBackward", false);
                }
            }
            else if (moveZ < 0) // �ڷ� �̵� ���� �� (S Ű)
            {
                if (isHoldingItem)
                {
                    animator.SetBool("isWalkingWithItem", true);
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isMovingBackward", true);
                }
                else
                {
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isWalkingWithItem", false);
                    animator.SetBool("isMovingBackward", true);
                }
            }
        }
        else
        {
            // ���� �ʴ� ��� ��� ���¸� �ʱ�ȭ�ϰ�, �������� ��� ���� ���� ������ ���̵� ���·� ��ȯ
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingWithItem", false);
            animator.SetBool("isMovingBackward", false);

            if (isHoldingItem)
            {
                // �������� ��� ���� �� ������ ���̵� ���·� ��ȯ
                animator.SetBool("isItemIdle", true);
            }
            else
            {
                animator.SetBool("isItemIdle", false);
            }
        }
    }

    

    private void PlayerVelocity(Vector3 mov, float moveX, float moveZ)
    {
        // �߷� ó��
        if (controller.isGrounded)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move((mov + velocity) * Time.deltaTime);

        playerState.GetState(out state);
            // ���� �Ҹ� ó��
        if ((moveX != 0 || moveZ != 0) && controller.isGrounded && state == PlayerState.playerState.Survival)
        {
            if (!walkSound.isPlaying)
            {
                walkSound.Play();
            }
            walkSound.volume = walkVolume;
            isWalking = true;
            if (!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
            {
                if (MonsterAI.Instance != null)
                {
                    photonView.RPC("SendDecibelToMaster", RpcTarget.MasterClient, walkSound.volume, transform.position);
                }
                else
                {
                    Debug.Log("몬스터 없음2");
                }
            }
        }
        else
        {
            walkSound.Stop();
            isWalking = false;
        }
    }
    [PunRPC]
    public void SendDecibelToMaster(float decibel, Vector3 playerPosition)
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
        {
            MonsterAI.Instance.HandleItemSound(playerPosition);
        }
    }
    private void UpdateWalkingAnimation()
    {
        // Animator�� �� ����
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
           // Debug.Log($"isWalking: {isWalking}, Animator Parameter: {animator.GetBool("isWalking")}");
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("���ο� ���� : " + newMasterClient.NickName);
    }


}
