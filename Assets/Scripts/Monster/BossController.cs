using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public float hearingRange = 50f; // 몬스터의 청각 범위
    public NavMeshAgent navAgent; // 몬스터 이동을 위한 NavMeshAgent

    private Vector3 lastHeardPosition; // 마지막으로 감지된 소리 위치
    private bool isChasingSound = false; // 소리 추적 중인지 여부

    void Start()
    {
        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
        }
    }

    public void ReactToSound(Vector3 soundPosition)
    {
        // 소리 감지 시 해당 위치로 이동
        lastHeardPosition = soundPosition;
        isChasingSound = true;
        navAgent.SetDestination(lastHeardPosition); // NavMesh를 통해 이동
        Debug.Log($"Boss moving to sound position: {soundPosition}");
    }

    void Update()
    {
        if (isChasingSound)
        {
            // 목표 지점에 도착했는지 확인
            if (Vector3.Distance(transform.position, lastHeardPosition) < 2f)
            {
                isChasingSound = false; // 추적 종료
                Debug.Log("Boss reached the sound position.");
            }
        }
    }
}
