using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public NavMeshAgent agent;                     // NavMeshAgent 컴포넌트
    public Transform[] patrolPoints;                // 순찰 지점 배열
    public LayerMask playerLayer;                   // 플레이어가 속한 레이어

    private List<Transform> detectedPlayers;       // 감지된 플레이어 리스트
    private int currentPatrolIndex;                 // 현재 순찰 지점 인덱스
    private Vector3 lastKnownPosition;              // 플레이어 마지막 위치
    private Vector3 soundHeardPosition;             // 소리를 들은 위치 저장

    public float viewDistance = 10f;                // 시야 거리
    public float fieldOfView = 120f;                // 시야각
    public float hearingRange = 15f;                // 청각 범위

    private enum State { Patrol, Chase, Search, Investigate };   // 상태 정의에 Investigate 추가
    private State currentState;

    private void Start()
    {
        // NavMeshAgent 초기화
        agent = GetComponent<NavMeshAgent>();

        // 플레이어를 수동으로 할당하지 않았다면 자동으로 찾기
        detectedPlayers = new List<Transform>();

        // NavMeshAgent가 없으면 오류
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent가 " + gameObject.name + "에 없습니다.");
            enabled = false; // 스크립트를 비활성화하여 추가 에러 방지
            return;
        }

        // 초기 상태 설정
        currentState = State.Patrol;
        currentPatrolIndex = 0;
        GoToNextPatrolPoint();

        // 유리병 깨진 소리를 듣고 반응하는 이벤트 등록
        GlassCupFunction.OnGlassBreak += OnGlassBreakHeard;
    }

    private void OnDestroy()
    {
        // 이벤트 등록 해제
        GlassCupFunction.OnGlassBreak -= OnGlassBreakHeard;
    }

    private void Update()
    {
        // 현재 상태에 따라 적절한 행동 수행
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Search:
                Search();
                break;
            case State.Investigate:  // 소리 발생 위치로 이동
                Investigate();
                break;
        }

        // 플레이어 감지 상태 업데이트
        UpdateDetectedPlayers();
    }

    private void Patrol()
    {
        // 순찰 상태: 다음 순찰 지점으로 이동
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }

        // 플레이어를 감지하면 추적 상태로 전환
        if (detectedPlayers.Count > 0)
        {
            currentState = State.Chase;
        }
    }

    private void Chase()
    {
        // 가장 가까운 플레이어를 추적
        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer != null)
        {
            agent.SetDestination(closestPlayer.position);
            lastKnownPosition = closestPlayer.position; // 마지막으로 본 위치 업데이트
        }

        // 시야에서 플레이어를 잃으면 수색 상태로 전환
        if (detectedPlayers.Count == 0)
        {
            currentState = State.Search;
        }
    }

    private void Search()
    {
        // 마지막으로 본 위치로 이동하여 수색
        agent.SetDestination(lastKnownPosition);

        // 수색 위치에 도착하면 순찰 상태로 전환
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Patrol;
        }

        // 다시 플레이어를 발견하면 추적 상태로 전환
        if (detectedPlayers.Count > 0)
        {
            currentState = State.Chase;
        }
    }

    private void Investigate()
    {
        // 유리병이 깨진 위치로 이동
        agent.SetDestination(soundHeardPosition);

        // 해당 위치에 도착하면 순찰 상태로 돌아감
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Patrol;
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // 다음 지점으로 인덱스 이동
    }

    private Transform GetClosestPlayer()
    {
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform player in detectedPlayers)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    private void UpdateDetectedPlayers()
    {
        // 모든 플레이어 오브젝트를 찾아 감지 리스트를 업데이트
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        List<Transform> currentPlayers = new List<Transform>();

        foreach (GameObject playerObject in playerObjects)
        {
            Transform playerTransform = playerObject.transform;

            // 플레이어가 감지 범위 내에 있는지 확인
            if (CanSeePlayer(playerTransform) || CanHearPlayer(playerTransform))
            {
                // 감지된 플레이어를 리스트에 추가
                if (!detectedPlayers.Contains(playerTransform))
                {
                    detectedPlayers.Add(playerTransform);
                }
                currentPlayers.Add(playerTransform);
            }
        }

        // 감지되지 않은 플레이어를 리스트에서 제거
        for (int i = detectedPlayers.Count - 1; i >= 0; i--)
        {
            if (!currentPlayers.Contains(detectedPlayers[i]))
            {
                detectedPlayers.RemoveAt(i);
            }
        }
    }

    private bool CanSeePlayer(Transform player)
    {
        // 플레이어까지의 방향 벡터 계산
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // 몬스터의 정면과 플레이어의 방향 간의 각도를 계산
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        // 시야각 안에 있는지 먼저 확인
        if (angle < fieldOfView / 2)
        {
            // 시야 거리 내에 있는지 확인
            if (Vector3.Distance(transform.position, player.position) <= viewDistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, viewDistance))
                {
                    // 플레이어와의 사이에 장애물이 없는지 확인
                    if (hit.transform == player)
                    {
                        return true;  // 플레이어가 시야 내에 있음
                    }
                }
            }
        }
        return false;  // 시야 내에 없으면 false 반환
    }

    private bool CanHearPlayer(Transform player)
    {
        // 플레이어가 청각 범위 내에 있는지 확인
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어가 소리를 내는 중인지 확인
        Player playerSound = player.GetComponent<Player>();
        return distanceToPlayer <= hearingRange && playerSound != null && playerSound.audioSource.isPlaying;
    }

    private void OnGlassBreakHeard(Vector3 position)
    {
        // 유리병 깨진 소리가 들렸을 때, 소리의 위치로 이동하도록 설정
        float distanceToSound = Vector3.Distance(transform.position, position);

        if (distanceToSound <= hearingRange)
        {
            soundHeardPosition = position;
            currentState = State.Investigate;  // 소리 들린 곳으로 이동
        }
    }
}
