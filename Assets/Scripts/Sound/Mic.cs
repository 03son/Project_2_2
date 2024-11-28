using Photon.Voice.Unity;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;
using Recorder = Photon.Voice.Unity.Recorder;

public class Mic : MonoBehaviour
{
    public static Mic Instance;

    [Header("멀티 용")]
    public Recorder recorder; // Photon Voice의 Recorder
    private float currentDb = 0f; // 현재 데시벨 값
    private PhotonView photonView; // 로컬 플레이어 확인용

    [Header("싱글 용")]
    public AudioSource mic; 
    private AudioClip micClip;
    private int sampleRate = 44100; 
    private int bufferSize = 1024; 
    public bool isRecording = false; 
    public bool singleMic = false; 


    [SerializeField]GameObject Microphone_Decibel_Bar; //인게임 마이크 데시벨 UI

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
            if (!photonView.IsMine) // 로컬 플레이어가 아니면 스크립트 비활성화
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
            //멀티에 사용할 마이크 설정
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

        //마이크 데시벨 UI 할당
        Microphone_Decibel_Bar = GameObject.Find("Microphone_Decibel_Bar");
    }

    void Update()
    {
        //마이크 off일 때 UI 막대 값 = 0
         if (!recorder.TransmitEnabled || !singleMic)
             Microphone_Decibel_Bar.GetComponent<Slider>().value = 0;


        if (Global_Microphone.UseMic == null)
            return;

        if (single) //싱글일 때
        {
            Single_DecibelLevel();
        }
        else //멀티일 때
        {
            Multi_DecibelLevel();
        }
    }
    #region 싱글일 때 마이크

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

    #region 멀티일 때 마이크

    void Multi_DecibelLevel() //멀티 일 때 데시벨 계산
    {
        if (recorder != null && recorder.IsCurrentlyTransmitting)
        {
            // 데시벨 계산
            float rms = recorder.LevelMeter.CurrentAvgAmp;
            currentDb = 20 * Mathf.Log10(rms + 1e-6f) + 80; // +1e-6f로 로그 방지

            // Debug.Log("Current Decibel Level: " + currentDb);

            if (!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("SendDecibelToMaster", RpcTarget.MasterClient, currentDb, transform.position);
            }

            SetMicDecibel_UI();
        }
    }

    // 데시벨을 반환하는 함수 (필요할 경우 사용)
    public float GetDecibelAtDistance(Vector3 listenerPosition)
    {
        // 마이크와의 거리 계산
        float distance = Vector3.Distance(listenerPosition, transform.position);
        if (recorder.TransmitEnabled == true || isRecording == true)
        {
            // 현재 데시벨 값을 반환하고, 거리 기반으로 감쇠 적용
            return currentDb - 10 * Mathf.Log10(distance + 1e-6f);  // 거리 기반으로 데시벨 값 조정 (단위: dB)
        }
        return 0f; // 마이크가 켜져있지 않으면 0을 출력
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

    //마이크 데시벨을 슬라이더 UI에 전송
    public void SetMicDecibel_UI()
    {
        if(recorder.TransmitEnabled) 
        {
            //currentDb의 값을 UI의 Value 범위에 맞게 조정 후 적용
            Microphone_Decibel_Bar.GetComponent<Slider>().value = Mathf.InverseLerp(30, 60, (int)currentDb);
            //Debug.Log("UI 데시벨 값" + Mathf.InverseLerp(30, 60, (int)currentDb));
        }
    }
}
