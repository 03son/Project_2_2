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
        if (!Global_Microphone.MicMode) //������ ���ϱ� ���
        {
            // Ű�� ������ �ִ� ���ȸ� ������ ����
            if (Input.GetKey(KeyManager.Mic_Key))
            {
                recorder.TransmitEnabled = true; // ����ũ Ȱ��ȭ
                GetComponent<Mic>().singleMic = true;
            }
            else
            {
                recorder.TransmitEnabled = false; // ����ũ ��Ȱ��ȭ
                GetComponent<Mic>().singleMic = false;
            }
            return;
        }
        else if(Global_Microphone.MicMode)//�׻� ���ϱ� ���
        {
            recorder.TransmitEnabled = true; // ����ũ Ȱ��ȭ
            GetComponent<Mic>().singleMic = true;
            return;
        }
    }
}
