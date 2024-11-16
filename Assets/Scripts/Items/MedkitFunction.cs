using UnityEngine;

public class MedkitFunction : MonoBehaviour, IItemFunction
{
    public float holdTime = 5f; // 부활할 때 필요한 시간
    private float holdCounter = 0f;
    private bool isHolding = false;
    private PlayerDeathManager targetPlayer;

    void Update()
    {
        if (isHolding && targetPlayer != null)
        {
            holdCounter += Time.deltaTime;

            if (holdCounter >= holdTime)
            {
                targetPlayer.Revive(); // 플레이어 부활
                Debug.Log("플레이어가 부활했습니다.");
                Destroy(gameObject); // 사용 후 구급상자 제거
                ResetHold();
            }
        }
    }

    public void Function() // IItemFunction 인터페이스의 메서드 구현
    {
        // 이 메서드는 아이템 사용 시 호출됩니다.
        // 예를 들어, 플레이어가 이 아이템을 사용하면 해당 기능이 동작하도록 구현할 수 있습니다.
        Debug.Log("Medkit 사용이 시작되었습니다.");
    }

    public void StartRevive(PlayerDeathManager player)
    {
        targetPlayer = player;
        isHolding = true;
    }

    public void StopRevive()
    {
        ResetHold();
    }

    private void ResetHold()
    {
        isHolding = false;
        holdCounter = 0f;
        targetPlayer = null;
    }
}
