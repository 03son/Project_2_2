using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public NavMeshAgent agent;                     // NavMeshAgent ������Ʈ
    public Transform[] patrolPoints;                // ���� ���� �迭
    public LayerMask playerLayer;                   // �÷��̾ ���� ���̾�

    private List<Transform> detectedPlayers;       // ������ �÷��̾� ����Ʈ
    private int currentPatrolIndex;                 // ���� ���� ���� �ε���
    private Vector3 lastKnownPosition;              // �÷��̾� ������ ��ġ
    private Vector3 soundHeardPosition;             // �Ҹ��� ���� ��ġ ����

    public float viewDistance = 10f;                // �þ� �Ÿ�
    public float fieldOfView = 120f;                // �þ߰�
    public float hearingRange = 15f;                // û�� ����

    private enum State { Patrol, Chase, Search, Investigate };   // ���� ���ǿ� Investigate �߰�
    private State currentState;

    private void Start()
    {
        // NavMeshAgent �ʱ�ȭ
        agent = GetComponent<NavMeshAgent>();

        // �÷��̾ �������� �Ҵ����� �ʾҴٸ� �ڵ����� ã��
        detectedPlayers = new List<Transform>();

        // NavMeshAgent�� ������ ����
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent�� " + gameObject.name + "�� �����ϴ�.");
            enabled = false; // ��ũ��Ʈ�� ��Ȱ��ȭ�Ͽ� �߰� ���� ����
            return;
        }

        // �ʱ� ���� ����
        currentState = State.Patrol;
        currentPatrolIndex = 0;
        GoToNextPatrolPoint();

        // ������ ���� �Ҹ��� ��� �����ϴ� �̺�Ʈ ���
        GlassCupFunction.OnGlassBreak += OnGlassBreakHeard;
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ��� ����
        GlassCupFunction.OnGlassBreak -= OnGlassBreakHeard;
    }

    private void Update()
    {
        // ���� ���¿� ���� ������ �ൿ ����
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
            case State.Investigate:  // �Ҹ� �߻� ��ġ�� �̵�
                Investigate();
                break;
        }

        // �÷��̾� ���� ���� ������Ʈ
        UpdateDetectedPlayers();
    }

    private void Patrol()
    {
        // ���� ����: ���� ���� �������� �̵�
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }

        // �÷��̾ �����ϸ� ���� ���·� ��ȯ
        if (detectedPlayers.Count > 0)
        {
            currentState = State.Chase;
        }
    }

    private void Chase()
    {
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

    private void Search()
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

    private void Investigate()
    {
        // �������� ���� ��ġ�� �̵�
        agent.SetDestination(soundHeardPosition);

        // �ش� ��ġ�� �����ϸ� ���� ���·� ���ư�
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
            if (CanSeePlayer(playerTransform) || CanHearPlayer(playerTransform))
            {
                // ������ �÷��̾ ����Ʈ�� �߰�
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

    private bool CanHearPlayer(Transform player)
    {
        // �÷��̾ û�� ���� ���� �ִ��� Ȯ��
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �÷��̾ �Ҹ��� ���� ������ Ȯ��
        Player playerSound = player.GetComponent<Player>();
        return distanceToPlayer <= hearingRange && playerSound != null && playerSound.audioSource.isPlaying;
    }

    private void OnGlassBreakHeard(Vector3 position)
    {
        // ������ ���� �Ҹ��� ����� ��, �Ҹ��� ��ġ�� �̵��ϵ��� ����
        float distanceToSound = Vector3.Distance(transform.position, position);

        if (distanceToSound <= hearingRange)
        {
            soundHeardPosition = position;
            currentState = State.Investigate;  // �Ҹ� �鸰 ������ �̵�
        }
    }
}
