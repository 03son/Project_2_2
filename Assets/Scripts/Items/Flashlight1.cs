using System.Collections;
using UnityEngine;

public class Flashlight1 : MonoBehaviour
{
    [SerializeField] private GameObject flashlightLight; // Spot Light 오브젝트
    private bool flashlightActive = false;
    private bool isAcquired = false;
    private Transform cameraTransform;
    private Light flashlightComponent;
    private Animator animator;

    private Vector3 targetPosition; // 손전등이 따라가야 할 목표 위치
    private Quaternion targetRotation; // 손전등이 따라가야 할 목표 회전
    [SerializeField] private float followSpeed = 5; // 따라가는 속도 (딜레이 정도)




    [SerializeField] private float intensity = 15f; // 기본 인텐시티 값
    [SerializeField] private float range = 10f; // 기본 렌지 값
    [SerializeField] private float minSpotAngle = 30f; // 빛이 가장 좁을 때의 각도
    [SerializeField] private float maxSpotAngle = 80f; // 빛이 가장 넓을 때의 각도
    [SerializeField] private float minIntensity = 1f; // 가까울 때의 빛 강도
    [SerializeField] private float maxIntensity = 3f; // 멀 때의 빛 강도
    [SerializeField] private float maxDistance = 10f; // 최대 거리

    private float currentSpotAngle; // 현재 빛의 각도
    private float currentIntensity; // 현재 빛의 강도
    private float smoothTime = 5f; // 부드럽게 변화하는 시간

    private float updateInterval = 0.7f; // 목표 갱신 주기
    private float updateTimer = 0f; // 타이머

    private void Start()
    {
        if (flashlightLight == null)
        {
            Debug.LogWarning("Spot Light가 할당되지 않았습니다.");
        }
        else
        {
            flashlightComponent = flashlightLight.GetComponent<Light>();
            flashlightComponent.intensity = intensity;
            flashlightComponent.range = range;
            flashlightLight.SetActive(false); // 시작 시 꺼진 상태
            currentSpotAngle = maxSpotAngle; // 초기 각도 설정
            currentIntensity = maxIntensity; // 초기 강도 설정
        }
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    public void AcquireFlashlight()
    {
        isAcquired = true;
        Debug.Log("손전등 획득 완료");
    }

    public bool IsFlashlightActive()
    {
        return flashlightActive;
    }

    private void Update()
    {
        // 손전등을 장착하고 있지 않으면 종료
        if (transform.parent != null && transform.parent.name != "handitemattach")
        {
            if (flashlightActive)
            {
                ToggleFlashlight(); // 손전등 상태를 끄도록 호출
            }
            return;
        }

        // 마우스 좌클릭으로 손전등 켜기/끄기
        if (Input.GetMouseButtonDown(0))
        {
            ToggleFlashlight();
        }

        // 손전등이 활성화된 상태에서 빛 조절
        if (flashlightActive)
        {
            AdjustFlashlight();
        }
    }

    private void LateUpdate()
    {
        // 손전등이 획득되고, 활성화 상태이며, 올바른 부모를 가질 때만 실행
        if (isAcquired && transform.parent != null && transform.parent.name == "handitemattach" && flashlightActive)
        {
            // 타이머를 사용해 일정 주기마다 목표 값 갱신
            updateTimer += Time.deltaTime;
            if (updateTimer >= updateInterval)
            {
                targetPosition = cameraTransform.TransformPoint(Vector3.zero);
                targetRotation = cameraTransform.rotation;
                updateTimer = 0f; // 타이머 초기화
            }

            // 손전등의 위치와 회전을 목표값으로 부드럽게 이동
            flashlightLight.transform.position = Vector3.Lerp(
                flashlightLight.transform.position,
                targetPosition,
                Time.deltaTime * followSpeed
            );

            flashlightLight.transform.rotation = Quaternion.Lerp(
                flashlightLight.transform.rotation,
                targetRotation,
                Time.deltaTime * followSpeed
            );

            Debug.Log("손전등 위치와 회전 동기화 중: " + flashlightLight.transform.position);
            Debug.Log("목표 위치: " + targetPosition + ", 현재 위치: " + flashlightLight.transform.position);

            // 손전등 흔들림 효과 적용
            float shakeAmount = 2f; // 흔들림 정도
            float noiseX = Mathf.PerlinNoise(Time.time, 0.0f) - 0.5f; // -0.5 ~ 0.5
            float noiseY = Mathf.PerlinNoise(0.0f, Time.time) - 0.5f; // -0.5 ~ 0.5
            flashlightLight.transform.rotation *= Quaternion.Euler(noiseX * shakeAmount, noiseY * shakeAmount, 0);
        }
    }


    private void AdjustFlashlight()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            float distance = hit.distance;
            float t = distance / maxDistance;

            float targetSpotAngle = Mathf.Lerp(minSpotAngle, maxSpotAngle, t);
            float targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            flashlightComponent.spotAngle = Mathf.SmoothDamp(flashlightComponent.spotAngle, targetSpotAngle, ref currentSpotAngle, smoothTime);
            flashlightComponent.intensity = Mathf.SmoothDamp(flashlightComponent.intensity, targetIntensity, ref currentIntensity, smoothTime);
        }
        else
        {
            flashlightComponent.spotAngle = Mathf.SmoothDamp(flashlightComponent.spotAngle, maxSpotAngle, ref currentSpotAngle, smoothTime);
            flashlightComponent.intensity = Mathf.SmoothDamp(flashlightComponent.intensity, maxIntensity, ref currentIntensity, smoothTime);
        }
    }

    public void ToggleFlashlight()
    {
        flashlightActive = !flashlightActive;
        flashlightLight.SetActive(flashlightActive);
        Debug.Log("손전등 " + (flashlightActive ? "켜짐" : "꺼짐"));

        if (flashlightLight.activeSelf != flashlightActive)
        {
            Debug.LogWarning("손전등이 제대로 활성화되지 않았습니다.");
        }

        if (animator != null)
        {
            animator.SetBool("isFlashlightOn", flashlightActive);
        }
    }
}

