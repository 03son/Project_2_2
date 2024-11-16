using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public float hearingRange = 50f; // ������ û�� ����
    public NavMeshAgent navAgent; // ���� �̵��� ���� NavMeshAgent

    private Vector3 lastHeardPosition; // ���������� ������ �Ҹ� ��ġ
    private bool isChasingSound = false; // �Ҹ� ���� ������ ����

    void Start()
    {
        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
        }
    }

    public void ReactToSound(Vector3 soundPosition)
    {
        // �Ҹ� ���� �� �ش� ��ġ�� �̵�
        lastHeardPosition = soundPosition;
        isChasingSound = true;
        navAgent.SetDestination(lastHeardPosition); // NavMesh�� ���� �̵�
        Debug.Log($"Boss moving to sound position: {soundPosition}");
    }

    void Update()
    {
        if (isChasingSound)
        {
            // ��ǥ ������ �����ߴ��� Ȯ��
            if (Vector3.Distance(transform.position, lastHeardPosition) < 2f)
            {
                isChasingSound = false; // ���� ����
                Debug.Log("Boss reached the sound position.");
            }
        }
    }
}
