
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

        //esc 창이 닫혀있을 때만 점프
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu)
        {
            // 중력 처리 및 점프
            if (controller.isGrounded)
            {
                velocity.y = -2f; // 바닥에 있을 때 약간의 중력만 적용

                if (Input.GetButtonDown("Jump"))
                {
                    velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
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
}

