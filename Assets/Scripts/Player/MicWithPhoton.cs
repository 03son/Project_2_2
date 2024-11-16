using Photon.Voice.Unity;
using Photon.Pun;
using UnityEngine;

public class MicWithPhoton : MonoBehaviour
{
    public Recorder recorder; // Photon Voice의 Recorder
    public float noiseThreshold = 50f; // 몬스터가 반응할 데시벨 기준값
    private float currentDb = 0f; // 현재 데시벨 값
    private PhotonView photonView; // 로컬 플레이어 확인용

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
    }

    void Update()
    {
        if (recorder != null && recorder.IsCurrentlyTransmitting)
        {
            // 데시벨 계산
            float rms = recorder.LevelMeter.CurrentAvgAmp;
            currentDb = 20 * Mathf.Log10(rms + 1e-6f) + 80; // +1e-6f로 로그 방지

            Debug.Log("Current Decibel Level: " + currentDb);

            // 모든 몬스터에 대해 소리 감지 확인
            CheckMonstersHearing();
        }
    }

    private void CheckMonstersHearing()
    {
        // Enemy 레이어에 있는 모든 몬스터 찾기
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        GameObject[] enemies = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject enemy in enemies)
        {
            if (enemy.layer == enemyLayer)
            {
                BossController boss = enemy.GetComponent<BossController>();
                if (boss != null && IsWithinHearingRange(boss.transform.position, transform.position, boss.hearingRange))
                {
                    boss.ReactToSound(transform.position); // 몬스터가 플레이어 위치에 반응
                    Debug.Log($"Monster {enemy.name} alerted by player sound!");
                }
            }
        }
    }

    private bool IsWithinHearingRange(Vector3 enemyPosition, Vector3 playerPosition, float hearingRange)
    {
        // 거리와 데시벨 기준으로 몬스터의 청각 범위 확인
        float distance = Vector3.Distance(enemyPosition, playerPosition);
        if (currentDb >= noiseThreshold && distance <= hearingRange)
        {
            Debug.Log($"Player sound detected by enemy! Distance: {distance}, Decibel: {currentDb}");
            return true;
        }
        return false;
    }
}
