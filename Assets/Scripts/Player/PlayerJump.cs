using Photon.Pun;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] float jumpForce = 5f; // 점프 힘
    [SerializeField] float gravity = -15f; // 중력 값 조정
    [SerializeField] private float groundCheckDistance = 0.2f; // 바닥 체크 거리
    [SerializeField] private float jumpBufferTime = 0.2f; // 점프 입력 버퍼 시간

    private CharacterController controller;
    private Vector3 velocity;
    private Animator animator;

    PlayerState playerState;
    PlayerState.playerState state;

    private bool jumpRequested = false; // 점프 요청 플래그
    private float lastJumpTime; // 점프 입력 시간 기록

    PhotonView pv;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        pv = GetComponent<PhotonView>();
        animator = GetComponentInChildren<Animator>();

        playerState = GetComponent<PlayerState>();
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected && !pv.IsMine)
            return;

        // ESC 창 닫힘 상태 및 생존 상태 체크
        playerState.GetState(out state);
        if (!CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Survival)
        {
            HandleJump();
        }

        // 점프 키 입력 감지 및 버퍼 저장
        if (Input.GetKeyDown(KeyManager.Jump_Key))
        {
            lastJumpTime = Time.time; // 점프 입력 시간 기록
        }
    }

    private void HandleJump()
    {
        bool grounded = IsGrounded();

        //Debug.Log("IsGrounded 상태: " + grounded);

        if (grounded)
        {
            // 바닥에 있을 때 중력 초기화
            velocity.y = 0f;
            animator.SetBool("isJumping", false);

            // 점프 버퍼 처리
            if (Time.time - lastJumpTime <= jumpBufferTime)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // 점프 처리
                animator.SetBool("isJumping", true);
                lastJumpTime = -1f; // 점프 처리 후 버퍼 초기화
                Debug.Log("점프 발생! Velocity Y: " + velocity.y);
            }
        }
        else
        {
            // 공중에 있을 때 중력 적용
            velocity.y += gravity * Time.deltaTime;
        }

        // 이동 처리
        Vector3 move = new Vector3(0, velocity.y, 0);
        controller.Move(move * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        return controller.isGrounded;
    }

}
