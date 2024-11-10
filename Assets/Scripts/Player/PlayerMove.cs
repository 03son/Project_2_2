using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    [SerializeField] float speed = 5f;
    [SerializeField] float mouseSpeed = 8f;
    [SerializeField] Transform cameraTransform; // 카메라의 Transform 참조

    private CharacterController controller;
    private Vector3 velocity;
    private float gravity = -9.81f;
    private float mouseX;

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!photonView.IsMine)
                return;
        }

        controller = GetComponent<CharacterController>();

        // Main Camera 자동 할당 (Inspector에서 할당되지 않았을 경우)
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
        }
    }

    void Update()
    {
        // 로컬 플레이어만 제어
        if (PhotonNetwork.IsConnected)
        {
            if (!photonView.IsMine)
                return;
        }

        //esc 창이 닫혀있으면 실행
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu)
        {
            HandleMouseLook();
            HandleMovement();
        }
        else //esc 창이 열려있으면 실행
        {
            PlayerVelocity(Vector3.zero);
        }
    }

    // 마우스 회전 처리
    private void HandleMouseLook()
    {
        if (cameraTransform == null) return;

        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        transform.localRotation = Quaternion.Euler(0, mouseX, 0); // Y축 회전 (좌우 회전)
    }

    // 이동 및 중력 처리
    private void HandleMovement()
    {
        if (controller == null || cameraTransform == null) return;

        // 즉각적인 이동
        float moveX = 0; // Input.GetAxisRaw("Horizontal");
        float moveZ = 0; //Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyManager.Front_Key)) moveZ = 1; //앞
        if (Input.GetKey(KeyManager.Back_Key)) moveZ = -1; //뒤
        if (Input.GetKey(KeyManager.Left_Key)) moveX = -1; //좌
        if (Input.GetKey(KeyManager.Right_Key)) moveX = 1; //우

        // 카메라 방향으로 이동 계산
        Vector3 direction = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
        direction.y = 0f; // Y축 이동 제거
        direction.Normalize();

        // 이동 벡터 적용
        Vector3 mov = direction * speed;

        PlayerVelocity(mov);
    }
    void PlayerVelocity(Vector3 mov)
    {
        // 중력 적용
        if (controller.isGrounded)
        {
            velocity.y = -2f;  // 땅에 있을 때 약간의 중력만 적용
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // 공중에서 중력 가속 적용
        }

        // 최종 이동 처리
        controller.Move((mov + velocity) * Time.deltaTime);
    }
}
