using UnityEngine;
using Photon.Pun;

public class PlayerDeathManager : MonoBehaviourPunCallbacks
{
    public bool isDead = false;
    public GameObject deathEffect; // ���� ȿ�� (�ִϸ��̼�, ��ƼŬ ��)
    private Vector3 deathPosition;

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        deathPosition = transform.position;

        // ���� ȿ�� ǥ��
        if (deathEffect != null)
            Instantiate(deathEffect, deathPosition, Quaternion.identity);

        // ���� �÷��̾��� �����Ӱ� ��ȣ�ۿ� ���߱�
        GetComponent<PlayerMove>().enabled = false;
        GetComponent<PlayerDash>().enabled = false;

        // �̱� �÷��̿����� NotifyDeath()�� ���� ȣ��
        NotifyDeath(deathPosition);
    }

    void NotifyDeath(Vector3 position)
    {
        Debug.Log("�÷��̾ �� ��ġ���� �׾����ϴ�: " + position);
    }

    public void Revive()
    {
        if (!isDead) return;

        isDead = false;

        // �÷��̾� ��Ȱ ��ġ �� ���� ����
        transform.position = deathPosition;
        GetComponent<PlayerMove>().enabled = true;
        GetComponent<PlayerDash>().enabled = true;
    }
}
