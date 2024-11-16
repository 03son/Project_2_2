using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviourPunCallbacks, IPunObservable
{
    public NavMeshAgent agent;                     // NavMeshAgent 컴포넌트
    public Transform patrolParent;                 // 순찰 지점들의 부모 오브젝트
    public Transform[] patrolPoints;               // 순찰 지점 배열
    public LayerMask playerLayer;                  // 플레이어가 속한 레이어
    public float moveSpeed = 3.5f;                 // 순찰 시 이동 속도
    public float chaseSpeed = 10.0f;               // 추적 시 이동 속도
    public float waitTimeBeforePatrol = 2.0f;      // 순찰 시작 전 대기 시간
    public float idleTimeBeforePatrol = 5.0f;      // 정지 후 순찰로 돌아가는 시간
    public float hearingRange = 50f;               // 청각 범위
    public float minDecibelToDetect = 30f;         // 감지 가능한 최소 데시벨 값
    private Mic micScript;                         // Mic 스크립트 참조

    private List<Transform> detectedPlayers;       // 감지된 플레이어 리스트
    private int currentPatrolIndex;                // 현재 순찰 지점 인덱스
    private Vector3 lastKnownPosition;             // 플레이어 마지막 위치
    private Vector3 investigatePoint;              // 조사할 위치
    private float waitTimer = 0f;                  // 대기 타이머
    private float idleTimer = 0f;                  // 정지 상태 타이머

    private enum State { Idle, Patrol, Chase, Search, Investigate }; // 몬스터 상태 정의
    private State currentState;                    // 현재 상태

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 순찰 지점 설정
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

        currentState = State.Idle;
        currentPatrolIndex = 0;
        detectedPlayers = new List<Transform>();
        waitTimer = waitTimeBeforePatrol;
    }

    private void Update()
    {
        if (!photonView.IsMine) // 로컬 클라이언트만 행동 처리
            return;

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

        UpdateDetectedPlayers();
    }

    private void Idle()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0f)
        {
            currentState = State.Patrol;
            GoToNextPatrolPoint();
        }
    }

    private void Patrol()
    {
        if (agent.speed != moveSpeed)
        {
            agent.speed = moveSpeed;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Idle;
            waitTimer = waitTimeBeforePatrol;
        }
    }

    private void Chase()
    {
        if (agent.speed != chaseSpeed)
        {
            agent.speed = chaseSpeed;
        }

        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer != null)
        {
            agent.SetDestination(closestPlayer.position);
            lastKnownPosition = closestPlayer.position;
        }
    }

    private void Search()
    {
        agent.SetDestination(lastKnownPosition);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Patrol;
        }
    }

    private void Investigate()
    {
        agent.SetDestination(investigatePoint);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Patrol;
        }
    }

    public void SetInvestigatePoint(Vector3 point)
    {
        investigatePoint = point;
        currentState = State.Investigate;
    }

    private Transform GetClosestPlayer()
    {
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform player in detectedPlayers)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    private void UpdateDetectedPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        detectedPlayers.Clear();

        foreach (GameObject player in players)
        {
            Transform playerTransform = player.transform;

            if (CanSeePlayer(playerTransform) || CanHearSound(playerTransform))
            {
                detectedPlayers.Add(playerTransform);
            }
        }
    }

    private bool CanSeePlayer(Transform player)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, direction);

        if (angle < 120f / 2)
        {
            if (Vector3.Distance(transform.position, player.position) <= 10f)
            {
                return true;
            }
        }
        return false;
    }

    private bool CanHearSound(Transform player)
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= hearingRange; // 간단한 청각 처리
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // 데이터를 전송
        {
            stream.SendNext(currentState);
        }
        else // 데이터를 수신
        {
            currentState = (State)stream.ReceiveNext();
        }
    }
}
