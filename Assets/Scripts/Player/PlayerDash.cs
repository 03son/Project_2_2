using Photon.Pun;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashSpeed = 10f;

    private CharacterController controller;
    private Transform cameraTransform;

    private Animator animator; // Animator 추가
    PhotonView pv;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        animator = GetComponentInChildren<Animator>(); // Animator 컴포넌트 가져오기
        pv = GetComponent<PhotonView>();
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
            animator.SetBool("isRunning", false); // 스프린트 종료
        }
    }

    private void HandleDash()
    {
        Vector3 dashDirection = cameraTransform.forward;
        dashDirection.y = 0f;
        dashDirection.Normalize();

        // 스프린트 처리
        controller.Move(dashDirection * dashSpeed * Time.deltaTime);

        // Animator 업데이트 (달리기 상태로 전환)
        animator.SetBool("isRunning", true);
    }
}
