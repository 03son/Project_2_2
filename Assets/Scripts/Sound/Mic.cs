using Photon.Voice.Unity;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Mic : MonoBehaviour
{
    public Recorder recorder; // Photon Voice의 Recorder
    private float currentDb = 0f; // 현재 데시벨 값
    private PhotonView photonView; // 로컬 플레이어 확인용

    [SerializeField]GameObject Microphone_Decibel_Bar; //인게임 마이크 데시벨 UI

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!photonView.IsMine) // 로컬 플레이어가 아니면 스크립트 비활성화
        {
            enabled = false;
            return;
        }

        if (recorder == null)
        {
            Debug.LogError("Recorder is not assigned!");
            enabled = false;
            return;
        }

        //마이크 데시벨 UI 할당
        Microphone_Decibel_Bar = GameObject.Find("Microphone_Decibel_Bar");
    }

    void Update()
    {
        //마이크 off일 때 UI 막대 값 = 0
        if (!recorder.TransmitEnabled)
            Microphone_Decibel_Bar.GetComponent<Slider>().value = 0;

        if (recorder != null && recorder.IsCurrentlyTransmitting)
        {
            // 데시벨 계산
            float rms = recorder.LevelMeter.CurrentAvgAmp;
            currentDb = 20 * Mathf.Log10(rms + 1e-6f) + 80; // +1e-6f로 로그 방지

            Debug.Log("Current Decibel Level: " + currentDb);

            SetMicDecibel_UI();
        }
    }
    // 데시벨을 반환하는 함수 (필요할 경우 사용)
    public float GetDecibelAtDistance(Vector3 listenerPosition)
    {
        // 마이크와의 거리 계산
        float distance = Vector3.Distance(listenerPosition, transform.position);
        if (recorder.TransmitEnabled == true)
        {
            // 현재 데시벨 값을 반환하고, 거리 기반으로 감쇠 적용
            return currentDb - 10 * Mathf.Log10(distance + 1e-6f);  // 거리 기반으로 데시벨 값 조정 (단위: dB)
        }
        return 0f; // 마이크가 켜져있지 않으면 0을 출력
    }

    //마이크 데시벨을 슬라이더 UI에 전송
    public void SetMicDecibel_UI()
    {
        if(recorder.TransmitEnabled) 
        {
            //currentDb의 값을 UI의 Value 범위에 맞게 조정 후 적용
            Microphone_Decibel_Bar.GetComponent<Slider>().value = Mathf.InverseLerp(30, 60, (int)currentDb);
            Debug.Log("ddjfhjshfdksj" + Mathf.InverseLerp(30, 60, (int)currentDb));
        }
    }
}
