using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScreenMode : MonoBehaviour
{
    public TMP_Dropdown ScreenMode_Dropdown;
    // string[] screenModeText = new string[3] { "��üȭ��","�׵θ� ���� â���","â���"};

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


        //������ ���� �÷��̾� �����۷����� ���� 
        PlayerPrefs.SetInt("ScreenMode", option);
    }
    void PlayerPrefsValue()
    {
        //����� ���� ������ ��������
        if (PlayerPrefs.HasKey("ScreenMode"))
        {
            ScreenMode_Dropdown.value = PlayerPrefs.GetInt("ScreenMode");
        }
        else //����� ���� ������ �ʱⰪ���� ����
        {
            ScreenMode_Dropdown.value = 0;
        }
    }
}
