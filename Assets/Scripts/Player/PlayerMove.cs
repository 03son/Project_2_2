using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    [SerializeField] float speed = 5f;
    [SerializeField] float mouseSpeed = 8f;
    [SerializeField] Transform cameraTransform;

    [SerializeField] AudioSource walkSound;
    [SerializeField] AudioClip walkingClip;
    [SerializeField][Range(0f, 1f)] float walkVolume = 0.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private float gravity = -9.81f;
    private float mouseX;

    private Animator animator; // Animator �߰�
    private bool isWalking;

    void Start()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
        
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
        }

        if (walkSound != null && walkingClip != null)
        {
            walkSound.clip = walkingClip;
            walkSound.loop = true;
            walkSound.volume = walkVolume;
        }
    }

    void Update()
    {

        // Height �� ���� ����
        if (controller.height != 0.1f)
        {
            controller.height = 0.1f;
        }
        // Photon View Ȯ��
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            Debug.Log("Not my PhotonView, skipping Update.");
            return;
        }

        mouseSpeed = GameInfo.MouseSensitivity; //���� ����ȭ

        // esc â�� �������� ���� ���� ������ ó��
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu)
        {
            HandleMouseLook();
            HandleMovement();
            UpdateWalkingAnimation(); // ��ŷ ���� ������Ʈ
        }
        else
        {
            // esc â�� �������� ���� �̵� ����
            PlayerVelocity(Vector3.zero, 0f, 0f);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            if (slopeAngle > 20) // 20�� �̻��̸� ������� �Ǵ�
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    animator.SetBool("isClimbingUpStairs", true);
                    animator.SetBool("isClimbingDownStairs", false);
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    animator.SetBool("isClimbingUpStairs", false);
                    animator.SetBool("isClimbingDownStairs", true);
                }
            }
            else
            {
                animator.SetBool("isClimbingUpStairs", false);
                animator.SetBool("isClimbingDownStairs", false);
            }
        }
    }

    private void HandleMouseLook()
    {
        if (cameraTransform == null) return;

        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        transform.localRotation = Quaternion.Euler(0, mouseX, 0);
    }

    private void HandleMovement()
    {
        if (controller == null || cameraTransform == null) return;

        float moveX = 0; // Input.GetAxisRaw("Horizontal");
        float moveZ = 0; //Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyManager.Front_Key)) moveZ = 1; //��
        if (Input.GetKey(KeyManager.Back_Key)) moveZ = -1; //��
        if (Input.GetKey(KeyManager.Left_Key)) moveX = -1; //��
        if (Input.GetKey(KeyManager.Right_Key)) moveX = 1; //��

        Vector3 direction = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
        direction.y = 0f;
        direction.Normalize();

        Vector3 mov = direction * speed;

        PlayerVelocity(mov, moveX, moveZ);

        // �ִϸ����Ϳ� �Ķ���� ����
        isWalking = (moveX != 0 || moveZ != 0);
    }

    private void PlayerVelocity(Vector3 mov, float moveX, float moveZ)
    {
        // �߷� ó��
        if (controller.isGrounded)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move((mov + velocity) * Time.deltaTime);

        // ���� �Ҹ� ó��
        if ((moveX != 0 || moveZ != 0) && controller.isGrounded)
        {
            if (!walkSound.isPlaying)
            {
                walkSound.Play();
            }
            walkSound.volume = walkVolume;
            isWalking = true;
        }
        else if (isWalking)
        {
            walkSound.Stop();
            isWalking = false;
        }
    }

    private void UpdateWalkingAnimation()
    {
        // Animator�� �� ����
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
            //Debug.Log($"isWalking: {isWalking}, Animator Parameter: {animator.GetBool("isWalking")}");
        }
    }
}
