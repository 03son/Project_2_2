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


            if (animator == null)
            {
                Debug.LogError("Animator component not found! Check if the Animator exists in the children of this object.");
            }
            else
            {
                Debug.Log($"Animator found and connected: {animator.gameObject.name}");
            }
            walkSound.volume = walkVolume;
        }
    }

    void Update()
    {



        if (animator != null)
        {
            bool walking = (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
            animator.SetBool("isWalking", walking);
            Debug.Log($"Set isWalking: {walking}");

            bool currentParam = animator.GetBool("isWalking");
            Debug.Log($"Animator Parameter isWalking: {currentParam}");

            if (walking != currentParam)
            {
                Debug.LogError("Animator parameter 'isWalking' is not updating properly.");
            }
        }


        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
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
            PlayerVelocity(Vector3.zero, 0f, 0f);
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
