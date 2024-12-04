using Photon.Pun;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float moveSpeed = 5f; // 기본 이동 속도 추가

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

        HandleMovement();
    }

    private void HandleMovement()
    {
        // 플레이어 움직임 입력 받기
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 이동 벡터 계산
        Vector3 direction = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        direction.y = 0f; // 수직 방향 제거
        direction.Normalize(); // 방향 벡터 정규화

        // 이동 속도 결정 (Shift 키를 누르고 있을 때만 스프린트 속도로 이동)
        float speed = (Input.GetKey(KeyManager.Run_Key) && direction.magnitude > 0) ? dashSpeed : moveSpeed;

        // 이동 처리
        controller.Move(direction * speed * Time.deltaTime);

        // 아이템 장착 여부에 따른 애니메이션 설정
        if (speed == dashSpeed)
        {
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
        else
        {
            // 스프린트 종료
            animator.SetBool("isRunning", false);
            animator.SetBool("isRunningWithItem", false);
        }
    }
}
