using System.Collections;
using UnityEngine;

public class GlassCupCollisionHandler : MonoBehaviour
{
    [Header("Glass Cup Settings")]
    public AudioClip breakSound;  // �������� ���� �� ���� �Ҹ�
    public GameObject brokenGlassPrefab;  // �������� ������ �� ������ ������ (���� ���� ����)

    private bool hasBroken = false;  // �������� �̹� �������� Ȯ���ϴ� ����

    void OnCollisionEnter(Collision collision)
    {
        // �������� �̹� �����ٸ� �� �̻� ó������ ����
        if (hasBroken)
            return;

        // ���� �ӵ� �̻��� �浹�� ��� ���� ó��
        if (collision.relativeVelocity.magnitude > 2f)
        {
            BreakGlass(collision.contacts[0].point);
        }
    }

    void BreakGlass(Vector3 breakPoint)
    {
        hasBroken = true;

        // ������ �Ҹ� ���
        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, breakPoint);
        }

        // ���� ���� ������ ����
        if (brokenGlassPrefab != null)
        {
           // Instantiate(brokenGlassPrefab, transform.position, transform.rotation);
        }

        // ���͵鿡�� ���� ��ġ �˸�
        AlertMonsters(breakPoint);

        // ���� ������ ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }

    void AlertMonsters(Vector3 breakPoint)
    {
        // �ݰ� ���� ��� Collider ã��
        Collider[] colliders = Physics.OverlapSphere(breakPoint, 50f); // û�� ����(50����)

        foreach (var collider in colliders)
        {
            MonsterAI monsterAI = collider.GetComponent<MonsterAI>();  // MonsterAI ������Ʈ�� ���� �� Ž��
            if (monsterAI != null)
            {
                float distanceToMonster = Vector3.Distance(breakPoint, monsterAI.transform.position);

                // ������ û�� ���� ���� �ִ� ��쿡�� ����
                if (distanceToMonster <= monsterAI.hearingRange)
                {
                    monsterAI.SetInvestigatePoint(breakPoint);  // ������ ���� ������ ���� ������ ����
                }
            }
        }
    }

    // �������� ȹ������ �� ���� ������ �����ϴ� �޼���
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
            col.isTrigger = true;  // �÷��̾�� �浹���� �ʰ� �ϱ� ����
        }
    }
}
