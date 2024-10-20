
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerJump : MonoBehaviour
{
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // �߷� ó�� �� ����
        if (controller.isGrounded)
        {
            velocity.y = -2f; // �ٴڿ� ���� �� �ణ�� �߷¸� ����

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // ĳ���� �̵� ó��
        controller.Move(velocity * Time.deltaTime);
    }
}

