susing Photon.Voice.Unity;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;
using Recorder = Photon.Voice.Unity.Recorder;

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


    [SerializeField]GameObject Microphone_Decibel_Bar; //�ΰ��� ����ũ ���ú� UI

    bool single;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        Instance = this;
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!photonView.IsMine) // ���� �÷��̾ �ƴϸ� ��ũ��Ʈ ��Ȱ��ȭ
            {
                enabled = false;
                return;
            }
        }

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


        isRecording = false;

        single = PhotonNetwork.IsConnected ? false : true;

        if (single)
            mic.volume = 0;

        //����ũ ���ú� UI �Ҵ�
        Microphone_Decibel_Bar = GameObject.Find("Microphone_Decibel_Bar");
    }

    void Update()
    {
        //����ũ off�� �� UI ���� �� = 0
         if (!recorder.TransmitEnabled || !singleMic)
             Microphone_Decibel_Bar.GetComponent<Slider>().value = 0;


        if (Global_Microphone.UseMic == null)
            return;

        if (single) //�̱��� ��
        {
            Single_DecibelLevel();
        }
        else //��Ƽ�� ��
        {
            Multi_DecibelLevel();
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

    #region ��Ƽ�� �� ����ũ

    void Multi_DecibelLevel() //��Ƽ �� �� ���ú� ���
    {
        if (recorder != null && recorder.IsCurrentlyTransmitting)
        {
            // ���ú� ���
            float rms = recorder.LevelMeter.CurrentAvgAmp;
            currentDb = 20 * Mathf.Log10(rms + 1e-6f) + 80; // +1e-6f�� �α� ����

            // Debug.Log("Current Decibel Level: " + currentDb);

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
            MonsterAI.Instance.HandlePlayerSound(decibel, playerPosition);
        }
    }
    #endregion

    //����ũ ���ú��� �����̴� UI�� ����
    public void SetMicDecibel_UI()
    {
        if(recorder.TransmitEnabled) 
        {
            //currentDb�� ���� UI�� Value ������ �°� ���� �� ����
            Microphone_Decibel_Bar.GetComponent<Slider>().value = Mathf.InverseLerp(30, 60, (int)currentDb);
            //Debug.Log("UI ���ú� ��" + Mathf.InverseLerp(30, 60, (int)currentDb));
        }
    }
}
