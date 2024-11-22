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

    private void OnEnable()
    {
        string[] selectedMic = Microphone.devices;

        foreach (string mic in selectedMic)
        {
            options.Add(mic);
        }
        inputVoice.ClearOptions();
        inputVoice.AddOptions(options);

        inputVoice.value = PlayerPrefs.HasKey("UseMicNumber") ? PlayerPrefs.GetInt("UseMicNumber") : 0;

        inputVoice.onValueChanged.AddListener(SetMic);
    }

    void SetMic(int MicIndex)
    {
        if(Mic.Instance != null) Mic.Instance.isRecording = false; //�̱� ����ũ �κ�

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
    
}
