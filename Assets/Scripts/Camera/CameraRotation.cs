using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRot : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 8f; // 회전 속도
    [SerializeField] private Transform playerTransform; // 플레이어의 Transform

    private float mouseX = 0f; // 좌우 회전 값
    private float mouseY = 0f; // 위아래 회전 값
    [SerializeField] private Vector3 offset; // 카메라와 플레이어 사이의 간격

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 고정
    }

    void Update()
    {
        // 마우스 입력을 받아 카메라 회전 처리
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSpeed;

        // 위아래 회전 각도 제한
        mouseY = Mathf.Clamp(mouseY, -50f, 30f);

        // 카메라의 회전 적용 (플레이어의 회전을 따라감)
        this.transform.localEulerAngles = new Vector3(mouseY, mouseX, 0);

        // 카메라 위치를 플레이어 위치에 고정
        this.transform.position = playerTransform.position + offset;
    }
}