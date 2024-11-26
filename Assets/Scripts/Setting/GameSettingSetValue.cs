using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class GameSettingSetValue : MonoBehaviour
{   //����� ������ ������ �ε�

    public static GameSettingSetValue Instance;

    static bool LoadGameSetting = false;

    List<Resolution> resolutions;

    public AudioMixer m_AudioMixer;

    void Awake()
    {
        //PlayerPrefs.DeleteAll();

        Instance = this;

        //���� ���� ���࿡�� ����� ������ ����
        if (LoadGameSetting)
            return;

        InputKeySetting();
        VideosSetting();
        SoundSetting();
    }
    #region ���� ���� �ҷ�����
    void InputKeySetting()
    {
        LoadKey();
        LoadMouseSensitivity();
    } //����Ű
    void VideosSetting()
    {
        Resolution();
        ScreenMode();
        Brightness();
    }//���� ����
    void SoundSetting()
    {
        SoundSeting();
        Voice();
    }//���� ����
    #endregion

    #region ���� ���� �ҷ�����
    void Resolution() //�ػ�
    {

        // ��� �ػ󵵸� �������� 16:9 ������ �ػ󵵸� ����
        resolutions = new List<Resolution>(Screen.resolutions);

        // �ػ󵵸� ���͸��� 16:9 ���� �� ���� ���� �ֻ����� ������ �ػ󵵸� ����
        resolutions = resolutions
            .Where(x => Mathf.Approximately((float)x.width / x.height, 16f / 9)) // 16:9 ���� ����
            .GroupBy(x => new { x.width, x.height }) // �ػ󵵷� �׷�ȭ
            .Select(g => g.OrderByDescending(x => x.refreshRateRatio.numerator).First()) // �� �׷쿡�� �ִ� �ֻ��� ����
            .ToList();

        int currentResolutionIndex = 0;
        //����� ���� ����Ͱ� ��� �� �� �ִ� �ػ󵵶� ��ġ�ϸ� �Ҵ�
        if (PlayerPrefs.HasKey("resolutionIndex"))
        {
            var _resolution = resolutions[PlayerPrefs.GetInt("resolutionIndex")];
            string _option = $"{_resolution.width} x {_resolution.height}";

            if (_option == PlayerPrefs.GetString("resolutionValue"))
            {
                currentResolutionIndex = PlayerPrefs.GetInt("resolutionIndex");
            }

            // ���õ� �ػ󵵸� ����
            Resolution selectedResolution = resolutions[currentResolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode);
        }
    }
    void ScreenMode() //ȭ�� ���
    {
        //����� ���� ������ ��������
        if (PlayerPrefs.HasKey("ScreenMode"))
        {
            int option = PlayerPrefs.GetInt("ScreenMode");
            switch (option)
            {
                case 0: // ��üȭ�� ���
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    Debug.Log("��üȭ�� ���");
                    break;
                case 1: // �׵θ� ���� â ���
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    Debug.Log("�׵θ� ���� â ���");
                    break;
                case 2: // â ���
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    Debug.Log("â���");
                    break;
            }
        }
        else //����� ���� ������ �ʱⰪ���� ����
        {
            //��üȭ��
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
    }
    void Brightness()//���
    { 
    
    }
    #endregion
    #region ����Ű Ű�� ���� �ҷ�����
    KeyCode StringToKeyCode(string keyString) //string -> KeyCode
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyString);
    }
    public void LoadKey()
    {
       KeyManager.Front_Key = PlayerPrefs.HasKey("FrontKey") ? StringToKeyCode(PlayerPrefs.GetString("FrontKey")) : KeyCode.W; //��
       KeyManager.Back_Key = PlayerPrefs.HasKey("BackKey") ? StringToKeyCode(PlayerPrefs.GetString("BackKey")) : KeyCode.S; //��
       KeyManager.Left_Key = PlayerPrefs.HasKey("LeftKey") ? StringToKeyCode(PlayerPrefs.GetString("LeftKey")) : KeyCode.A; //��
       KeyManager.Right_Key = PlayerPrefs.HasKey("RightKey") ? StringToKeyCode(PlayerPrefs.GetString("RightKey")) : KeyCode.D; //��
       KeyManager.Jump_Key = PlayerPrefs.HasKey("Jump_Key") ? StringToKeyCode(PlayerPrefs.GetString("Jump_Key")) : KeyCode.Space; //����
       KeyManager.Run_Key = PlayerPrefs.HasKey("Run_Key") ? StringToKeyCode(PlayerPrefs.GetString("Run_Key")) : KeyCode.LeftShift; //�޸���
       KeyManager.SitDown_Key = PlayerPrefs.HasKey("SitDown_Key") ? StringToKeyCode(PlayerPrefs.GetString("SitDown_Key")) : KeyCode.LeftControl; //�ɱ�

       KeyManager.Interaction_Key = PlayerPrefs.HasKey("Interaction_Key") ? StringToKeyCode(PlayerPrefs.GetString("Interaction_Key")) : KeyCode.F; //��ȣ�ۿ�
       KeyManager.Mic_Key = PlayerPrefs.HasKey("Mic_Key") ? StringToKeyCode(PlayerPrefs.GetString("Mic_Key")) : KeyCode.T; //����ũ
       KeyManager.Drop_Key = PlayerPrefs.HasKey("Drop_Key") ? StringToKeyCode(PlayerPrefs.GetString("Drop_Key")) : KeyCode.G; //������
    }

    void LoadMouseSensitivity()
    {
        float defaultValue = 0.2f;
        float volume = PlayerPrefs.HasKey("MouseSensitivity") ? PlayerPrefs.GetFloat("MouseSensitivity") : defaultValue;
        GameInfo.MouseSensitivity = (int)(volume * 10);
    }
    #endregion
    #region ����� ���� �ҷ�����
    void SoundSeting()//�����ͺ���, ��溼��, ȿ��������
    {
        float defaultValue = 0.6f;
        float volume;

        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume = PlayerPrefs.HasKey("MasterVolume") ? PlayerPrefs.GetFloat("MasterVolume") : defaultValue) * 20);
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume = PlayerPrefs.HasKey("BGMVolume") ? PlayerPrefs.GetFloat("BGMVolume") : defaultValue) * 20);
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : defaultValue) * 20);
    }
    void Voice()//����ä�� ����, ����ũ ��� �ɼ�
    {
        if (Microphone.devices.Length > 0) //����ũ ������
        {
            Global_Microphone.UseMic = PlayerPrefs.HasKey("UseMicName") ? PlayerPrefs.GetString("UseMicName") : Microphone.devices[0]; //����ũ ����
        }
        else
        {
            Global_Microphone.UseMic = null;
        }

        if (PlayerPrefs.HasKey("microphoneMode"))
        {
            if (PlayerPrefs.GetInt("microphoneMode") == 0)
            {
                Global_Microphone.MicMode = true;//�׻� ���ϱ�
            }
            else
            {
                Global_Microphone.MicMode = false; //������ ���ϱ�
            }
        } //����ũ ���
        else
        {
            Global_Microphone.MicMode = true;
        }
    }
    #endregion
}
