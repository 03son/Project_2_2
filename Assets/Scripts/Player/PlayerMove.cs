using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float mouseSpeed = 8f;
    private float gravity = -9.81f;
    private CharacterController controller;
    private Vector3 mov;
    private Vector3 velocity;

    [SerializeField] Transform cameraTransform; // ī�޶��� Transform�� ����

    private float mouseX;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mov = Vector3.zero;
        velocity = Vector3.zero;
        Cursor.lockState = CursorLockMode.Locked;  // ���콺 Ŀ�� ����
    }

    void Update()
    {
        // ���콺 ȸ�� ó��
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        this.transform.localRotation = Quaternion.Euler(0, mouseX, 0);

        // ī�޶��� ������ �������� �̵� ó��
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // ī�޶��� forward�� right ������ �������� �̵� ���� ���
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // ī�޶��� Y���� �����ϰ� ��鿡���� �̵��� ���
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // ���� �̵� ���� ���
        mov = (forward * moveZ + right * moveX).normalized * speed;

        // �߷� ó��
        if (controller.isGrounded)
        {
            velocity.y = -2f; // �ٴڿ� ���� �� �ణ�� �߷¸� ����
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // ���߿� ���� �� �߷� ����
        }

        // ĳ���� �̵� ó��
        controller.Move((mov + velocity) * Time.deltaTime);
    }


}
