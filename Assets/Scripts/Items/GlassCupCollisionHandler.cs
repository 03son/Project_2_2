using System.Collections;
using UnityEngine;

public class GlassCupCollisionHandler : MonoBehaviour
{
    [Header("Glass Cup Settings")]
    public AudioClip breakSound;  // 유리컵이 깨질 때 나는 소리
    public GameObject brokenGlassPrefab;  // 유리컵이 깨졌을 때 생성될 프리팹 (깨진 유리 조각)

    private bool hasBroken = false;  // 유리컵이 이미 깨졌는지 확인하는 변수

    void OnCollisionEnter(Collision collision)
    {
        // 유리컵이 이미 깨졌다면 더 이상 처리하지 않음
        if (hasBroken)
            return;

        // 일정 속도 이상의 충돌일 경우 깨짐 처리
        if (collision.relativeVelocity.magnitude > 2f)
        {
            BreakGlass(collision.contacts[0].point);
        }
    }

    void BreakGlass(Vector3 breakPoint)
    {
        hasBroken = true;

        // 깨지는 소리 재생
        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, breakPoint);
        }

        // 깨진 유리 프리팹 생성
        if (brokenGlassPrefab != null)
        {
           // Instantiate(brokenGlassPrefab, transform.position, transform.rotation);
        }

        // 몬스터들에게 깨진 위치 알림
        AlertMonsters(breakPoint);

        // 기존 유리컵 오브젝트 비활성화
        gameObject.SetActive(false);
    }

    void AlertMonsters(Vector3 breakPoint)
    {
        // 반경 내의 모든 Collider 찾기
        Collider[] colliders = Physics.OverlapSphere(breakPoint, 50f); // 청각 범위(50미터)

        foreach (var collider in colliders)
        {
            MonsterAI monsterAI = collider.GetComponent<MonsterAI>();  // MonsterAI 컴포넌트를 가진 적 탐색
            if (monsterAI != null)
            {
                float distanceToMonster = Vector3.Distance(breakPoint, monsterAI.transform.position);

                // 몬스터의 청각 범위 내에 있는 경우에만 반응
                if (distanceToMonster <= monsterAI.hearingRange)
                {
                    monsterAI.SetInvestigatePoint(breakPoint);  // 몬스터의 조사 지점을 깨진 곳으로 설정
                }
            }
        }
    }

    // 아이템을 획득했을 때 물리 설정을 변경하는 메서드
    public void SetPhysicsForPickedUp()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;  // 플레이어와 충돌하지 않게 하기 위함
        }
    }
}
