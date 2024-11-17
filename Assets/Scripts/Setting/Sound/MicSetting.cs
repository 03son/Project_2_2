using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using TMPro;
using UnityEditor.Rendering;

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
            Debug.Log(mic);
        }
        inputVoice.ClearOptions();
        inputVoice.AddOptions(options);

        inputVoice.onValueChanged.AddListener(SetMic);
    }

    void SetMic(int MicIndex)
    {
        Debug.Log($"������ ����ũ �̸� : {options[MicIndex]}");
    }
    
}
