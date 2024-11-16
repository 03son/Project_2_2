using UnityEngine;

public class RevivePlayer : MonoBehaviour
{
    public float holdTime = 5f;
    private float holdCounter = 0f;
    private bool isHolding = false;

    private PlayerDeathManager targetPlayer;
    public MedkitTimerUI medkitTimerUI;

    void Update()
    {
        if (targetPlayer != null && Input.GetKey(KeyCode.E))
        {
            isHolding = true;
            holdCounter += Time.deltaTime;

            if (medkitTimerUI != null && holdCounter > 0 && !isHolding)
            {
                medkitTimerUI.StartMedkitTimer();
            }

            if (holdCounter >= holdTime)
            {
                targetPlayer.Revive();
                Debug.Log("플레이어가 부활했습니다.");
                ResetHold();
            }
        }
        else if (Input.GetKeyUp(KeyCode.E) || !isHolding)
        {
            if (medkitTimerUI != null)
            {
                medkitTimerUI.ResetTimer();
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
