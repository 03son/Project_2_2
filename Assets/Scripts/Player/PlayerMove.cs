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
        if (photonView.IsMine)
        {
            controller = GetComponent<CharacterController>();

            // Main Camera �ڵ� �Ҵ� (Inspector���� �Ҵ���� �ʾ��� ���)
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main?.transform;
            }
        }
    }

    void Update()
    {
        // ���� �÷��̾ ����
        if (photonView.IsMine)
        {
            HandleMouseLook();
            HandleMovement();
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
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // ī�޶� �������� �̵� ���
        Vector3 direction = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
        direction.y = 0f; // Y�� �̵� ����
        direction.Normalize();

        // �̵� ���� ����
        Vector3 mov = direction * speed;

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
