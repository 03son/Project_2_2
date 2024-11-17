using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    private Vector3 investigatePoint;
    public NavMeshAgent agent;                     // NavMeshAgent 컴포넌트
    public Transform patrolParent;                 // 순찰 지점들의 부모 오브젝트
    public Transform[] patrolPoints;               // 순찰 지점 배열
    public LayerMask playerLayer;                  // 플레이어가 속한 레이어
    public float moveSpeed = 3.5f;                 // 순찰 시 이동 속도
    public float chaseSpeed = 10.0f;                // 추적 시 이동 속도
    public float waitTimeBeforePatrol = 2.0f;      // 순찰 시작 전 대기 시간
    public float idleTimeBeforePatrol = 5.0f;      // 예외 처리 - 정지 후 순찰로 돌아가는 시간

    private List<Transform> detectedPlayers;       // 감지된 플레이어 리스트
    private int currentPatrolIndex;                // 현재 순찰 지점 인덱스
    private Vector3 lastKnownPosition;             // 플레이어 마지막 위치
    private bool isWaiting = true;                 // 대기 상태인지 여부
    private float waitTimer = 0f;                  // 대기 타이머
    private float idleTimer = 0f;                  // 정지 상태 타이머

    public float viewDistance = 10f;               // 시야 거리
    public float fieldOfView = 120f;               // 시야각
    public float hearingRange = 50f;               // 청각 범위
    public float minDecibelToDetect = 30f;        // 감지 가능한 최소 데시벨 값
    private Mic micScript;                        // Mic 스크립트 참조

    private enum State { Idle, Patrol, Chase, Search, Investigate };  // 상태 정의 (Idle 추가)
    private State currentState;                    // 현재 상태

    private void Start()
    {
        // NavMeshAgent 초기화
        agent = GetComponent<NavMeshAgent>();

        // 순찰 지점 배열을 자식 오브젝트에서 자동으로 가져오기
        if (patrolParent != null)
        {
            patrolPoints = new Transform[patrolParent.childCount];
            for (int i = 0; i < patrolParent.childCount; i++)
            {
                patrolPoints[i] = patrolParent.GetChild(i);
            }
        }
        else
        {
            Debug.LogError("PatrolParent가 설정되지 않았습니다.");
            enabled = false;
            return;
        }

        // 이동 속도 설정
        if (agent != null)
        {
            agent.speed = moveSpeed;
        }
        else
        {
            Debug.LogError("NavMeshAgent가 " + gameObject.name + "에 없습니다.");
            enabled = false;
            return;
        }

        // 초기 상태 설정
        currentState = State.Idle;
        currentPatrolIndex = 0;
        detectedPlayers = new List<Transform>();
        waitTimer = waitTimeBeforePatrol;
    }

    private void Update()
    {
        // 추후 제작할 예외 처리 (몬스터가 하나의 플레이어만 따라가지 않도록 바꾸기)
        // 예외 처리 (몬스터가 움직임이 없으면 순찰 상태로 변경
        if (agent.velocity.magnitude < 0.1f && !agent.pathPending)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTimeBeforePatrol)
            {
                currentState = State.Patrol;
                GoToNextPatrolPoint();
                idleTimer = 0f; // 타이머 초기화
                return;
            }
        }
        else
        {
            idleTimer = 0f; // 몬스터가 움직이면 idleTimer 초기화
        }
        //Debug.Log(currentState);
        // 현재 상태에 따라 적절한 행동 수행
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Search:
                Search();
                break;
            case State.Investigate:
                Investigate();
                break;
        }

        // 플레이어 감지 상태 업데이트
        UpdateDetectedPlayers();
    }

    private void Idle()
    {
        // 대기 타이머를 갱신
        if (waitTimer < 0)
        {
            waitTimer = waitTimeBeforePatrol;
        }
        // 대기 타이머 업데이트
        waitTimer -= Time.deltaTime;

        // 대기 중 플레이어를 감지하면 추적 상태로 전환
        if (detectedPlayers.Count > 0)
        {
            currentState = State.Chase;
            return;
        }

        // 대기 시간이 끝나면 순찰 시작
        if (waitTimer <= 0f)
        {
            currentState = State.Patrol;
            GoToNextPatrolPoint();
        }
    }

    private void Patrol() // 순찰모드
    {
        if (agent.speed == chaseSpeed)
        {
            agent.speed = moveSpeed;
        }
        // 순찰 상태: 다음 순찰 지점으로 이동
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Idle(); // 순찰이 끝나면 잠시 대기모드로 전환
        }

        // 플레이어를 감지하면 추적 상태로 전환
        if (detectedPlayers.Count > 0)
        {
            currentState = State.Chase;
        }
    }

    private void Chase() // 추적 모드
    {
        if (agent.speed == moveSpeed)
        {
            agent.speed = chaseSpeed;
        }
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

    private void Search() // 수색 모드
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
    private void Investigate() // 조사 상태 추가
    {
        agent.SetDestination(investigatePoint);

        // 조사 지점에 도달하면 순찰 상태로 전환
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

    public void SetInvestigatePoint(Vector3 point)
    {
        investigatePoint = point;
        currentState = State.Investigate;
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
            if (CanSeePlayer(playerTransform) || CanHearSoundSource(playerTransform) || CanHearVoiceSource(playerTransform))
            {
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
    private bool CanHearSoundSource(Transform player)
    {
        // 모든 SoundSource를 가져오고, 각 SoundSource의 데시벨을 계산하여 몬스터가 감지할 수 있는 범위 내에 있는지 확인
        SoundSource[] soundSources = FindObjectsOfType<SoundSource>();
        foreach (SoundSource soundSource in soundSources)
        {
            float decibel = soundSource.GetDecibelAtDistance(transform.position);
            // 데시벨이 감지 가능한 최소 데시벨 값 이상이고, 소리의 범위 내에 있어야 감지
            if (decibel >= minDecibelToDetect && Vector3.Distance(transform.position, player.position) <= soundSource.range)
            {
                return true; // 소리가 감지됨
            }
        }
        return false; // 감지되지 않음
    }
    private bool CanHearVoiceSource(Transform monster)
    {
        // 모든 플레이어 객체를 가져오기
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        monster = this.gameObject.transform;

        // 각 플레이어에 대해 확인
        foreach (GameObject playerObject in playerObjects)
        {
            // 플레이어의 Mic 컴포넌트 찾기
            micScript = playerObject.GetComponentInChildren<Mic>();

            // Mic가 없으면 감지할 수 없음
            if (micScript == null)
            {
                continue;
            }

            // Mic에서 실시간으로 계산된 데시벨 값 가져오기
            float decibel = micScript.GetDecibelAtDistance(transform.position);
            Debug.Log("몬스터가 듣는 데시벨" + decibel);

            // 데시벨이 일정 범위 이상이고, 청각 범위 내에 있으면 소리 감지
            if (decibel >= minDecibelToDetect && Vector3.Distance(transform.position, playerObject.transform.position) <= hearingRange)
            {
                Debug.Log("목소리 청취");
                decibel = 0f;
                return true;  // 소리가 감지됨
            }
        }
        // 어느 플레이어의 소리도 감지되지 않으면 false 반환
        return false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // 부채꼴을 그리기 위한 세그먼트 개수
        int segments = 20;
        float angleStep = fieldOfView / segments; // 각도 단위
        Vector3 origin = transform.position; // 몬스터의 위치

        // 시야각 부채꼴 그리기
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -fieldOfView / 2 + angleStep * i; // 각도 계산
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward; // 각도를 회전하여 방향 벡터 계산
            Vector3 endPoint = origin + direction * viewDistance; // 시야 거리만큼 떨어진 지점 계산

            Gizmos.DrawLine(origin, endPoint); // 몬스터의 위치에서 해당 방향으로 선을 그림
        }

        // 시야 범위 끝에 부채꼴을 완성하는 경계 그리기
        for (int i = 0; i < segments; i++)
        {
            float currentAngle = -fieldOfView / 2 + angleStep * i;
            float nextAngle = currentAngle + angleStep;

            Vector3 currentDir = Quaternion.Euler(0, currentAngle, 0) * transform.forward;
            Vector3 nextDir = Quaternion.Euler(0, nextAngle, 0) * transform.forward;

            Vector3 currentPoint = origin + currentDir * viewDistance;
            Vector3 nextPoint = origin + nextDir * viewDistance;

            Gizmos.DrawLine(currentPoint, nextPoint); // 부채꼴의 각도를 따라 끝 부분을 연결
        }
        // 청각 범위 (원형) 그리기
        Gizmos.color = Color.blue; // 청각 범위는 파란색으로 표시
        Gizmos.DrawWireSphere(origin, hearingRange);
    }

    

}