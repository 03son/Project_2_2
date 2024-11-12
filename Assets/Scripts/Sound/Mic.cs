using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mic : MonoBehaviour
{
    public AudioSource mic; // ����ũ�� ������ �Ҹ��� ����� ������ҽ� ������Ʈ
    private AudioClip micClip;
    private int sampleRate = 44100;  // ���ø� ����Ʈ
    private int bufferSize = 1024;   // �� ���� ó���� ����� ������ ũ��
    private bool isRecording = false; // ���� ���� üũ
    private float currentDb = 0f; // �ǽð����� ���� ���ú� �� ����

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
            // start�� ������ ������ ����µ� ������ ������ �𸣰���
            // ù ��° ����ũ ��ġ�� �����ϰ�, ���� ���� ����
            micClip = Microphone.Start(Microphone.devices[0], true, 10, sampleRate);
            isRecording = true;

            // ����� �ҽ��� Ŭ���� ����
            mic.clip = micClip;
            mic.loop = true;
            mic.Play();
        }

        if (isRecording)
        {
            // ������ �ǽð����� ó�� (���۸� �����ͼ� ���)
            int micPosition = Microphone.GetPosition(Microphone.devices[0]);
            if (micPosition >= bufferSize)
            {
                // ����ũ �����͸� �����ͼ� ���ۿ� �߰�
                float[] micData = new float[bufferSize];
                micClip.GetData(micData, micPosition - bufferSize);

                // RMS ���� ���
                float rms = CalculateRMS(micData);
                currentDb = 20 * Mathf.Log10(rms) + 80;  // ���ú� ���� 0���� �����ϵ��� ����

                // ���ú� �� ����� ���
                Debug.Log("Current Decibel Level: " + currentDb);
            }
        }
    }

    // RMS ��� �Լ�
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

    // ���ú��� ��ȯ�ϴ� �Լ� (�ʿ��� ��� ���)
    public float GetDecibelAtDistance(Vector3 listenerPosition)
    {
        // ����ũ���� �Ÿ� ���
        float distance = Vector3.Distance(listenerPosition, transform.position);

        // ���� ���ú� ���� ��ȯ�ϰ�, �Ÿ� ������� ���� ����
        return currentDb - 20 * Mathf.Log10(distance);  // �Ÿ� ������� ���ú� �� ���� (����: dB)
    }
}
