using UnityEngine;

public class Flashlight2 : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // 메인 카메라의 Transform
    [SerializeField] private float followSpeed = 20f; // 따라가는 속도 (딜레이 조절)
    private Quaternion currentRotation; // 현재 회전값
    private Quaternion targetRotation; // 목표 회전값

    [SerializeField] private float shakeAmount = 30f; // 흔들림 정도 (강도 증가)
    private CharacterController characterController; // 플레이어 이동 상태 확인용

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
        characterController = GetComponentInParent<CharacterController>(); // 플레이어 이동 상태 확인
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

         if (IsPlayerMoving())
    {
        float noiseX = Mathf.PerlinNoise(Time.time * 5f, 0.0f) - 0.5f;
        float noiseY = Mathf.PerlinNoise(0.0f, Time.time * 5f) - 0.5f;

        float adjustedShakeAmount = shakeAmount;

        if (IsPlayerRunning())
        {
            adjustedShakeAmount *= 1.5f; // 달리기 시 흔들림 강도 증가
        }

        transform.rotation *= Quaternion.Euler(noiseX * adjustedShakeAmount, noiseY * adjustedShakeAmount, 0);
        Debug.Log("흔들림 적용 중!");
    }
    }
    private bool IsPlayerRunning()
    {
        return Input.GetKey(KeyManager.Run_Key) && IsPlayerMoving();
    }
    private bool IsPlayerMoving()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 이동 입력값이 있는 경우 이동 중으로 간주
        return Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;
    }
}
