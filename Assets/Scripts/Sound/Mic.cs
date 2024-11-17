using Photon.Voice.Unity;
using Photon.Pun;
using UnityEngine;

public class Mic : MonoBehaviour
{
    public Recorder recorder; // Photon Voice�� Recorder
    private float currentDb = 0f; // ���� ���ú� ��
    private PhotonView photonView; // ���� �÷��̾� Ȯ�ο�


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
    }

    void Update()
    {
        if (recorder != null && recorder.IsCurrentlyTransmitting)
        {
            // ���ú� ���
            float rms = recorder.LevelMeter.CurrentAvgAmp;
            currentDb = 20 * Mathf.Log10(rms + 1e-6f) + 80; // +1e-6f�� �α� ����
            Debug.Log("�÷��̾ ���� ���ú�" + currentDb);
        }
    }
    // ���ú��� ��ȯ�ϴ� �Լ� (�ʿ��� ��� ���)
    public float GetDecibelAtDistance(Vector3 listenerPosition)
    {
        // ����ũ���� �Ÿ� ���
        float distance = Vector3.Distance(listenerPosition, transform.position);
        if (recorder.TransmitEnabled == true)
        {
            // ���� ���ú� ���� ��ȯ�ϰ�, �Ÿ� ������� ���� ����
            return currentDb - 10 * Mathf.Log10(distance + 1e-6f);  // �Ÿ� ������� ���ú� �� ���� (����: dB)
        }
        return 0f; // ����ũ�� �������� ������ 0�� ���
    }
}
