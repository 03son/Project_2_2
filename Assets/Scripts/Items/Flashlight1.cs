using System.Collections;
using UnityEngine;

public class Flashlight1 : MonoBehaviour
{
    [SerializeField] private GameObject flashlightLight; // Spot Light 오브젝트
    private bool flashlightActive = false;
    private bool isAcquired = false;
    private Transform cameraTransform;
    private Light flashlightComponent;

    [SerializeField] private float intensity = 1f; // 기본 인텐시티 값
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
        if (isAcquired && transform.parent == cameraTransform.parent.Find("EquipCamera/EquipItem"))
        {
            // `Main Camera`의 위치와 회전을 동기화
            flashlightLight.transform.position = cameraTransform.position;
            flashlightLight.transform.rotation = cameraTransform.rotation;

            // 손전등 켜기/끄기
            if (Input.GetMouseButtonDown(0))
                Debug.Log("작동");
            {
                flashlightActive = !flashlightActive;
                flashlightLight.SetActive(flashlightActive);
                Debug.Log("손전등 " + (flashlightActive ? "켜짐" : "꺼짐"));
            }

            // 인텐시티 값 조절 (키보드로 '+'와 '-' 키 사용)
            if (Input.GetKeyDown(KeyCode.Equals)) // '+' 키
            {
                intensity += 0.5f;
                flashlightComponent.intensity = intensity;
                Debug.Log("손전등 인텐시티 증가: " + intensity);
            }
            else if (Input.GetKeyDown(KeyCode.Minus)) // '-' 키
            {
                intensity = Mathf.Max(0, intensity - 0.5f);
                flashlightComponent.intensity = intensity;
                Debug.Log("손전등 인텐시티 감소: " + intensity);
            }

            // 렌지 값 조절 (키보드로 '['와 ']' 키 사용)
            if (Input.GetKeyDown(KeyCode.RightBracket)) // ']' 키
            {
                range += 1f;
                flashlightComponent.range = range;
                Debug.Log("손전등 렌지 증가: " + range);
            }
            else if (Input.GetKeyDown(KeyCode.LeftBracket)) // '[' 키
            {
                range = Mathf.Max(1, range - 1f);
                flashlightComponent.range = range;
                Debug.Log("손전등 렌지 감소: " + range);
            }
        }
    }
}
