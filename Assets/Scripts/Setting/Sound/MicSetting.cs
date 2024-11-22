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
    public TMP_Dropdown inputVoice; //입력 음성

    public TMP_Dropdown microphoneMode; //마이크 모드

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
        if(Mic.Instance != null) Mic.Instance.isRecording = false; //싱글 마이크 부분

        if (Mic.Instance != null)
        {
            //멀티에 사용할 마이크 설정(마이크마다 설정이 안 될 수도 있음)
            Mic.Instance.recorder.MicrophoneType = Recorder.MicType.Unity;
            Mic.Instance.recorder.MicrophoneDevice = new Photon.Voice.DeviceInfo(0, Global_Microphone.UseMic);
            Mic.Instance.recorder.RestartRecording();
        }

        Global_Microphone.UseMic = options[MicIndex];

        PlayerPrefs.SetInt("UseMicNumber", MicIndex);
        PlayerPrefs.SetString("UseMicName", options[MicIndex]);
    }
    
}
