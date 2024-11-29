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

        // SoundSource�� ���� �Ҹ� ó��
        SoundSource soundSource = GetComponent<SoundSource>();
        if (soundSource != null)
        {
            soundSource.PlaySound();  // SoundSource�� �˾Ƽ� �Ҹ��� �����ϵ��� ȣ��
        }

        // ���� ���� ������ ����
        if (brokenGlassPrefab != null)
        {
            Instantiate(brokenGlassPrefab, transform.position, transform.rotation);
        }

        // ���� ������ ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }


    /*void AlertMonsters(Vector3 breakPoint)
    {
        SoundSource soundSource = GetComponent<SoundSource>();
        // �ݰ� ���� ��� Collider ã��
        Collider[] colliders = Physics.OverlapSphere(breakPoint, 50f); // û�� ����(50����)

        foreach (var collider in colliders)
        {
            MonsterAI monsterAI = collider.GetComponent<MonsterAI>();  // MonsterAI ������Ʈ�� ���� �� Ž��
            if (monsterAI != null)
            {
                float distanceToMonster = Vector3.Distance(breakPoint, monsterAI.transform.position);

                // ���ú� ��� (�Ÿ� ���)
                float decibel = soundSource.baseDecibel - 20f * Mathf.Log10(distanceToMonster);

                // ������ û�� ������ ���ú� �Ӱ谪 Ȯ��
                if (distanceToMonster <= monsterAI.hearingRange && decibel >= monsterAI.minDecibelToDetect)
                {
                    Debug.Log($"Monster at {monsterAI.transform.position} detected sound with {decibel} dB");
                    monsterAI.SetInvestigatePoint(breakPoint);  // ������ ���� ������ ���� ������ ����
                }
                else
                {
                    Debug.Log($"Monster at {monsterAI.transform.position} could not detect the sound (Decibel: {decibel})");
                }
            }
        }
    }*/


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
