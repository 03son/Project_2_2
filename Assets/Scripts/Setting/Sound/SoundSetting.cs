using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using TMPro;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using System;

public class SoundSetting : MonoBehaviour
{
    public TMP_Dropdown outputVoice; //���� ���

    List<string> options = new List<string>();

    private void OnEnable()
    {
        int index = 0;
       // var enumerator = new MMDeviceEnumerator();
        //string speakerName = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).FriendlyName;
        //Debug.Log(speakerName);
        
        for (int i = 0; i < WaveOut.DeviceCount; i++)
        {
             options.Add(WaveOut.GetCapabilities(i).ProductName);
            /* 
            if (speakerName == WaveOut.GetCapabilities(i).ProductName)
             {
                 Debug.Log($"���� ��� ���� ����Ŀ �̸� : {speakerName}");
                index = i;
             }
            */
        }
        //string defaultSpeaker = new MMDeviceEnumerator().GetDevice(WaveOut.GetCapabilities(1).ProductName).FriendlyName;
        //Debug.Log(defaultSpeaker);

        outputVoice.ClearOptions();
        outputVoice.value = index;
        outputVoice.AddOptions(options);
        outputVoice.onValueChanged.AddListener(SetSpeaker);
    }

    void SetSpeaker(int speakerIndex)
    {
        var outputDevice = new WaveOutEvent() { DeviceNumber = speakerIndex };
        Debug.Log(outputDevice.ToString());

        Debug.Log(options[speakerIndex]);
    }

    #region ��ü ����
    void SetMasterVolume()
    { 
    
    }
    #endregion
    #region ��� ����
    void SetBGMVolume()
    {

    }
    #endregion
    #region ȿ���� ����
    void SetSFXVolume()
    {

    }
    #endregion
}
