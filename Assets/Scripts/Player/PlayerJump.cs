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

    private bool jumpRequested = false; // 점프 요청을 저장하는 변수


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
        if (!CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Survival)
        {
            HandleJump();
        }

        // 점프 키 입력 감지
        if (Input.GetKeyDown(KeyManager.Jump_Key))
        {
            jumpRequested = true; // 점프 요청 저장
        }

    }

    private void HandleJump()
    {
        bool grounded = IsGrounded();

        // 바닥 감지 상태 출력
        Debug.Log("IsGrounded 상태: " + grounded);
        Debug.Log("Velocity Y 상태: " + velocity.y);

        if (grounded)
        {
            velocity.y = -2f;
            animator.SetBool("isJumping", false);

            if (jumpRequested)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                animator.SetBool("isJumping", true);
                jumpRequested = false;

                // 점프 로그
                Debug.Log("점프 발생! Velocity Y: " + velocity.y);
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
        float groundCheckDistance = 1f; // 바닥 체크 거리 (기존보다 살짝 늘림)
        Vector3 origin = transform.position + Vector3.up * 1f;

        // Raycast 디버그 선 추가
        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.red);

        return Physics.Raycast(origin, Vector3.down, groundCheckDistance);
    }



}
