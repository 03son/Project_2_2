using Photon.Pun;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] float jumpForce = 5f; // ���� ��
    [SerializeField] float gravity = -15f; // �߷� �� ����
    [SerializeField] private float groundCheckDistance = 0.2f; // �ٴ� üũ �Ÿ�
    [SerializeField] private float jumpBufferTime = 0.2f; // ���� �Է� ���� �ð�

    private CharacterController controller;
    private Vector3 velocity;
    private Animator animator;

    PlayerState playerState;
    PlayerState.playerState state;

    private bool jumpRequested = false; // ���� ��û �÷���
    private float lastJumpTime; // ���� �Է� �ð� ���

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

        // ESC â ���� ���� �� ���� ���� üũ
        playerState.GetState(out state);
        if (!CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Survival)
        {
            HandleJump();
        }

        // ���� Ű �Է� ���� �� ���� ����
        if (Input.GetKeyDown(KeyManager.Jump_Key))
        {
            lastJumpTime = Time.time; // ���� �Է� �ð� ���
        }
    }

    private void HandleJump()
    {
        bool grounded = IsGrounded();

        //Debug.Log("IsGrounded ����: " + grounded);

        if (grounded)
        {
            // �ٴڿ� ���� �� �߷� �ʱ�ȭ
            velocity.y = 0f;
            animator.SetBool("isJumping", false);

            // ���� ���� ó��
            if (Time.time - lastJumpTime <= jumpBufferTime)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // ���� ó��
                animator.SetBool("isJumping", true);
                lastJumpTime = -1f; // ���� ó�� �� ���� �ʱ�ȭ
                Debug.Log("���� �߻�! Velocity Y: " + velocity.y);
            }
        }
        else
        {
            // ���߿� ���� �� �߷� ����
            velocity.y += gravity * Time.deltaTime;
        }

        // �̵� ó��
        Vector3 move = new Vector3(0, velocity.y, 0);
        controller.Move(move * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        return controller.isGrounded;
    }

}
