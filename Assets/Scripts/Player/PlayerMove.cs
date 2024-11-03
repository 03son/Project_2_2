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
    [SerializeField][Range(0f, 1f)] float walkVolume = 0.5f; // 볼륨 조절

    private CharacterController controller;
    private Vector3 velocity;
    private float gravity = -9.81f;
    private float mouseX;
    private bool isWalking;

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!photonView.IsMine)
                return;
        }

        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
        }

        if (walkSound != null && walkingClip != null)
        {
            walkSound.clip = walkingClip;
            walkSound.loop = true;
            walkSound.volume = walkVolume; // 초기 볼륨 설정
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!photonView.IsMine)
                return;
        }

        HandleMouseLook();
        HandleMovement();
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

        if (controller.isGrounded)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move((mov + velocity) * Time.deltaTime);

        // 소리 재생 및 볼륨 설정
        if ((moveX != 0 || moveZ != 0) && controller.isGrounded)
        {
            if (!walkSound.isPlaying)
            {
                walkSound.Play();
            }
            walkSound.volume = walkVolume; // 볼륨 업데이트
            isWalking = true;
        }
        else if (isWalking)
        {
            walkSound.Stop();
            isWalking = false;
        }
    }
}
