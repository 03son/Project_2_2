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

    private Animator animator; // Animator 추가
    private bool isWalking;

    void Start()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }

        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>(); // Animator 컴포넌트 가져오기

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
        }

        if (walkSound != null && walkingClip != null)
        {
            walkSound.clip = walkingClip;
            walkSound.loop = true;



        }
    }

    void Update()
    {
        // Photon View 확인
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            Debug.Log("Not my PhotonView, skipping Update.");
            return;
        }

        // esc 창이 열려있지 않을 때만 움직임 처리
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu)
        {
            HandleMouseLook();
            HandleMovement();

         
        }
        else
        {
            // esc 창이 열려있을 때는 이동 정지
            PlayerVelocity(Vector3.zero, 0f, 0f);
        }
    }



    // 애니메이션 상태 업데이트
    void UpdateAnimationStates()
    {
        bool isWalking = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // PlayerAnimationController 호출
        GetComponentInChildren<PlayerAnimationController>().SetWalking(isWalking);
        GetComponentInChildren<PlayerAnimationController>().SetRunning(isRunning);
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

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 direction = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
        direction.y = 0f;
        direction.Normalize();

        Vector3 mov = direction * speed;

        // Animator 업데이트
        bool walking = (moveX != 0 || moveZ != 0);
        animator.SetBool("isWalking", walking);
        Debug.Log($"isWalking: {walking}");

        PlayerVelocity(mov, moveX, moveZ);
    }


    void PlayerVelocity(Vector3 mov, float moveX, float moveZ)
    {
        // 중력 처리
        if (controller.isGrounded)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move((mov + velocity) * Time.deltaTime);

        // 걸음 소리 처리
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
}
