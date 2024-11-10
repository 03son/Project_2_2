using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    [SerializeField] float speed = 5f;
    [SerializeField] float mouseSpeed = 8f;
    [SerializeField] Transform cameraTransform; // ī�޶��� Transform ����

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

        // Main Camera �ڵ� �Ҵ� (Inspector���� �Ҵ���� �ʾ��� ���)
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
        }
    }

    void Update()
    {
        // ���� �÷��̾ ����
        if (PhotonNetwork.IsConnected)
        {
            if (!photonView.IsMine)
                return;
        }

        //esc â�� ���������� ����
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu)
        {
            HandleMouseLook();
            HandleMovement();
        }
        else //esc â�� ���������� ����
        {
            PlayerVelocity(Vector3.zero);
        }
    }

    // ���콺 ȸ�� ó��
    private void HandleMouseLook()
    {
        if (cameraTransform == null) return;

        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        transform.localRotation = Quaternion.Euler(0, mouseX, 0); // Y�� ȸ�� (�¿� ȸ��)
    }

    // �̵� �� �߷� ó��
    private void HandleMovement()
    {
        if (controller == null || cameraTransform == null) return;

        // �ﰢ���� �̵�
        float moveX = 0; // Input.GetAxisRaw("Horizontal");
        float moveZ = 0; //Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyManager.Front_Key)) moveZ = 1; //��
        if (Input.GetKey(KeyManager.Back_Key)) moveZ = -1; //��
        if (Input.GetKey(KeyManager.Left_Key)) moveX = -1; //��
        if (Input.GetKey(KeyManager.Right_Key)) moveX = 1; //��

        // ī�޶� �������� �̵� ���
        Vector3 direction = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
        direction.y = 0f; // Y�� �̵� ����
        direction.Normalize();

        // �̵� ���� ����
        Vector3 mov = direction * speed;

        PlayerVelocity(mov);
    }
    void PlayerVelocity(Vector3 mov)
    {
        // �߷� ����
        if (controller.isGrounded)
        {
            velocity.y = -2f;  // ���� ���� �� �ణ�� �߷¸� ����
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // ���߿��� �߷� ���� ����
        }

        // ���� �̵� ó��
        controller.Move((mov + velocity) * Time.deltaTime);
    }
}
