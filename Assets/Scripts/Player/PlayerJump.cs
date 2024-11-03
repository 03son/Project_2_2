
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

    PhotonView pv;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        pv = GetComponent<PhotonView>();

    }

    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!pv.IsMine)
                return;
        }

        //esc â�� �������� ���� ����
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu)
        {
            // �߷� ó�� �� ����
            if (controller.isGrounded)
            {
                velocity.y = -2f; // �ٴڿ� ���� �� �ణ�� �߷¸� ����

                if (Input.GetButtonDown("Jump"))
                {
                    velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
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
}

