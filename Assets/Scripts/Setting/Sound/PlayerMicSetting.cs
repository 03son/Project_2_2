using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Pun;

public class PlayerMicSetting : MonoBehaviour
{
    Recorder recorder; // Recorder를 연결합니다
    void Start()
    {
        recorder = GetComponent<Recorder>();
    }
    void Update()
    {
        // 키를 누르고 있는 동안만 음성을 전송
        if (Input.GetKey(KeyManager.Mic_Key))
        {
            recorder.TransmitEnabled = true; // 마이크 활성화
        }
        else
        {
            recorder.TransmitEnabled = false; // 마이크 비활성화
            
        }
    }
}
