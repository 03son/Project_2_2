using UnityEngine;
using Photon.Pun;

public class PlayerDeathManager : MonoBehaviourPunCallbacks
{
    public bool isDead = false;
    public GameObject deathEffect; // 죽음 효과 (애니메이션, 파티클 등)
    private Vector3 deathPosition;

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        deathPosition = transform.position;

        // 죽음 효과 표시
        if (deathEffect != null)
            Instantiate(deathEffect, deathPosition, Quaternion.identity);

        // 죽은 플레이어의 움직임과 상호작용 멈추기
        GetComponent<PlayerMove>().enabled = false;
        GetComponent<PlayerDash>().enabled = false;

        // 싱글 플레이용으로 NotifyDeath()를 직접 호출
        NotifyDeath(deathPosition);
    }

    void NotifyDeath(Vector3 position)
    {
        Debug.Log("플레이어가 이 위치에서 죽었습니다: " + position);
    }

    public void Revive()
    {
        if (!isDead) return;

        isDead = false;

        // 플레이어 부활 위치 및 상태 복구
        transform.position = deathPosition;
        GetComponent<PlayerMove>().enabled = true;
        GetComponent<PlayerDash>().enabled = true;
    }
}
