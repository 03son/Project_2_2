using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScreenMode : MonoBehaviour
{
    public TMP_Dropdown ScreenMode_Dropdown;
    // string[] screenModeText = new string[3] { "전체화면","테두리 없는 창모드","창모드"};

    void OnEnable()
    {
        PlayerPrefsValue();
    }
    private void Start()
    {
        ScreenMode_Dropdown.onValueChanged.AddListener(OnScreenModeChange);
    }
    public void OnScreenModeChange(int option)
    {
        switch (option)
        {
            case 0: // 전체화면 모드
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Debug.Log("전체화면 모드");
                break;
            case 1: // 테두리 없는 창 모드
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Debug.Log("테두리 없는 창 모드");
                break;
            case 2: // 창 모드
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Debug.Log("창모드");
                break;
        }


        //설정한 값을 플레이어 프리퍼런스에 저장 
        PlayerPrefs.SetInt("ScreenMode", option);
    }
    void PlayerPrefsValue()
    {
        //저장된 값이 있으면 가져오기
        if (PlayerPrefs.HasKey("ScreenMode"))
        {
            ScreenMode_Dropdown.value = PlayerPrefs.GetInt("ScreenMode");
        }
        else //저장된 값이 없으면 초기값으로 설정
        {
            ScreenMode_Dropdown.value = 0;
        }
    }
}
