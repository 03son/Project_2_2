using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight; // 손전등 역할을 하는 Spotlight 컴포넌트
    public KeyCode toggleKey = KeyCode.F; // 손전등을 켜고 끄는 키 (기본: F 키)
    private bool hasFlashlight = false; // 플레이어가 손전등을 획득했는지 여부
    private bool hasLoggedNoFlashlight = false; // 손전등이 없음을 로그로 출력했는지 여부

    void Start()
    {
        if (flashlight == null)
        {
            flashlight = GetComponentInChildren<Light>(); // 자식 오브젝트에서 Light 컴포넌트를 가져옴
        }

        if (flashlight != null)
        {
            flashlight.enabled = false; // 시작할 때 손전등은 꺼진 상태로 설정
            //Debug.Log("손전등 초기화: 꺼진 상태 (" + flashlight.name + ")");
        }
        else
        {
           // Debug.Log("손전등 Light 컴포넌트를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        if (hasFlashlight) // 손전등을 획득한 상태일 때만 작동
        {
            if (!hasLoggedNoFlashlight) // 손전등이 획득되었으면 더 이상 출력하지 않도록
            {
                hasLoggedNoFlashlight = true;
            }

            if (Input.GetKeyDown(toggleKey)) // 지정된 키를 눌렀을 때
            {
                Debug.Log("F 키가 눌렸습니다."); // F 키가 눌렸을 때 로그 출력

                if (flashlight != null)
                {
                    flashlight.enabled = !flashlight.enabled; // 손전등의 활성화 상태를 반전시킴
                    Debug.Log("손전등 상태 변경: " + (flashlight.enabled ? "켜짐" : "꺼짐"));
                }
                else
                {
                    Debug.Log("flashlight 변수가 할당되지 않았습니다.");
                }
            }
        }
        else
        {
            if (!hasLoggedNoFlashlight) // 손전등이 없다는 로그는 한 번만 출력
            {
               // Debug.Log("손전등이 아직 획득되지 않았습니다."); // 손전등을 획득하지 않은 상태임을 알림
                hasLoggedNoFlashlight = true;
            }
        }
    }

    // 손전등을 획득했을 때 호출되는 메서드
    public void AcquireFlashlight()
    {
        hasFlashlight = true; // 손전등 획득 상태로 설정
        Debug.Log("손전등을 획득했습니다.");

        if (flashlight != null)
        {
            flashlight.enabled = false; // 손전등을 획득 후에도 꺼진 상태로 시작
        }
    }
}









