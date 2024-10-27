using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Stunnable : MonoBehaviour
{
    private bool isStunned = false;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent�� " + gameObject.name + "�� �����ϴ�.");
            enabled = false;
        }
    }

    public void Stun(float stunDuration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(stunDuration));
        }
    }

    private IEnumerator StunCoroutine(float stunDuration)
    {
        isStunned = true;
        if (agent != null)
        {
            agent.isStopped = true; // NavMeshAgent ����
        }

        yield return new WaitForSeconds(stunDuration);

        if (agent != null)
        {
            agent.isStopped = false; // NavMeshAgent �ٽ� ����
        }
        isStunned = false;
    }
}
