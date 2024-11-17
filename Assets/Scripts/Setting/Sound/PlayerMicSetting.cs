using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Pun;

public class PlayerMicSetting : MonoBehaviour
{
    Recorder recorder; // Recorder�� �����մϴ�
    void Start()
    {
        recorder = GetComponent<Recorder>();
    }
    void Update()
    {
        // Ű�� ������ �ִ� ���ȸ� ������ ����
        if (Input.GetKey(KeyManager.Mic_Key))
        {
            recorder.TransmitEnabled = true; // ����ũ Ȱ��ȭ
        }
        else
        {
            recorder.TransmitEnabled = false; // ����ũ ��Ȱ��ȭ
            
        }
    }
}
