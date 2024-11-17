using Photon.Voice.Unity;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Mic : MonoBehaviour
{
    public Recorder recorder; // Photon Voice�� Recorder
    private float currentDb = 0f; // ���� ���ú� ��
    private PhotonView photonView; // ���� �÷��̾� Ȯ�ο�

    [SerializeField]GameObject Microphone_Decibel_Bar; //�ΰ��� ����ũ ���ú� UI

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!photonView.IsMine) // ���� �÷��̾ �ƴϸ� ��ũ��Ʈ ��Ȱ��ȭ
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

        //����ũ ���ú� UI �Ҵ�
        Microphone_Decibel_Bar = GameObject.Find("Microphone_Decibel_Bar");
    }

    void Update()
    {
        if (recorder != null && recorder.IsCurrentlyTransmitting)
        {
            // ���ú� ���
            float rms = recorder.LevelMeter.CurrentAvgAmp;
            currentDb = 20 * Mathf.Log10(rms + 1e-6f) + 80; // +1e-6f�� �α� ����

            Debug.Log("Current Decibel Level: " + currentDb);

            SetMicDecibel_UI();
        }
    }
    // ���ú��� ��ȯ�ϴ� �Լ� (�ʿ��� ��� ���)
    public float GetDecibelAtDistance(Vector3 listenerPosition)
    {
        // ����ũ���� �Ÿ� ���
        float distance = Vector3.Distance(listenerPosition, transform.position);

        // ���� ���ú� ���� ��ȯ�ϰ�, �Ÿ� ������� ���� ����
        return currentDb  * Mathf.Log10(distance);  // �Ÿ� ������� ���ú� �� ���� (����: dB)
    }

    //����ũ ���ú��� �����̴� UI�� ����
    public void SetMicDecibel_UI()
    {
        //currentDb�� ���� UI�� Value ������ �°� ���� �� ����
        //float micDecibel = Mathf.InverseLerp(0, 80,currentDb);
        // Microphone_Decibel_Bar.GetComponent<Slider>().value = Mathf.Lerp(0,1, micDecibel);
        Microphone_Decibel_Bar.GetComponent<Slider>().value = Mathf.InverseLerp(0, 80, (int)currentDb);
        Debug.Log("ddjfhjshfdksj"+Mathf.InverseLerp(0, 80, (int)currentDb));
    }
}
