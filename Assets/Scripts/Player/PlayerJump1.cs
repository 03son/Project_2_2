using UnityEngine;
using Photon.Pun;

public class PlayerJump1 : MonoBehaviour
{
    private Rigidbody pRigidbody;
    [SerializeField] private float jumpForce = 100f;
    private bool grounded = false;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private Transform footTransform; // Inspector에서 발 위치 지정
    [SerializeField] private LayerMask groundLayer;

    private Animator animator;

    PlayerState playerState;
    PlayerState.playerState state;

    PhotonView pv;

    private void Start()
    {
        pRigidbody = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        animator = GetComponentInChildren<Animator>();

        playerState = GetComponent<PlayerState>();

        // Rigidbody의 isKinematic 비활성화
        pRigidbody.isKinematic = false;
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnected && !pv.IsMine)
            return;

        // ESC 창 닫힘 상태 및 생존 상태 체크
        playerState.GetState(out state);
        if (!CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Survival)
        {
            Jump();
        }
        Debug.Log($"Grounded: {grounded}");
        Debug.Log($"Rigidbody isKinematic: {pRigidbody.isKinematic}, Use Gravity: {pRigidbody.useGravity}");

    }

    private void FixedUpdate()
    {
        grounded = IsGrounded(); // grounded 상태 업데이트

        if (Input.GetKeyDown(KeyManager.Jump_Key) && grounded)
        {
            Jump();
        }

    }

    private void Jump()
    {

        if (Input.GetKeyDown(KeyManager.Jump_Key))
        {
            Debug.Log("Jump Key Pressed");
        }
        else
        {
            Debug.Log("Jump Key NOT Pressed");
        }

        if (grounded)
        {
            Debug.Log("Player is Grounded");
        }
        else
        {
            Debug.Log("Player is NOT Grounded");
        }
        if (Input.GetKeyDown(KeyManager.Jump_Key) && grounded)
        {


            // Sqrt는 점프 속도를 조절해 낙하의 부자연스러움을 줄임
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpForce * -Physics.gravity.y);
            pRigidbody.AddForce(jumpVelocity, ForceMode.Impulse);
            Debug.Log($"Jump Force Applied: {jumpVelocity}");

            // 점프 애니메이션 추가 (선택 사항)
            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    // 플레이어 바닥과 충돌 체크
    private bool IsGrounded()
    {

        if (footTransform == null)
        {
            Debug.LogError("Foot Transform is not assigned!");
            return false;
        }

        Vector3 origin = footTransform.position + Vector3.down * 0.5f; // 발 위치에서 Ray 시작 + Vector3.down * 0.5f; // 발밑에서 체크 시작

        // Raycast 디버그 시각화
        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.red);

        return Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer);
    }
}
