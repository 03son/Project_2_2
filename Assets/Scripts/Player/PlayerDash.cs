using Photon.Pun;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float moveSpeed = 5f; // 기본 이동 속도 추가
    float speed;

    private CharacterController controller;
    private Transform cameraTransform;

    private Animator animator; // Animator 추가
    private PhotonView pv;
    private Player_Equip playerEquip; // Player_Equip 스크립트 참조
    private PlayerState playerState; // 플레이어 상태 참조
    private PlayerState.playerState state;
    SoundSource soundSource;

    void Start()
    {
        soundSource = GetComponent<SoundSource>();
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        animator = GetComponentInChildren<Animator>(); // Animator 컴포넌트 가져오기
        pv = GetComponent<PhotonView>();
        playerEquip = GetComponent<Player_Equip>(); // Player_Equip 컴포넌트 가져오기
        playerState = GetComponent<PlayerState>(); // PlayerState 스크립트 가져오기
    }

    void Update()
    {
        if (!pv.IsMine && PhotonNetwork.IsConnected)
            return;


        // esc 메뉴가 켜져 있으면 작동 중단
        if (CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu)
        {
            StopMovement();
            return;
        }

        // 플레이어 상태 가져오기
        playerState.GetState(out state);

        // 플레이어 상태가 Survival일 때만 이동 처리
        if (state == PlayerState.playerState.Survival)
        {
            HandleMovement();
        }
        else
        {
            // 상태가 Die일 경우 움직임을 멈춤
            StopMovement();
        }
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
        //= (Input.GetKey(KeyManager.Run_Key) && direction.magnitude > 0) ? dashSpeed : moveSpeed;

        if (!GetComponent<PlayerMove>().playerCrouch.isCrouching)
        {
            if (Input.GetKey(KeyManager.Run_Key) && direction.magnitude > 0)
            {
                speed = dashSpeed;
                soundSource.baseDecibel = 80f;
                Decibel_Bar.instance.Decibel_Value(GetComponent<PlayerMove>().walkSound.volume * 1.8f, false);
            }
        }
        else 
        {
            speed = moveSpeed;
        }
        /*if (!GetComponent<PlayerMove>().playerCrouch.isCrouching && Input.GetKey(KeyManager.Run_Key))
        {
            soundSource.baseDecibel = 80f;
            Decibel_Bar.instance.Decibel_Value(GetComponent<PlayerMove>().walkSound.volume * 1.8f , false);
        }*/

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

    private void StopMovement()
    {
        // 이동을 멈추기 위해 Vector3.zero를 전달
        controller.Move(Vector3.zero);

        // 이동 관련 애니메이션 초기화
        animator.SetBool("isRunning", false);
        animator.SetBool("isRunningWithItem", false);
    }
}
