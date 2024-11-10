using UnityEngine;

public class RevivePlayer : MonoBehaviour
{
    public float holdTime = 5f; // 입력을 유지해야 하는 시간
    private float holdCounter = 0f;
    private bool isHolding = false;

    private PlayerDeathManager targetPlayer; // 타겟 플레이어의 PlayerDeathManager
    public MedkitTimerUI medkitTimerUI; // 타이머 UI 스크립트

    void Update()
    {
        if (targetPlayer != null && Input.GetKey(KeyCode.E)) // E 키로 상호작용
        {
            isHolding = true;
            holdCounter += Time.deltaTime;

            if (medkitTimerUI != null && holdCounter > 0)
            {
                medkitTimerUI.StartMedkitTimer();
            }

            if (holdCounter >= holdTime)
            {
                targetPlayer.Revive(); // 부활 호출
                Debug.Log("플레이어가 부활했습니다.");
                ResetHold();
            }
        }
        else if (Input.GetKeyUp(KeyCode.E) || !isHolding)
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
