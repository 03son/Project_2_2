using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameSettingSetValue : MonoBehaviour
{   //����� ������ ������ �ε�

    static bool LoadGameSetting = false;

    List<Resolution> resolutions;

    void Awake()
    {
        //PlayerPrefs.DeleteAll();

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

    #endregion
    #region ����� ���� �ҷ�����
    void SoundSeting()//�����ͺ���, ��溼��, ȿ��������
    { 
    
    }
    void Voice()//����ä�� ����, ����ũ ��� �ɼ�
    { 
    
    }
    #endregion
}
