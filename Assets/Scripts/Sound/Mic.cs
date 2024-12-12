using Photon.Voice.Unity;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;
using Recorder = Photon.Voice.Unity.Recorder;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Photon.Voice.PUN;

public class Mic : MonoBehaviour
{
    public static Mic Instance;

    [Header("��Ƽ ��")]
    public Recorder recorder; // Photon Voice�� Recorder
    private float currentDb = 0f; // ���� ���ú� ��
    private PhotonView photonView; // ���� �÷��̾� Ȯ�ο�

    [Header("�̱� ��")]
    public AudioSource mic; 
    private AudioClip micClip;
    private int sampleRate = 44100; 
    private int bufferSize = 1024; 
    public bool isRecording = false; 
    public bool singleMic = false;
    public PlayerState playerState;
    public bool MicToggle = false;


    [SerializeField]GameObject Microphone_Decibel_Bar; //�ΰ��� ����ũ ���ú� UI

    [SerializeField] bool single;

    void Awake()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView = GetComponent<PhotonView>();
            recorder.enabled = true;
            GetComponent<Speaker>().enabled = true;
            GetComponent<PhotonVoiceView>().enabled = true;
        }
        else
        {
            recorder.enabled=false;
            GetComponent<Speaker>().enabled = false;
            GetComponent<PhotonVoiceView>().enabled = false;
        }

        Instance = this;

        single = PhotonNetwork.IsConnected ? false : true;

        if (single)
            mic.volume = 0;
    }

    void Start()
    {
        playerState = GetComponentInParent<PlayerState>();
        if (PhotonNetwork.IsConnected)
        {
            if (!photonView.IsMine) // ���� �÷��̾ �ƴϸ� ��ũ��Ʈ ��Ȱ��ȭ
            {
                enabled = false;
                return;
            }
        }
        //����ũ ���ú� UI �Ҵ�
        Microphone_Decibel_Bar = GameObject.Find("Microphone_Decibel_Bar");
        Microphone_Decibel_Bar.GetComponent<Slider>().value = 0;

        if (PhotonNetwork.IsConnected)
        {
            if (recorder == null)
            {
                Debug.LogError("Recorder is not assigned!");
                enabled = false;
                return;
            }

            if (Global_Microphone.UseMic != null)
            {
                //��Ƽ�� ����� ����ũ ����
                recorder.MicrophoneType = Recorder.MicType.Unity;
                recorder.MicrophoneDevice = new Photon.Voice.DeviceInfo(0, Global_Microphone.UseMic);
                recorder.RestartRecording();
            }
            else
            {
                this.gameObject.SetActive(false);
            }
            recorder.enabled = false;
            recorder.enabled = true;
        }
        isRecording = false;
        StartCoroutine(MicStart());
    }
    IEnumerator MicStart()
    {
        yield return new WaitForSecondsRealtime(1);
        isRecording = false;
    }
    void Update()
    {
        //����ũ off�� �� UI ���� �� = 0
        // if (!recorder.TransmitEnabled || !singleMic)
        // Microphone_Decibel_Bar.GetComponent<Slider>().value = 0;
        if (Global_Microphone.UseMic == null)
            return;

        if (MicToggle)
        {
            if (single) //�̱��� ��
            {
                Single_DecibelLevel();
            }
            else //��Ƽ�� ��
            {
                Multi_DecibelLevel();
            }
        }
    }
    #region �̱��� �� ����ũ

    void Single_DecibelLevel()
    {
        if (Global_Microphone.UseMic == null)
            return;

        if (!isRecording)
        {

            micClip = Microphone.Start(Global_Microphone.UseMic, true, 10, sampleRate);
            isRecording = true;

            mic.clip = micClip;
            mic.loop = true;
            mic.Play();
        }
        if (isRecording)
        {
            //int micPosition = Microphone.GetPosition(Microphone.devices[0]);
            int micPosition = Microphone.GetPosition(Global_Microphone.UseMic);
            if (micPosition >= bufferSize)
            {
                float[] micData = new float[bufferSize];
                micClip.GetData(micData, micPosition - bufferSize);

                float rms = CalculateRMS(micData);
                currentDb = 20 * Mathf.Log10(rms) + 80;

                Debug.Log("Current Decibel Level: " + currentDb);
                SetMicDecibel_UI();
            }
        }
    }

    private float CalculateRMS(float[] samples)
    {
        float sum = 0.0f;
        foreach (float sample in samples)
        {
            sum += sample * sample;
        }
        float rms = Mathf.Sqrt(sum / samples.Length);
        return rms;
    }
    #endregion

    #region 멀티 마이크

    void Multi_DecibelLevel() //��Ƽ �� �� ���ú� ���
    {
        if (recorder != null && recorder.IsCurrentlyTransmitting)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                recorder.TransmitEnabled = true;
            }
            // ���ú� ���
            float rms = recorder.LevelMeter.CurrentAvgAmp;
            currentDb = 20 * Mathf.Log10(rms + 1e-6f) + 80; // +1e-6f�� �α� ����

            //Debug.Log(rms);
            //Debug.Log("Current Decibel Level: " + currentDb);
            // 사망 상태인 플레이어는 소리 전송하지 않음
            if (playerState != null && playerState.State == PlayerState.playerState.Die)
            {
                return;
            }
            if (!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("SendDecibelToMaster", RpcTarget.MasterClient, currentDb, transform.position);
            }

            SetMicDecibel_UI();
        }
    }

    // ���ú��� ��ȯ�ϴ� �Լ� (�ʿ��� ��� ���)
    public float GetDecibelAtDistance(Vector3 listenerPosition)
    {
        // ����ũ���� �Ÿ� ���
        float distance = Vector3.Distance(listenerPosition, transform.position);
        if (recorder.TransmitEnabled == true || isRecording == true)
        {
            // ���� ���ú� ���� ��ȯ�ϰ�, �Ÿ� ������� ���� ����
            return currentDb - 10 * Mathf.Log10(distance + 1e-6f);  // �Ÿ� ������� ���ú� �� ���� (����: dB)
        }
        return 0f; // ����ũ�� �������� ������ 0�� ���
    }

    [PunRPC]
    public void SendDecibelToMaster(float decibel, Vector3 playerPosition)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (MonsterAI.Instance != null)
            {
                if (playerState != null && playerState.State == PlayerState.playerState.Die)
                    return;
                MonsterAI.Instance.HandlePlayerSound(decibel, playerPosition);
            }
            else
            {
                Debug.Log("몬스터 없음");
            }
        }
    }
    #endregion

    //����ũ ���ú��� �����̴� UI�� ����
    public void SetMicDecibel_UI()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (recorder.TransmitEnabled)
            {

                Microphone_Decibel_Bar.GetComponent<Decibel_Bar>().Decibel_Value(Mathf.InverseLerp(30, 60, (int)currentDb), true);
                //currentDb�� ���� UI�� Value ������ �°� ���� �� ����

                //Microphone_Decibel_Bar.GetComponent<Slider>().value = Mathf.InverseLerp(30, 60, (int)currentDb);

                //  Debug.Log("UI ���ú� ��" + Mathf.InverseLerp(30, 60, (int)currentDb));
            }
        }
        else
        {
            Microphone_Decibel_Bar.GetComponent<Decibel_Bar>().Decibel_Value(Mathf.InverseLerp(30, 60, (int)currentDb), true);
        }
    }
}
