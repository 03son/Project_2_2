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
    private Animator animator; // Animator 추가

    PlayerState playerState;
    PlayerState.playerState state;

    PhotonView pv;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        pv = GetComponent<PhotonView>();
        animator = GetComponentInChildren<Animator>(); // Animator 컴포넌트 가져오기

        playerState = GetComponent<PlayerState>();
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!pv.IsMine)
                return;
        }

        // esc 창이 닫혀있을 때 && 살았을 때 동작
        playerState.GetState(out state);
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Survival)
        {
            HandleJump();
        }
    }

    private void HandleJump()
    {
        // 중력 처리 및 점프
        if (controller.isGrounded)
        {
            velocity.y = -2f; // 바닥에 있을 때 약간의 중력만 적용
            animator.SetBool("isJumping", false); // 점프 상태 해제

            if (Input.GetKeyDown(KeyManager.Jump_Key))
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                animator.SetBool("isJumping", true); // 점프 애니메이션 실행
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
