using Photon.Pun;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashSpeed = 10f;

    private CharacterController controller;
    private Transform cameraTransform;

    private Animator animator; // Animator 추가
    private PhotonView pv;
    private Player_Equip playerEquip; // Player_Equip 스크립트 참조

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        animator = GetComponentInChildren<Animator>(); // Animator 컴포넌트 가져오기
        pv = GetComponent<PhotonView>();
        playerEquip = GetComponent<Player_Equip>(); // Player_Equip 컴포넌트 가져오기
    }

    void Update()
    {
        if (!pv.IsMine && PhotonNetwork.IsConnected)
            return;

        // 스프린트 기능 (왼쪽 Shift 키를 누르고 있는 동안 작동)
        if (Input.GetKey(KeyManager.Run_Key))
        {
            HandleDash();
        }
        else
        {
            // 스프린트 종료
            animator.SetBool("isRunning", false);
            animator.SetBool("isRunningWithItem", false);
        }
    }

    private void HandleDash()
    {
        Vector3 dashDirection = cameraTransform.forward;
        dashDirection.y = 0f;
        dashDirection.Normalize();

        // 스프린트 처리
        controller.Move(dashDirection * dashSpeed * Time.deltaTime);

        // 아이템 장착 여부에 따라 애니메이션 설정
        if (playerEquip != null && playerEquip.HasAnyEquippedItem())
        {
            animator.SetBool("isRunningWithItem", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isRunningWithItem", false);
        }
    }
}
