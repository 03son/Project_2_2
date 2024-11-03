using UnityEngine;

public class FlashlightSync : MonoBehaviour
{
    public Transform mainCamera; // 메인 카메라의 Transform을 할당할 변수

    void Start()
    {
        // 메인 카메라를 자동으로 찾음
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        // 카메라의 위치와 회전을 플래시라이트에 동기화
        if (mainCamera != null)
        {
            transform.position = mainCamera.position;
            transform.rotation = mainCamera.rotation;
        }
    }
}
