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
    List<string> microphoneMode_options = new List<string>();

    private void OnEnable()
    {
        if (Microphone.devices.Length > 0)//연결된 마이크가 있을 때
        {
            string[] selectedMic = Microphone.devices;

            foreach (string mic in selectedMic) //마이크
            {
                options.Add(mic);
            }
        }
        else
        {
            options.Add("마이크 없음");
        }

        microphoneMode_options.Add("항상 말하기");
        microphoneMode_options.Add("눌러서 말하기");
        
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
        if(Mic.Instance != null) Mic.Instance.isRecording = false; //싱글 마이크 부분

        if (Global_Microphone.UseMic == null)
            return;

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

    void SetMicMode(int Index)
    {
        microphoneMode.value = Index;
        Global_Microphone.MicMode = Index == 0 ? true : false; // T = 항상 말하기 , F = 눌러서 말하기
        PlayerPrefs.SetInt("microphoneMode", Index);
    }
    
}
