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
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Survival)
        {
            HandleJump();
        }
    }

    private void HandleJump()
    {
        // �߷� ó�� �� ����
        if (controller.isGrounded)
        {
            velocity.y = -2f; // �ٴڿ� ���� �� �ణ�� �߷¸� ����
            animator.SetBool("isJumping", false); // ���� ���� ����

            if (Input.GetKeyDown(KeyManager.Jump_Key))
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                animator.SetBool("isJumping", true); // ���� �ִϸ��̼� ����
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
