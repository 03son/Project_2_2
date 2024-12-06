using UnityEngine;
using Photon.Pun;

public class PlayerJump1 : MonoBehaviour
{
    private Rigidbody pRigidbody;
    [SerializeField] private float jumpForce = 100f;
    private bool grounded = false;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private Transform footTransform; // Inspector���� �� ��ġ ����
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

        // Rigidbody�� isKinematic ��Ȱ��ȭ
        pRigidbody.isKinematic = false;
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnected && !pv.IsMine)
            return;

        // ESC â ���� ���� �� ���� ���� üũ
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
        grounded = IsGrounded(); // grounded ���� ������Ʈ

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


            // Sqrt�� ���� �ӵ��� ������ ������ ���ڿ��������� ����
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpForce * -Physics.gravity.y);
            pRigidbody.AddForce(jumpVelocity, ForceMode.Impulse);
            Debug.Log($"Jump Force Applied: {jumpVelocity}");

            // ���� �ִϸ��̼� �߰� (���� ����)
            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    // �÷��̾� �ٴڰ� �浹 üũ
    private bool IsGrounded()
    {

        if (footTransform == null)
        {
            Debug.LogError("Foot Transform is not assigned!");
            return false;
        }

        Vector3 origin = footTransform.position + Vector3.down * 0.5f; // �� ��ġ���� Ray ���� + Vector3.down * 0.5f; // �߹ؿ��� üũ ����

        // Raycast ����� �ð�ȭ
        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.red);

        return Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer);
    }
}
