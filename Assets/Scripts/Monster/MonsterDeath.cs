using UnityEngine;

public class MonsterDeath : MonoBehaviour
{
    private void Start()
    {
        // 필요한 초기화 코드가 있다면 여기에 작성
    }

    private void OnTriggerEnter(Collider other)
    {
        // 몬스터의 콜라이더가 플레이어와 충돌할 때
        if (other.CompareTag("Player"))
        {
            // PlayerDeathManager 스크립트를 가져와 Die 메서드 호출
            PlayerDeathManager playerHealth = other.GetComponent<PlayerDeathManager>();

            if (playerHealth != null)
            {
                playerHealth.Die(); // 플레이어의 Die 메서드 호출
                Debug.Log("플레이어가 몬스터에게 잡혀 죽었습니다.");
            }
            else
            {
                Debug.LogWarning("PlayerDeathManager 컴포넌트를 플레이어에서 찾을 수 없습니다.");
            }
        }
    }
}
