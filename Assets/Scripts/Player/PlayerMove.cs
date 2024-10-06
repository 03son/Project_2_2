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

    [SerializeField] Transform cameraTransform; // 카메라의 Transform을 참조

    private float mouseX;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mov = Vector3.zero;
        velocity = Vector3.zero;
        Cursor.lockState = CursorLockMode.Locked;  // 마우스 커서 고정
    }

    void Update()
    {
        // 마우스 회전 처리
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        this.transform.localRotation = Quaternion.Euler(0, mouseX, 0);

        // 카메라의 방향을 기준으로 이동 처리
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // 카메라의 forward와 right 방향을 기준으로 이동 벡터 계산
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // 카메라의 Y축은 무시하고 평면에서의 이동만 계산
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // 최종 이동 벡터 계산
        mov = (forward * moveZ + right * moveX).normalized * speed;

        // 중력 처리
        if (controller.isGrounded)
        {
            velocity.y = -2f; // 바닥에 있을 때 약간의 중력만 적용
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // 공중에 있을 때 중력 적용
        }

        // 캐릭터 이동 처리
        controller.Move((mov + velocity) * Time.deltaTime);
    }


}
