using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class GameSettingSetValue : MonoBehaviour
{   //저장된 게임의 세팅을 로드

    public static GameSettingSetValue Instance;

    static bool LoadGameSetting = false;

    List<Resolution> resolutions;

    public AudioMixer m_AudioMixer;

    void Awake()
    {
        //PlayerPrefs.DeleteAll();

        Instance = this;

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
        LoadKey();
        LoadMouseSensitivity();
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
    KeyCode StringToKeyCode(string keyString) //string -> KeyCode
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyString);
    }
    public void LoadKey()
    {
       KeyManager.Front_Key = PlayerPrefs.HasKey("FrontKey") ? StringToKeyCode(PlayerPrefs.GetString("FrontKey")) : KeyCode.W; //앞
       KeyManager.Back_Key = PlayerPrefs.HasKey("BackKey") ? StringToKeyCode(PlayerPrefs.GetString("BackKey")) : KeyCode.S; //뒤
       KeyManager.Left_Key = PlayerPrefs.HasKey("LeftKey") ? StringToKeyCode(PlayerPrefs.GetString("LeftKey")) : KeyCode.A; //좌
       KeyManager.Right_Key = PlayerPrefs.HasKey("RightKey") ? StringToKeyCode(PlayerPrefs.GetString("RightKey")) : KeyCode.D; //우
       KeyManager.Jump_Key = PlayerPrefs.HasKey("Jump_Key") ? StringToKeyCode(PlayerPrefs.GetString("Jump_Key")) : KeyCode.Space; //점프
       KeyManager.Run_Key = PlayerPrefs.HasKey("Run_Key") ? StringToKeyCode(PlayerPrefs.GetString("Run_Key")) : KeyCode.LeftShift; //달리기
       KeyManager.SitDown_Key = PlayerPrefs.HasKey("SitDown_Key") ? StringToKeyCode(PlayerPrefs.GetString("SitDown_Key")) : KeyCode.LeftControl; //앉기

       KeyManager.Interaction_Key = PlayerPrefs.HasKey("Interaction_Key") ? StringToKeyCode(PlayerPrefs.GetString("Interaction_Key")) : KeyCode.F; //상호작용
       KeyManager.Mic_Key = PlayerPrefs.HasKey("Mic_Key") ? StringToKeyCode(PlayerPrefs.GetString("Mic_Key")) : KeyCode.T; //마이크
       KeyManager.Drop_Key = PlayerPrefs.HasKey("Drop_Key") ? StringToKeyCode(PlayerPrefs.GetString("Drop_Key")) : KeyCode.G; //버리기
    }

    void LoadMouseSensitivity()
    {
        float defaultValue = 0.2f;
        float volume = PlayerPrefs.HasKey("MouseSensitivity") ? PlayerPrefs.GetFloat("MouseSensitivity") : defaultValue;
        GameInfo.MouseSensitivity = (int)(volume * 10);
    }
    #endregion
    #region 오디오 설정 불러오기
    void SoundSeting()//마스터볼륨, 배경볼륨, 효과음볼륨
    {
        float defaultValue = 0.6f;
        float volume;

        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume = PlayerPrefs.HasKey("MasterVolume") ? PlayerPrefs.GetFloat("MasterVolume") : defaultValue) * 20);
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume = PlayerPrefs.HasKey("BGMVolume") ? PlayerPrefs.GetFloat("BGMVolume") : defaultValue) * 20);
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : defaultValue) * 20);
    }
    void Voice()//음성채팅 관련, 마이크 토글 옵션
    {
        if (Microphone.devices.Length > 0) //마이크 있으면
        {
            Global_Microphone.UseMic = PlayerPrefs.HasKey("UseMicName") ? PlayerPrefs.GetString("UseMicName") : Microphone.devices[0]; //마이크 설정
        }
        else
        {
            Global_Microphone.UseMic = null;
        }

        if (PlayerPrefs.HasKey("microphoneMode"))
        {
            if (PlayerPrefs.GetInt("microphoneMode") == 0)
            {
                Global_Microphone.MicMode = true;//항상 말하기
            }
            else
            {
                Global_Microphone.MicMode = false; //눌러서 말하기
            }
        } //마이크 모드
        else
        {
            Global_Microphone.MicMode = true;
        }
    }
    #endregion
}
