using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private Animator animator; // Animator �߰�

    PlayerState playerState;
    PlayerState.playerState state;

    private bool jumpRequested = false; // ���� ��û�� �����ϴ� ����


    PhotonView pv;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        pv = GetComponent<PhotonView>();
        animator = GetComponentInChildren<Animator>(); // Animator ������Ʈ ��������

        playerState = GetComponent<PlayerState>();
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!pv.IsMine)
                return;
        }

        // esc â�� �������� �� && ����� �� ����
        playerState.GetState(out state);
        if (!CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Survival)
        {
            HandleJump();
        }

        // ���� Ű �Է� ����
        if (Input.GetKeyDown(KeyManager.Jump_Key))
        {
            jumpRequested = true; // ���� ��û ����
        }

    }

    private void HandleJump()
    {
        bool grounded = IsGrounded();

        // �ٴ� ���� ���� ���
        Debug.Log("IsGrounded ����: " + grounded);
        Debug.Log("Velocity Y ����: " + velocity.y);

        if (grounded)
        {
            velocity.y = -2f;
            animator.SetBool("isJumping", false);

            if (jumpRequested)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                animator.SetBool("isJumping", true);
                jumpRequested = false;

                // ���� �α�
                Debug.Log("���� �߻�! Velocity Y: " + velocity.y);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }


    private bool IsGrounded()
    {
        float groundCheckDistance = 1f; // �ٴ� üũ �Ÿ� (�������� ��¦ �ø�)
        Vector3 origin = transform.position + Vector3.up * 1f;

        // Raycast ����� �� �߰�
        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.red);

        return Physics.Raycast(origin, Vector3.down, groundCheckDistance);
    }



}
