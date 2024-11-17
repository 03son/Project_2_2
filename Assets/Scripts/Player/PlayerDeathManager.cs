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

        // ���� ���¸� �ٸ� Ŭ���̾�Ʈ�� ����ȭ
        photonView.RPC("RPC_Die", RpcTarget.All, deathPosition);

        // ���� ȿ�� ǥ��
        if (deathEffect != null)
            Instantiate(deathEffect, deathPosition, Quaternion.identity);

        // ���� �÷��̾��� �����Ӱ� ��ȣ�ۿ� ���߱�
        GetComponent<PlayerMove>().enabled = false;
        GetComponent<PlayerDash>().enabled = false;
    }

    [PunRPC]
    void RPC_Die(Vector3 position)
    {
        Debug.Log("�÷��̾ �� ��ġ���� �׾����ϴ�: " + position);
        isDead = true;

        // �ٸ� Ŭ���̾�Ʈ������ �����Ӱ� ��ȣ�ۿ� ���߱�
        GetComponent<PlayerMove>().enabled = false;
        GetComponent<PlayerDash>().enabled = false;
    }

    public void Revive()
    {
        if (!isDead) return;

        isDead = false;

        // �÷��̾� ��Ȱ ��ġ �� ���� ����
        transform.position = deathPosition;
        GetComponent<PlayerMove>().enabled = true;
        GetComponent<PlayerDash>().enabled = true;

        // ��Ȱ ���¸� ����ȭ
        photonView.RPC("RPC_Revive", RpcTarget.All);
    }

    [PunRPC]
    void RPC_Revive()
    {
        isDead = false;
        GetComponent<PlayerMove>().enabled = true;
        GetComponent<PlayerDash>().enabled = true;
        Debug.Log("�÷��̾ ��Ȱ�߽��ϴ�.");
    }
}