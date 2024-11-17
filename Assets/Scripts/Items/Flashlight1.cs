using System.Collections;
using UnityEngine;

public class Flashlight1 : MonoBehaviour
{
    [SerializeField] private GameObject flashlightLight; // Spot Light 오브젝트
    private bool flashlightActive = false;
    private bool isAcquired = false;
    private Transform cameraTransform;
    private Light flashlightComponent;

    [SerializeField] private float intensity = 15f; // 기본 인텐시티 값
    [SerializeField] private float range = 10f; // 기본 렌지 값

    void Start()
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
        }
        cameraTransform = Camera.main.transform;
    }

    public void AcquireFlashlight()
    {
        isAcquired = true;
        Debug.Log("손전등 획득 완료");
    }

    void Update()
    {
        if (isAcquired)
        {
            Debug.Log("손전등이 획득된 상태입니다.");
        }

        Transform equipCameraTransform = cameraTransform.parent.Find("EquipCamera");
        Transform equipItemTransform = equipCameraTransform != null ? equipCameraTransform.Find("EquipItem") : null;

        if (equipItemTransform != null)
        {
            Debug.Log("카메라 부모의 EquipCamera/EquipItem: " + equipItemTransform.name);
        }
        else
        {
            //Debug.LogWarning("EquipCamera/EquipItem 경로를 찾을 수 없습니다.");
        }

        if (transform.parent != null && transform.parent.name == "EquipItem")
        {
            Debug.Log("손전등이 올바른 계층 구조에 있습니다.");
        }
        else
        {
            //Debug.Log("손전등이 올바른 계층 구조에 있지 않습니다.");
        }

        //Debug.Log($"isAcquired 상태: {isAcquired}");

        if (isAcquired && transform.parent != null && transform.parent.name == "EquipItem")
        {
            Debug.Log("손전등 Update 로직 실행 중");

            // `Main Camera`의 위치와 회전을 플래시라이트에 동기화
            flashlightLight.transform.position = cameraTransform.position;
            flashlightLight.transform.rotation = cameraTransform.rotation;

            // 마우스 좌클릭으로 손전등 켜기/끄기
            if (Input.GetMouseButtonDown(0))
            {
                flashlightActive = !flashlightActive;
                flashlightLight.SetActive(flashlightActive);
                Debug.Log("손전등 " + (flashlightActive ? "켜짐" : "꺼짐"));
            }
        }
    }


}
