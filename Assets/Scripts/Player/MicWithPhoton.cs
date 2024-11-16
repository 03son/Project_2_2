using Photon.Voice.Unity;
using Photon.Pun;
using UnityEngine;

public class MicWithPhoton : MonoBehaviour
{
    public Recorder recorder; // Photon Voice�� Recorder
    public float noiseThreshold = 50f; // ���Ͱ� ������ ���ú� ���ذ�
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

            Debug.Log("Current Decibel Level: " + currentDb);

            // ��� ���Ϳ� ���� �Ҹ� ���� Ȯ��
            CheckMonstersHearing();
        }
    }

    private void CheckMonstersHearing()
    {
        // Enemy ���̾ �ִ� ��� ���� ã��
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        GameObject[] enemies = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject enemy in enemies)
        {
            if (enemy.layer == enemyLayer)
            {
                BossController boss = enemy.GetComponent<BossController>();
                if (boss != null && IsWithinHearingRange(boss.transform.position, transform.position, boss.hearingRange))
                {
                    boss.ReactToSound(transform.position); // ���Ͱ� �÷��̾� ��ġ�� ����
                    Debug.Log($"Monster {enemy.name} alerted by player sound!");
                }
            }
        }
    }

    private bool IsWithinHearingRange(Vector3 enemyPosition, Vector3 playerPosition, float hearingRange)
    {
        // �Ÿ��� ���ú� �������� ������ û�� ���� Ȯ��
        float distance = Vector3.Distance(enemyPosition, playerPosition);
        if (currentDb >= noiseThreshold && distance <= hearingRange)
        {
            Debug.Log($"Player sound detected by enemy! Distance: {distance}, Decibel: {currentDb}");
            return true;
        }
        return false;
    }
}
