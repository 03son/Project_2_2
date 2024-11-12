using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mic : MonoBehaviour
{
    public AudioSource mic; // 마이크로 녹음된 소리를 재생할 오디오소스 컴포넌트
    private AudioClip micClip;
    private int sampleRate = 44100;  // 샘플링 레이트
    private int bufferSize = 1024;   // 한 번에 처리할 오디오 데이터 크기
    private bool isRecording = false; // 녹음 여부 체크
    private float currentDb = 0f; // 실시간으로 계산된 데시벨 값 저장

    void Start()
    {
        string[] myMic = Microphone.devices;
        for (int i = 0; i < myMic.Length; i++)
        {
            Debug.Log(Microphone.devices[i].ToString());
        }
    }

    void Update()
    {
        if (!isRecording)
        {
            // start에 넣으면 오류가 생기는데 이유를 도저히 모르겠음
            // 첫 번째 마이크 장치를 선택하고, 음성 녹음 시작
            micClip = Microphone.Start(Microphone.devices[0], true, 10, sampleRate);
            isRecording = true;

            // 오디오 소스에 클립을 지정
            mic.clip = micClip;
            mic.loop = true;
            mic.Play();
        }

        if (isRecording)
        {
            // 음성을 실시간으로 처리 (버퍼를 가져와서 재생)
            int micPosition = Microphone.GetPosition(Microphone.devices[0]);
            if (micPosition >= bufferSize)
            {
                // 마이크 데이터를 가져와서 버퍼에 추가
                float[] micData = new float[bufferSize];
                micClip.GetData(micData, micPosition - bufferSize);

                // RMS 값을 계산
                float rms = CalculateRMS(micData);
                currentDb = 20 * Mathf.Log10(rms) + 80;  // 데시벨 값을 0부터 시작하도록 보정

                // 데시벨 값 디버그 출력
                Debug.Log("Current Decibel Level: " + currentDb);
            }
        }
    }

    // RMS 계산 함수
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

    // 데시벨을 반환하는 함수 (필요할 경우 사용)
    public float GetDecibelAtDistance(Vector3 listenerPosition)
    {
        // 마이크와의 거리 계산
        float distance = Vector3.Distance(listenerPosition, transform.position);

        // 현재 데시벨 값을 반환하고, 거리 기반으로 감쇠 적용
        return currentDb - 20 * Mathf.Log10(distance);  // 거리 기반으로 데시벨 값 조정 (단위: dB)
    }
}
