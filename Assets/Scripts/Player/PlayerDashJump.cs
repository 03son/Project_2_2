using Photon.Pun;
using UnityEngine;

public class PlayerDashJump : MonoBehaviour
{
    [SerializeField] float dashSpeed = 5;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float gravity = -9.81f;

    private CharacterController controller;
    private Transform cameraTransform;
    private Animator animator;
    private PhotonView pv;

    private Vector3 velocity;
    private bool isDashing = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        animator = GetComponentInChildren<Animator>();
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!pv.IsMine && PhotonNetwork.IsConnected)
            return;

        // esc 창이 닫혀있을 때만 동작
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu)
        {
            HandleMovement();
            HandleDash();
            HandleJump();
        }
    }

    private void HandleMovement()
    {
        // 기본 이동 방향 설정
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 direction = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
        direction.y = 0f;
        direction.Normalize();

        // 대쉬 중이 아닐 때만 일반 이동 처리
        if (!isDashing)
        {
            Vector3 mov = direction * dashSpeed * 0.5f; // 일반 속도는 대쉬 속도의 절반으로 설정
            controller.Move(mov * Time.deltaTime);
        }
    }

    private void HandleDash()
    {
        if (Input.GetKey(KeyManager.Run_Key) && controller.isGrounded && !isDashing)
        {
            isDashing = true;
            animator.SetBool("isRunning", true); // 달리기 애니메이션 시작
        }

        if (isDashing)
        {
            Vector3 dashDirection = cameraTransform.forward;
            dashDirection.y = 0f;
            dashDirection.Normalize();

            // 대쉬 처리
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);

            if (Input.GetKeyUp(KeyManager.Run_Key))
            {
                isDashing = false; // 대쉬 종료
                animator.SetBool("isRunning", false); // 달리기 애니메이션 종료
            }
        }
    }

    private void HandleJump()
    {
        // 점프 중력 및 점프 처리
        if (controller.isGrounded)
        {
            velocity.y = -2f; // 바닥에 있을 때 약간의 중력만 적용
            animator.SetBool("isJumping", false); // 점프 애니메이션 종료

            if (Input.GetKeyDown(KeyManager.Jump_Key))
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                animator.SetBool("isJumping", true); // 점프 애니메이션 시작
                isDashing = false; // 대쉬 중 점프하면 대쉬 종료
                animator.SetBool("isRunning", false); // 대쉬 애니메이션 종료
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // 캐릭터 이동 처리
        controller.Move(velocity * Time.deltaTime);
    }
}
