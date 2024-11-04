using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingSetValue : MonoBehaviour
{
    //����� ������ ������ �ε�

    void Awake()
    {
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
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    Debug.Log("��üȭ�� ���");
                    break;
                case 1: // �׵θ� ���� â ���
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
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
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
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
