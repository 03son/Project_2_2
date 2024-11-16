using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public NavMeshAgent agent;                     // NavMeshAgent ������Ʈ
    public Transform patrolParent;                 // ���� �������� �θ� ������Ʈ
    public Transform[] patrolPoints;               // ���� ���� �迭
    public LayerMask playerLayer;                  // �÷��̾ ���� ���̾�
    public float moveSpeed = 3.5f;                 // ���� �� �̵� �ӵ�
    public float chaseSpeed = 10.0f;               // ���� �� �̵� �ӵ�
    public float waitTimeBeforePatrol = 2.0f;      // ���� ���� �� ��� �ð�
    public float idleTimeBeforePatrol = 5.0f;      // ���� ó�� - ���� �� ������ ���ư��� �ð�

    private List<Transform> detectedPlayers;       // ������ �÷��̾� ����Ʈ
    private int currentPatrolIndex;                // ���� ���� ���� �ε���
    private Vector3 lastKnownPosition;             // �÷��̾� ������ ��ġ
    private bool isWaiting = true;                 // ��� �������� ����
    private float waitTimer = 0f;                  // ��� Ÿ�̸�
    private float idleTimer = 0f;                  // ���� ���� Ÿ�̸�
    private Vector3 investigatePoint;              // ������ ����

    public float viewDistance = 10f;               // �þ� �Ÿ�
    public float fieldOfView = 120f;               // �þ߰�
    public float hearingRange = 50f;               // û�� ����
    public float minDecibelToDetect = 30f;        // ���� ������ �ּ� ���ú� ��
    private Mic micScript;                        // Mic ��ũ��Ʈ ����

    private enum State { Idle, Patrol, Chase, Search, Investigate };  // ���� ���� (Investigate �߰�)
    private State currentState;                    // ���� ����

    private void Start()
    {
        // NavMeshAgent �ʱ�ȭ
        agent = GetComponent<NavMeshAgent>();

        // ���� ���� �迭�� �ڽ� ������Ʈ���� �ڵ����� ��������
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
            Debug.LogError("PatrolParent�� �������� �ʾҽ��ϴ�.");
            enabled = false;
            return;
        }

        // �̵� �ӵ� ����
        if (agent != null)
        {
            agent.speed = moveSpeed;
        }
        else
        {
            Debug.LogError("NavMeshAgent�� " + gameObject.name + "�� �����ϴ�.");
            enabled = false;
            return;
        }

        // �ʱ� ���� ����
        currentState = State.Idle;
        currentPatrolIndex = 0;
        detectedPlayers = new List<Transform>();
        waitTimer = waitTimeBeforePatrol;
    }

    private void Update()
    {
        // ���� ó�� (���Ͱ� �������� ������ ���� ���·� ����)
        if (agent.velocity.magnitude < 0.1f && !agent.pathPending)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTimeBeforePatrol)
            {
                currentState = State.Patrol;
                GoToNextPatrolPoint();
                idleTimer = 0f; // Ÿ�̸� �ʱ�ȭ
                return;
            }
        }
        else
        {
            idleTimer = 0f; // ���Ͱ� �����̸� idleTimer �ʱ�ȭ
        }

        // ���� ���¿� ���� ������ �ൿ ����
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

        // �÷��̾� ���� ���� ������Ʈ
        UpdateDetectedPlayers();
    }

    private void Idle()
    {
        // ��� Ÿ�̸Ӹ� ����
        if (waitTimer < 0)
        {
            waitTimer = waitTimeBeforePatrol;
        }
        // ��� Ÿ�̸� ������Ʈ
        waitTimer -= Time.deltaTime;

        // ��� �� �÷��̾ �����ϸ� ���� ���·� ��ȯ
        if (detectedPlayers.Count > 0)
        {
            currentState = State.Chase;
            return;
        }

        // ��� �ð��� ������ ���� ����
        if (waitTimer <= 0f)
        {
            currentState = State.Patrol;
            GoToNextPatrolPoint();
        }
    }

    private void Patrol() // �������
    {
        if (agent.speed == chaseSpeed)
        {
            agent.speed = moveSpeed;
        }
        // ���� ����: ���� ���� �������� �̵�
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Idle(); // ������ ������ ��� ������ ��ȯ
        }

        // �÷��̾ �����ϸ� ���� ���·� ��ȯ
        if (detectedPlayers.Count > 0)
        {
            currentState = State.Chase;
        }
    }

    private void Chase() // ���� ���
    {
        if (agent.speed == moveSpeed)
        {
            agent.speed = chaseSpeed;
        }
        // ���� ����� �÷��̾ ����
        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer != null)
        {
            agent.SetDestination(closestPlayer.position);
            lastKnownPosition = closestPlayer.position; // ���������� �� ��ġ ������Ʈ
        }

        // �þ߿��� �÷��̾ ������ ���� ���·� ��ȯ
        if (detectedPlayers.Count == 0)
        {
            currentState = State.Search;
        }
    }

    private void Search() // ���� ���
    {
        // ���������� �� ��ġ�� �̵��Ͽ� ����
        agent.SetDestination(lastKnownPosition);

        // ���� ��ġ�� �����ϸ� ���� ���·� ��ȯ
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Patrol;
        }

        // �ٽ� �÷��̾ �߰��ϸ� ���� ���·� ��ȯ
        if (detectedPlayers.Count > 0)
        {
            currentState = State.Chase;
        }
    }

    private void Investigate() // ���� ���� �߰�
    {
        agent.SetDestination(investigatePoint);

        // ���� ������ �����ϸ� ���� ���·� ��ȯ
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Patrol;
        }

        // �ٽ� �÷��̾ �߰��ϸ� ���� ���·� ��ȯ
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
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // ���� �������� �ε��� �̵�
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
        // ��� �÷��̾� ������Ʈ�� ã�� ���� ����Ʈ�� ������Ʈ
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        List<Transform> currentPlayers = new List<Transform>();

        foreach (GameObject playerObject in playerObjects)
        {
            Transform playerTransform = playerObject.transform;

            // �÷��̾ ���� ���� ���� �ִ��� Ȯ��
            if (CanSeePlayer(playerTransform) || CanHearSoundSource(playerTransform) || CanHearVoiceSource(playerTransform))
            {
                if (!detectedPlayers.Contains(playerTransform))
                {
                    detectedPlayers.Add(playerTransform);
                }
                currentPlayers.Add(playerTransform);
            }
        }

        // �������� ���� �÷��̾ ����Ʈ���� ����
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
        // �÷��̾������ ���� ���� ���
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // ������ ����� �÷��̾��� ���� ���� ������ ���
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        // �þ߰� �ȿ� �ִ��� ���� Ȯ��
        if (angle < fieldOfView / 2)
        {
            // �þ� �Ÿ� ���� �ִ��� Ȯ��
            if (Vector3.Distance(transform.position, player.position) <= viewDistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, viewDistance))
                {
                    // �÷��̾���� ���̿� ��ֹ��� ������ Ȯ��
                    if (hit.transform == player)
                    {
                        return true;  // �÷��̾ �þ� ���� ����
                    }
                }
            }
        }
        return false;  // �þ� ���� ������ false ��ȯ
    }
    private bool CanHearSoundSource(Transform player)
    {
        // ��� SoundSource�� ��������, �� SoundSource�� ���ú��� ����Ͽ� ���Ͱ� ������ �� �ִ� ���� ���� �ִ��� Ȯ��
        SoundSource[] soundSources = FindObjectsOfType<SoundSource>();
        foreach (SoundSource soundSource in soundSources)
        {
            float decibel = soundSource.GetDecibelAtDistance(transform.position);
            // ���ú��� ���� ������ �ּ� ���ú� �� �̻��̰�, �Ҹ��� ���� ���� �־�� ����
            if (decibel >= minDecibelToDetect && Vector3.Distance(transform.position, player.position) <= soundSource.range)
            {
                return true; // �Ҹ��� ������
            }
        }
        return false; // �������� ����
    }
    private bool CanHearVoiceSource(Transform player)
    {
        // ��� �÷��̾� ��ü�� ��������
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        // �� �÷��̾ ���� Ȯ��
        foreach (GameObject playerObject in playerObjects)
        {
            // �÷��̾��� Mic ������Ʈ ã��
            micScript = playerObject.GetComponentInChildren<Mic>();

            // Mic�� ������ ������ �� ����
            if (micScript == null)
            {
                continue;
            }

            // Mic���� �ǽð����� ���� ���ú� �� ��������
            float decibel = micScript.GetDecibelAtDistance(transform.position);
            Debug.Log(decibel);

            // ���ú��� ���� ���� �̻��̰�, û�� ���� ���� ������ �Ҹ� ����
            if (decibel >= minDecibelToDetect && Vector3.Distance(transform.position, playerObject.transform.position) <= hearingRange)
            {
                Debug.Log("Sound detected from player within hearing range");
                return true;  // �Ҹ��� ������
            }
        }

        // ��� �÷��̾��� �Ҹ��� �������� ������ false ��ȯ
        return false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // ��ä���� �׸��� ���� ���׸�Ʈ ����
        int segments = 20;
        float angleStep = fieldOfView / segments; // ���� ����
        Vector3 origin = transform.position; // ������ ��ġ

        // �þ߰� ��ä�� �׸���
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -fieldOfView / 2 + angleStep * i; // ���� ���
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward; // ������ ȸ���Ͽ� ���� ���� ���
            Vector3 endPoint = origin + direction * viewDistance; // �þ� �Ÿ���ŭ ������ ���� ���

            Gizmos.DrawLine(origin, endPoint); // ������ ��ġ���� �ش� �������� ���� �׸�
        }

        // �þ� ���� ���� ��ä���� �ϼ��ϴ� ��� �׸���
        for (int i = 0; i < segments; i++)
        {
            float currentAngle = -fieldOfView / 2 + angleStep * i;
            float nextAngle = currentAngle + angleStep;

            Vector3 currentDir = Quaternion.Euler(0, currentAngle, 0) * transform.forward;
            Vector3 nextDir = Quaternion.Euler(0, nextAngle, 0) * transform.forward;

            Vector3 currentPoint = origin + currentDir * viewDistance;
            Vector3 nextPoint = origin + nextDir * viewDistance;

            Gizmos.DrawLine(currentPoint, nextPoint); // ��ä���� ������ ���� �� �κ��� ����
        }
        // û�� ���� (����) �׸���
        Gizmos.color = Color.blue; // û�� ������ �Ķ������� ǥ��
        Gizmos.DrawWireSphere(origin, hearingRange);


    }
}