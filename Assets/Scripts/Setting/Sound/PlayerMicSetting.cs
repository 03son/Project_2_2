using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Pun;

public class PlayerMicSetting : MonoBehaviour
{
    Recorder recorder;
    void Start()
    {
        recorder = GetComponent<Recorder>();
    }
    void Update()
    {
        if (!Global_Microphone.MicMode) //눌러서 말하기 모드
        {
            // 키를 누르고 있는 동안만 음성을 전송
            if (Input.GetKey(KeyManager.Mic_Key))
            {
                recorder.TransmitEnabled = true; // 마이크 활성화
                GetComponent<Mic>().singleMic = true;
            }
            else
            {
                recorder.TransmitEnabled = false; // 마이크 비활성화
                GetComponent<Mic>().singleMic = false;
            }
            return;
        }
        else if(Global_Microphone.MicMode)//항상 말하기 모드
        {
            recorder.TransmitEnabled = true; // 마이크 활성화
            GetComponent<Mic>().singleMic = true;
            return;
        }
    }
}
