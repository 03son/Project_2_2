using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Flashlight1 : MonoBehaviourPunCallbacks, IPunObservable
{
    private Light playerFlashlight; // Player 오브젝트에 있는 Light 컴포넌트
    [SerializeField] private GameObject flashlightLight; // Spot Light 오브젝트
    private bool flashlightActive = false;
    private bool isAcquired = false;
    private Transform cameraTransform;
    private Light flashlightComponent;
    private Animator animator;

    private Vector3 targetPosition; // 손전등이 따라가야 할 목표 위치
    private Quaternion targetRotation; // 손전등이 따라가야 할 목표 회전
    




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

    private PhotonView photonView;



    private void Awake()
    {
        // 자식에서 Light 컴포넌트 검색
        flashlightComponent = flashlightLight.GetComponentInChildren<Light>();
        if (flashlightComponent == null)
        {
            Debug.LogError("Light 컴포넌트를 찾을 수 없습니다!");
        }

       
    }


    private void Start()
    {
        if (flashlightLight == null)
        {
            Debug.LogWarning("Spot Light가 할당되지 않았습니다.");
        }
        else
        {
          flashlightComponent = flashlightLight.GetComponentInChildren<Light>();
            flashlightComponent.intensity = intensity;
            flashlightComponent.range = range;
            flashlightLight.SetActive(false); // 시작 시 꺼진 상태
            currentSpotAngle = maxSpotAngle; // 초기 각도 설정
            currentIntensity = maxIntensity; // 초기 강도 설정
        }
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();


        photonView = GetComponent<PhotonView>();
        if (photonView == null)
        {
            Debug.LogError("부모 오브젝트에 PhotonView가 없습니다!");
            return;
        }

        if (playerFlashlight == null)
        {
            // 런타임 중에 정확한 경로로 PlayerFlashlight 오브젝트 찾기
            Transform flashlightTransform = transform.root.Find("캐릭터모델링/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/Head/PlayerFlashlight");
            if (flashlightTransform != null)
            {
                playerFlashlight = flashlightTransform.GetComponent<Light>();
                if (playerFlashlight == null)
                {
                    Debug.LogError("PlayerFlashlight 오브젝트에 Light 컴포넌트가 없습니다!");
                }
            }
            else
            {
             //   Debug.LogError("PlayerFlashlight를 찾을 수 없습니다! 경로를 확인하세요.");
            }
        }

        // 초기 설정
        flashlightLight.SetActive(false);
        if (playerFlashlight != null)
        {
            playerFlashlight.enabled = false; // PlayerFlashlight 끄기
        }

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
        /*
        // 손전등을 장착하고 있지 않으면 종료
        if (transform.parent.name != "handitemattach")
        {
            if (flashlightActive)
            {
                ToggleFlashlight(); // 손전등 상태를 끄도록 호출
            }
            return;
        }
        
        if (transform.parent)
        {
            // 마우스 좌클릭으로 손전등 켜기/끄기
            if (Input.GetMouseButtonDown(0) && transform.parent.name == "handitemattach")
            {
                ToggleFlashlight();
               
            }
        } */
        
        // 손전등이 활성화된 상태에서 빛 조절
        if (flashlightActive)
        {
            AdjustFlashlight();
        }
        
    }

    private void LateUpdate()
    {

        // 디버깅 조건 확인
        if (!isAcquired)
        {
          //  Debug.Log("손전등이 아직 획득되지 않았습니다.");
            return;
        }

     

        // 손전등이 획득되고, 활성화 상태이며, 올바른 부모를 가질 때만 실행
        if (isAcquired && transform.parent != null && transform.parent.name == "handitemattach" && flashlightActive)
        {
            // 목표 위치와 회전값 업데이트
            targetPosition = cameraTransform.TransformPoint(Vector3.zero);
            targetRotation = cameraTransform.rotation;

            

           
           


          
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

        if (playerFlashlight != null)
        {
            playerFlashlight.GetComponent<PlayerFlashlight>().SetFlashlightState(flashlightActive);
        }

        // RPC로 손전등 상태 동기화
        //photonView.RPC("SyncFlashlightState", RpcTarget.All, flashlightActive);
    }

   



    [PunRPC]
    private void SyncFlashlightState(bool isActive)
    {
        flashlightActive = isActive;
        flashlightLight.SetActive(isActive);
        if (playerFlashlight != null)
        {
            playerFlashlight.enabled = isActive;
        }

        Debug.Log("손전등 상태 동기화: " + (isActive ? "켜짐" : "꺼짐"));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 로컬 상태를 네트워크에 전송
            stream.SendNext(flashlightLight.GetComponentInChildren<Light>().intensity);
            stream.SendNext(flashlightLight.GetComponentInChildren<Light>().spotAngle);
        }
        else
        {
            // 네트워크 상태를 로컬에 반영
            flashlightLight.GetComponentInChildren<Light>().intensity = (float)stream.ReceiveNext();
            flashlightLight.GetComponentInChildren<Light>().spotAngle = (float)stream.ReceiveNext();
        }
    }
}

