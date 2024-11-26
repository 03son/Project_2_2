using System.Collections;
using UnityEngine;

public class Flashlight1 : MonoBehaviour
{
    [SerializeField] private GameObject flashlightLight; // Spot Light 오브젝트
    private bool flashlightActive = false;
    private bool isAcquired = false;
    private Transform cameraTransform;
    private Light flashlightComponent;
    private Animator animator; // Animator 컴포넌트 추가

    [SerializeField] private float intensity = 15f; // 기본 인텐시티 값
    [SerializeField] private float range = 10f; // 기본 렌지 값
    [SerializeField] private float minSpotAngle = 30f; // 빛이 가장 좁을 때의 각도
    [SerializeField] private float maxSpotAngle = 80f; // 빛이 가장 넓을 때의 각도
    [SerializeField] private float minIntensity = 1f; // 가까울 때의 빛 강도
    [SerializeField] private float maxIntensity = 3f; // 멀 때의 빛 강도
    [SerializeField] private float maxDistance = 10f; // 최대 거리

    private float currentSpotAngle; // 현재 빛의 각도
    private float currentIntensity; // 현재 빛의 강도
    private float smoothTime = 0.1f; // 부드럽게 변화하는 시간

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
            flashlightLight.SetActive(true);; // 시작 시 꺼진 상태
            currentSpotAngle = maxSpotAngle; // 초기 각도 설정
            currentIntensity = maxIntensity; // 초기 강도 설정
        }
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기

        if (animator == null)
        {
            Debug.LogError("Animator 컴포넌트를 찾을 수 없습니다. 손전등 오브젝트에 Animator가 있어야 합니다.");
        }
    }

    public void AcquireFlashlight()
    {
        isAcquired = true;
        Debug.Log("손전등 획득 완료");
    }

    // 손전등 켜기/끄기 상태 전환 메서드 추가
    public void ToggleFlashlight(bool state)
    {
        flashlightActive = state;
        flashlightLight.SetActive(flashlightActive);
        Debug.Log("손전등 " + (flashlightActive ? "켜짐" : "꺼짐"));
    }

    public bool IsFlashlightActive()
    {
        return flashlightActive;
    }

    private void Update()
    {
        if (isAcquired && transform.parent != null && transform.parent.name == "EquipItem")
        {
            // 카메라 위치와 회전을 손전등에 동기화
            flashlightLight.transform.position = cameraTransform.position;
            flashlightLight.transform.rotation = cameraTransform.rotation;

            // 마우스 좌클릭으로 손전등 켜기/끄기
            if (Input.GetMouseButtonDown(0))
            {
                flashlightActive = !flashlightActive;
                flashlightLight.SetActive(flashlightActive);
                Debug.Log("손전등 " + (flashlightActive ? "켜짐" : "꺼짐"));

                // 손전등 켜고 끄기 애니메이션 설정
                if (animator != null)
                {
                    animator.SetBool("isFlashlightOn", flashlightActive);
                }
            }

            if (flashlightActive)
            {
                AdjustFlashlight(); // 손전등의 빛 조절
            }
        }
    }

    private void AdjustFlashlight()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        // 레이캐스트를 통해 벽이나 물체에 부딪혔는지 확인
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            float distance = hit.distance;

            // 거리에 따라 Spot Angle과 Intensity 조절
            float t = distance / maxDistance; // 거리에 대한 비율 계산 (0에서 1 사이)

            float targetSpotAngle = Mathf.Lerp(minSpotAngle, maxSpotAngle, t);
            float targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            // 부드럽게 변화시키기
            flashlightComponent.spotAngle = Mathf.SmoothDamp(flashlightComponent.spotAngle, targetSpotAngle, ref currentSpotAngle, smoothTime);
            flashlightComponent.intensity = Mathf.SmoothDamp(flashlightComponent.intensity, targetIntensity, ref currentIntensity, smoothTime);
        }
        else
        {
            // 벽에 부딪히지 않았을 때 기본 최대 값 사용
            float targetSpotAngle = maxSpotAngle;
            float targetIntensity = maxIntensity;

            flashlightComponent.spotAngle = Mathf.SmoothDamp(flashlightComponent.spotAngle, targetSpotAngle, ref currentSpotAngle, smoothTime);
            flashlightComponent.intensity = Mathf.SmoothDamp(flashlightComponent.intensity, targetIntensity, ref currentIntensity, smoothTime);
        }
    }
}
