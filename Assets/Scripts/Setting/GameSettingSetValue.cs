using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingSetValue : MonoBehaviour
{
    //저장된 게임의 세팅을 로드

    void Awake()
    {
        InputKeySetting();
        VideosSetting();
        SoundSetting();
    }
    #region 게임 설정 불러오기
    void InputKeySetting()
    {

    } //조작키
    void VideosSetting()
    {
        Resolution();
        ScreenMode();
        Brightness();
    }//비디오 설정
    void SoundSetting()
    {
        SoundSeting();
        Voice();
    }//사운드 설정
    #endregion

    #region 비디오 설정 불러오기
    void Resolution() //해상도
    { 
    
    }
    void ScreenMode() //화면 모드
    {
        //저장된 값이 있으면 가져오기
        if (PlayerPrefs.HasKey("ScreenMode"))
        {
            int option = PlayerPrefs.GetInt("ScreenMode");
            switch (option)
            {
                case 0: // 전체화면 모드
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    Debug.Log("전체화면 모드");
                    break;
                case 1: // 테두리 없는 창 모드
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                    Debug.Log("테두리 없는 창 모드");
                    break;
                case 2: // 창 모드
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    Debug.Log("창모드");
                    break;
            }
        }
        else //저장된 값이 없으면 초기값으로 설정
        {
            //전체화면
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }
    void Brightness()//밝기
    { 
    
    }
    #endregion
    #region 조작키 키값 설정 불러오기

    #endregion
    #region 오디오 설정 불러오기
    void SoundSeting()//마스터볼륨, 배경볼륨, 효과음볼륨
    { 
    
    }
    void Voice()//음성채팅 관련, 마이크 토글 옵션
    { 
    
    }
    #endregion
}
