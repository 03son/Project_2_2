using UnityEngine;
using Photon.Pun;

public class PlayerFlashlight : MonoBehaviourPun
{
    private Light flashlightLight; // PlayerFlashlight의 Light 컴포넌트

    [SerializeField] private Transform headTransform; // Head 오브젝트 참조
    [SerializeField] private float maxDistance = 10f; // 빛의 최대 거리
    [SerializeField] private float minSpotAngle = 30f; // 최소 각도
    [SerializeField] private float maxSpotAngle = 80f; // 최대 각도
    [SerializeField] private float minIntensity = 1f; // 최소 강도
    [SerializeField] private float maxIntensity = 3f; // 최대 강도
    private float smoothTime = 0.1f; // 부드러운 변화 시간

    private float currentSpotAngle; // 현재 스팟 각도
    private float currentIntensity; // 현재 강도

    private void Awake()
    {
        // Light 컴포넌트 초기화
        flashlightLight = GetComponent<Light>();
        if (flashlightLight == null)
        {
            Debug.LogError("PlayerFlashlight에 Light 컴포넌트가 없습니다!");
        }
        else
        {
            flashlightLight.enabled = false; // 기본 꺼진 상태로 시작
            currentSpotAngle = maxSpotAngle; // 초기 각도
            currentIntensity = maxIntensity; // 초기 강도
        }

        if (headTransform == null)
        {
            // Head 오브젝트를 찾아 초기화
            Transform head = transform.root.Find("캐릭터모델링/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/Head");
            if (head != null)
            {
                headTransform = head;
            }
            else
            {
                Debug.LogError("Head 오브젝트를 찾을 수 없습니다!");
            }
        }
    }
    void Start()
    {
        if (photonView.IsMine) // 로컬 플레이어라면
        {
            GetComponent<Light>().enabled = false; // 라이트 끄기
        }
    }


    private void Update()
    {
        if (flashlightLight != null && flashlightLight.enabled)
        {
            AdjustFlashlight();
        }
    }

    private void AdjustFlashlight()
    {
        if (headTransform == null) return;

        Ray ray = new Ray(headTransform.position, headTransform.forward); // Head 방향 기준
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            float distance = hit.distance;
            float t = distance / maxDistance;

            float targetSpotAngle = Mathf.Lerp(minSpotAngle, maxSpotAngle, t);
            float targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            flashlightLight.spotAngle = Mathf.SmoothDamp(flashlightLight.spotAngle, targetSpotAngle, ref currentSpotAngle, smoothTime);
            flashlightLight.intensity = Mathf.SmoothDamp(flashlightLight.intensity, targetIntensity, ref currentIntensity, smoothTime);
        }
        else
        {
            flashlightLight.spotAngle = Mathf.SmoothDamp(flashlightLight.spotAngle, maxSpotAngle, ref currentSpotAngle, smoothTime);
            flashlightLight.intensity = Mathf.SmoothDamp(flashlightLight.intensity, maxIntensity, ref currentIntensity, smoothTime);
        }
    }

    public void SetFlashlightState(bool isActive)
    {
        if (photonView.IsMine)
        {
            // 로컬에서 상태 설정
            SetLocalFlashlightState(isActive);

            // RPC로 다른 클라이언트에 동기화
            photonView.RPC("SyncFlashlightState", RpcTarget.Others, isActive);
        }
    }

    private void SetLocalFlashlightState(bool isActive)
    {
        if (flashlightLight != null)
        {
            if (!photonView.IsMine) // 로컬 플레이어가 아니라면만 상태를 변경
            {
                flashlightLight.enabled = isActive; // 빛 활성화/비활성화
                Debug.Log("PlayerFlashlight 상태 변경: " + (isActive ? "켜짐" : "꺼짐"));
            }
            else
            {
                Debug.Log("로컬 플레이어라서 PlayerFlashlight는 변경되지 않습니다.");
            }
        }
    }


    [PunRPC]
    private void SyncFlashlightState(bool isActive)
    {
        // 다른 클라이언트에서 동기화된 상태 적용
        SetLocalFlashlightState(isActive);
    }
}
