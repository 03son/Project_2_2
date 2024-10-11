using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlighFunction : MonoBehaviour, IItemFunction
{
    public Light flashlight; // 손전등 역할을 하는 Light 컴포넌트
    public KeyCode toggleKey = KeyCode.F;
    private bool hasFlashlight = false; // 손전등을 획득했는지 여부

    void Start()
    {
        // 자식에서 Light 컴포넌트를 찾아 할당 (Inspector에서 할당되지 않은 경우에만)
        if (flashlight == null)
        {
            flashlight = GetComponentInChildren<Light>();
        }

        // Light 컴포넌트가 있다면 기본적으로 꺼진 상태로 설정
        if (flashlight != null)
        {
            flashlight.enabled = false;
        }
    }

    void Update()
    {
        // 손전등을 획득한 후에만 동작하도록 함
        if (hasFlashlight && Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight(); // F 키를 눌렀을 때 손전등 켜기/끄기
        }
    }

    // IItemFunction 인터페이스의 Function() 메서드 구현
    public void Function()
    {
        ToggleFlashlight(); // 손전등 켜기/끄기 동작
    }

    // 손전등 획득 시 호출되는 메서드
    public void AcquireFlashlight()
    {
        hasFlashlight = true; // 손전등을 획득한 상태로 설정

        // Light 컴포넌트를 획득한 후에도 다시 확인 및 할당
        if (flashlight == null)
        {
            flashlight = GetComponentInChildren<Light>();
        }

        if (flashlight != null)
        {
            flashlight.enabled = false; // 획득 후 꺼진 상태로 시작

            // 손전등의 부모를 메인 카메라로 설정하여 위치와 회전을 따라가게 함
            flashlight.transform.SetParent(Camera.main.transform);
            flashlight.transform.localPosition = new Vector3(0.3f, -0.3f, 0.5f); // 오른손에 들린 것처럼 위치 조정
            flashlight.transform.localRotation = Quaternion.Euler(0, 0, 0); // 필요한 회전 설정
        }
    }

    // 손전등을 켜고 끄는 메서드
    private void ToggleFlashlight()
    {
        if (flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled; // 불 켜고 끄기
        }
    }
}
