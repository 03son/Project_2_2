using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float crouchSpeed = 2f;   // 앉을 때 이동 속도
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

    private Animator animator; // Animator 추가
    private bool isWalking;

    void Start()
    {

        playerEquip = GetComponent<Player_Equip>();
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기

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
        
        // Photon View 확인
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            Debug.Log("Not my PhotonView, skipping Update.");
            return;
        }

        mouseSpeed = GameInfo.MouseSensitivity; // 감도 동기화

        // esc 창이 열려있지 않을 때만 움직임 처리
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu)
        {
            HandleMouseLook();
            HandleMovement();
            UpdateWalkingAnimation(); // 워킹 상태 업데이트
        }
        else
        {
            // esc 창이 열려있을 때는 이동 정지
            PlayerVelocity(Vector3.zero, 0f, 0f);
        }
    }

    private void HandleMouseLook()
    {
        if (cameraTransform == null) return;

        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        transform.localRotation = Quaternion.Euler(0, mouseX, 0);
    }

    private void HandleMovement()
    {
        if (controller == null || cameraTransform == null) return;

        // 현재 이동 속도 설정: 앉은 상태인지 여부에 따라 결정
        float currentSpeed = Input.GetKey(KeyManager.SitDown_Key) ? crouchSpeed : normalSpeed;

        float moveX = 0;
        float moveZ = 0;

        if (Input.GetKey(KeyManager.Front_Key)) moveZ = 1; // 앞 (W 키)
        if (Input.GetKey(KeyManager.Back_Key)) moveZ = -1; // 뒤 (S 키)
        if (Input.GetKey(KeyManager.Left_Key)) moveX = -1; // 좌 (A 키)
        if (Input.GetKey(KeyManager.Right_Key)) moveX = 1; // 우 (D 키)

        Vector3 direction = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
        direction.y = 0f;
        direction.Normalize();

        Vector3 mov = direction * currentSpeed;

        PlayerVelocity(mov, moveX, moveZ);

        // 애니메이터에 파라미터 설정
        isWalking = (moveX != 0 || moveZ != 0);

        // 이동 방향에 따라 애니메이션 상태 전환 설정
        if (isWalking)
        {
            if (moveZ > 0) // 앞으로 이동 중일 때 (W 키)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isMovingBackward", false);
            }
            else if (moveZ < 0) // 뒤로 이동 중일 때 (S 키)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isMovingBackward", true);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isMovingBackward", false);
        }

        bool isHoldingItem = playerEquip != null && playerEquip.HasAnyEquippedItem();

        // 걷는 애니메이션 상태 전환 로직
        if (isWalking)
        {
            if (moveZ > 0) // 앞으로 이동 중일 때 (W 키)
            {
                if (isHoldingItem)
                {
                    // 아이템을 들고 앞으로 걷는 경우
                    animator.SetBool("isWalkingWithItem", true);
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isMovingBackward", false);
                }
                else
                {
                    // 아이템 없이 앞으로 걷는 경우
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isWalkingWithItem", false);
                    animator.SetBool("isMovingBackward", false);
                }
            }
            else if (moveZ < 0) // 뒤로 이동 중일 때 (S 키)
            {
                if (isHoldingItem)
                {
                    // 아이템을 들고 뒤로 걷는 경우
                    animator.SetBool("isWalkingWithItem", true);
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isMovingBackward", true);
                }
                else
                {
                    // 아이템 없이 뒤로 걷는 경우
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isWalkingWithItem", false);
                    animator.SetBool("isMovingBackward", true);
                }
            }
        }
        else
        {
            // 걷지 않는 경우
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingWithItem", false);
            animator.SetBool("isMovingBackward", false);
        }
    
}

    private void PlayerVelocity(Vector3 mov, float moveX, float moveZ)
    {
        // 중력 처리
        if (controller.isGrounded)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move((mov + velocity) * Time.deltaTime);

        // 걸음 소리 처리
        if ((moveX != 0 || moveZ != 0) && controller.isGrounded)
        {
            if (!walkSound.isPlaying)
            {
                walkSound.Play();
            }
            walkSound.volume = walkVolume;
            isWalking = true;
        }
        else if (isWalking)
        {
            walkSound.Stop();
            isWalking = false;
        }
    }

    private void UpdateWalkingAnimation()
    {
        // Animator에 값 설정
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
           // Debug.Log($"isWalking: {isWalking}, Animator Parameter: {animator.GetBool("isWalking")}");
        }
    }
}
