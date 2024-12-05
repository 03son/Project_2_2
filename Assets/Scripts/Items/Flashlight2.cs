using UnityEngine;

public class Flashlight2 : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // 메인 카메라의 Transform
    [SerializeField] private float followSpeed = 1.5f; // 따라가는 속도 (딜레이 조절)
    private Quaternion currentRotation; // 현재 회전값
    private Quaternion targetRotation; // 목표 회전값
    private Light flashlightComponent;

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
            if (cameraTransform == null)
            {
                Debug.LogError("Main Camera를 찾을 수 없습니다!");
            }
        }

        currentRotation = transform.rotation; // 초기 회전값
    }

    private void LateUpdate()
    {
        if (cameraTransform == null) return;

        // 목표 회전값 업데이트
        targetRotation = cameraTransform.rotation;

        // SmoothDamp처럼 부드럽게 회전
        currentRotation = Quaternion.Lerp(
            currentRotation,
            targetRotation,
            Time.deltaTime * followSpeed
        );

        // 실제 회전값 적용
        transform.rotation = currentRotation;

        // 디버깅
        Debug.Log("스팟라이트 회전 동기화 중: " + transform.rotation.eulerAngles);


    }
}
