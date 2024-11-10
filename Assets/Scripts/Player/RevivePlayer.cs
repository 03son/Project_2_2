using Photon.Pun;
using UnityEngine;
using System.Collections;

public class RevivePlayer : MonoBehaviourPun
{
    public float holdTime = 5f;
    private float holdCounter = 0f;
    private bool isHolding = false;
    private PlayerDeathManager targetPlayer;

    public MedkitTimerUI medkitTimerUI;

    void Start()
    {
        if (medkitTimerUI == null)
        {
            medkitTimerUI = FindObjectOfType<MedkitTimerUI>();
            if (medkitTimerUI == null)
            {
                Debug.LogError("MedkitTimerUI�� ã�� �� �����ϴ�. ���� �ش� ������Ʈ�� �ִ��� Ȯ���ϼ���.");
            }
        }
    }

    void Update()
    {
        if (targetPlayer != null && Input.GetKey(KeyCode.E))
        {
            if (!isHolding)
            {
                isHolding = true;
                if (medkitTimerUI != null && medkitTimerUI.photonView.IsMine)
                {
                    Debug.Log("RPC ȣ�� �õ� ��");
                    medkitTimerUI.photonView.RPC("RPC_TriggerMedkitTimer", RpcTarget.All);
                }
            }

            holdCounter += Time.deltaTime;

            if (holdCounter >= holdTime)
            {
                // �޵�Ŷ ��� RPC ȣ��
                photonView.RPC("RPC_UseMedkit", RpcTarget.All, targetPlayer.photonView.ViewID);
                ResetHold();
            }
        }
        else if (Input.GetKeyUp(KeyCode.E) || !isHolding)
        {
            ResetHold();
        }
    }



    [PunRPC]
    public void RPC_UseMedkit(int targetPlayerViewID)
    {
        PhotonView targetPhotonView = PhotonView.Find(targetPlayerViewID);
        if (targetPhotonView != null)
        {
            PlayerDeathManager targetPlayer = targetPhotonView.GetComponent<PlayerDeathManager>();
            if (targetPlayer != null && targetPlayer.isDead)
            {
                targetPlayer.Revive();
                Debug.Log("�÷��̾ ��Ȱ�߽��ϴ�.");
            }
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        holdCounter = 0f;
        if (medkitTimerUI != null)
        {
            medkitTimerUI.ResetTimer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerDeathManager>().isDead)
        {
            targetPlayer = other.GetComponent<PlayerDeathManager>();
            Debug.Log("���� �÷��̾� �߰�. E Ű�� ���� ��Ȱ ����.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetPlayer = null;
            ResetHold();
        }
    }
}
