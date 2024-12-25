using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine.Profiling;
using Recorder = Photon.Voice.Unity.Recorder;

public class MicSetting : MonoBehaviour
{
    public TMP_Dropdown inputVoice; //�Է� ����

    public TMP_Dropdown microphoneMode; //����ũ ���

    List<string> options = new List<string>();
    List<string> microphoneMode_options = new List<string>();

    private void OnEnable()
    {
        if (Microphone.devices.Length > 0)//����� ����ũ�� ���� ��
        {
            string[] selectedMic = Microphone.devices;

            foreach (string mic in selectedMic) //����ũ
            {
                options.Add(mic);
            }
        }
        else
        {
            options.Add("����ũ ����");
        }

        microphoneMode_options.Add("�׻� ���ϱ�");
        microphoneMode_options.Add("������ ���ϱ�");
        
        microphoneMode.ClearOptions();
        microphoneMode.AddOptions(microphoneMode_options);

        inputVoice.ClearOptions();
        inputVoice.AddOptions(options);

        inputVoice.value = PlayerPrefs.HasKey("UseMicNumber") ? PlayerPrefs.GetInt("UseMicNumber") : 0;
        microphoneMode.value = PlayerPrefs.HasKey("microphoneMode") ? PlayerPrefs.GetInt("microphoneMode") : 0;

        inputVoice.onValueChanged.AddListener(SetMic);
        microphoneMode.onValueChanged.AddListener(SetMicMode);
    }

    void SetMic(int MicIndex)
    {
        if(Mic.Instance != null) Mic.Instance.isRecording = false; //�̱� ����ũ �κ�

        if (Global_Microphone.UseMic == null)
            return;

        if (Mic.Instance != null) 
        {
            //��Ƽ�� ����� ����ũ ����(����ũ���� ������ �� �� ���� ����)
            Mic.Instance.recorder.MicrophoneType = Recorder.MicType.Unity;
            Mic.Instance.recorder.MicrophoneDevice = new Photon.Voice.DeviceInfo(0, Global_Microphone.UseMic);
            Mic.Instance.recorder.RestartRecording();
        }

        Global_Microphone.UseMic = options[MicIndex];

        PlayerPrefs.SetInt("UseMicNumber", MicIndex);
        PlayerPrefs.SetString("UseMicName", options[MicIndex]);
    }

    void SetMicMode(int Index)
    {
        microphoneMode.value = Index;
        Global_Microphone.MicMode = Index == 0 ? true : false; // T = �׻� ���ϱ� , F = ������ ���ϱ�
        PlayerPrefs.SetInt("microphoneMode", Index);
    }
    
}
