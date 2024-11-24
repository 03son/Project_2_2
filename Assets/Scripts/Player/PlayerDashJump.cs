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

        // esc â�� �������� ���� ����
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu)
        {
            HandleMovement();
            HandleDash();
            HandleJump();
        }
    }

    private void HandleMovement()
    {
        // �⺻ �̵� ���� ����
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 direction = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
        direction.y = 0f;
        direction.Normalize();

        // �뽬 ���� �ƴ� ���� �Ϲ� �̵� ó��
        if (!isDashing)
        {
            Vector3 mov = direction * dashSpeed * 0.5f; // �Ϲ� �ӵ��� �뽬 �ӵ��� �������� ����
            controller.Move(mov * Time.deltaTime);
        }
    }

    private void HandleDash()
    {
        if (Input.GetKey(KeyManager.Run_Key) && controller.isGrounded && !isDashing)
        {
            isDashing = true;
            animator.SetBool("isRunning", true); // �޸��� �ִϸ��̼� ����
        }

        if (isDashing)
        {
            Vector3 dashDirection = cameraTransform.forward;
            dashDirection.y = 0f;
            dashDirection.Normalize();

            // �뽬 ó��
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);

            if (Input.GetKeyUp(KeyManager.Run_Key))
            {
                isDashing = false; // �뽬 ����
                animator.SetBool("isRunning", false); // �޸��� �ִϸ��̼� ����
            }
        }
    }

    private void HandleJump()
    {
        // ���� �߷� �� ���� ó��
        if (controller.isGrounded)
        {
            velocity.y = -2f; // �ٴڿ� ���� �� �ణ�� �߷¸� ����
            animator.SetBool("isJumping", false); // ���� �ִϸ��̼� ����

            if (Input.GetKeyDown(KeyManager.Jump_Key))
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                animator.SetBool("isJumping", true); // ���� �ִϸ��̼� ����
                isDashing = false; // �뽬 �� �����ϸ� �뽬 ����
                animator.SetBool("isRunning", false); // �뽬 �ִϸ��̼� ����
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // ĳ���� �̵� ó��
        controller.Move(velocity * Time.deltaTime);
    }
}
