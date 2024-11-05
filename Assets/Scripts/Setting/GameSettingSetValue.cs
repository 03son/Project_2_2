using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameSettingSetValue : MonoBehaviour
{   //저장된 게임의 세팅을 로드

    static bool LoadGameSetting = false;

    List<Resolution> resolutions;

    void Awake()
    {
        //PlayerPrefs.DeleteAll();

        //게임 최초 실행에만 저장된 설정을 적용
        if (LoadGameSetting)
            return;

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

        // 모든 해상도를 가져오고 16:9 비율의 해상도만 선택
        resolutions = new List<Resolution>(Screen.resolutions);

        // 해상도를 필터링해 16:9 비율 중 가장 높은 주사율을 가지는 해상도만 남김
        resolutions = resolutions
            .Where(x => Mathf.Approximately((float)x.width / x.height, 16f / 9)) // 16:9 비율 필터
            .GroupBy(x => new { x.width, x.height }) // 해상도로 그룹화
            .Select(g => g.OrderByDescending(x => x.refreshRateRatio.numerator).First()) // 각 그룹에서 최대 주사율 선택
            .ToList();

        int currentResolutionIndex = 0;
        //저장된 값이 모니터가 출력 할 수 있는 해상도랑 일치하면 할당
        if (PlayerPrefs.HasKey("resolutionIndex"))
        {
            var _resolution = resolutions[PlayerPrefs.GetInt("resolutionIndex")];
            string _option = $"{_resolution.width} x {_resolution.height}";

            if (_option == PlayerPrefs.GetString("resolutionValue"))
            {
                currentResolutionIndex = PlayerPrefs.GetInt("resolutionIndex");
            }

            // 선택된 해상도를 설정
            Resolution selectedResolution = resolutions[currentResolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode);
        }
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
        }
        else //저장된 값이 없으면 초기값으로 설정
        {
            //전체화면
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
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
