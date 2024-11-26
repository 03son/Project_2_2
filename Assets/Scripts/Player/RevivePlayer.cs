using UnityEngine;

public class RevivePlayer : MonoBehaviour
{
    public float holdTime = 5f; // 입력을 유지해야 하는 시간
    private float holdCounter = 0f;
    private bool isHolding = false;

    private PlayerDeathManager targetPlayer; // 타겟 플레이어의 PlayerDeathManager
    public MedkitTimerUI medkitTimerUI; // 타이머 UI 스크립트

    void Start()
    {
        // MedkitTimerUI가 Inspector에서 연결되지 않았을 경우 자동으로 찾기
        if (medkitTimerUI == null)
        {
            medkitTimerUI = FindObjectOfType<MedkitTimerUI>();
            if (medkitTimerUI != null)
            {
                Debug.Log("MedkitTimerUI 연결 성공: " + medkitTimerUI.name);
            }
            else
            {
                Debug.LogError("MedkitTimerUI를 찾을 수 없습니다. 씬에 MedkitTimerUI가 있는지 확인하세요.");
            }
        }
    }

    void Update()
    {
        if (targetPlayer != null && Input.GetKey(KeyManager.Interaction_Key)) // 상호작용
        {
            isHolding = true;
            holdCounter += Time.deltaTime;

            if (medkitTimerUI != null && holdCounter > 0)
            {
                medkitTimerUI.StartMedkitTimer(); // 타이머 UI 시작
            }

            if (holdCounter >= holdTime) // 지정된 시간이 지나면 부활
            {
                //targetPlayer.Revive(); // 부활 호출
                Debug.Log("플레이어가 부활했습니다.");
                ResetHold();
            }
        }
        else if (Input.GetKeyUp(KeyManager.Interaction_Key) || !isHolding) // 키를 떼거나 상호작용 중단
        {
            if (medkitTimerUI != null)
            {
                medkitTimerUI.ResetTimer(); // 타이머 초기화
            }
            ResetHold();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerDeathManager>().isDead)
        {
            targetPlayer = other.GetComponent<PlayerDeathManager>();
            Debug.Log("죽은 플레이어 발견. E 키를 눌러 부활 가능.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetPlayer = null;
            ResetHold();
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        holdCounter = 0f;
    }
}
