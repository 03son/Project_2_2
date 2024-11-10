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
                Debug.LogError("MedkitTimerUI를 찾을 수 없습니다. 씬에 해당 컴포넌트가 있는지 확인하세요.");
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
                    Debug.Log("RPC 호출 시도 중");
                    medkitTimerUI.photonView.RPC("RPC_TriggerMedkitTimer", RpcTarget.All);
                }
            }

            holdCounter += Time.deltaTime;

            if (holdCounter >= holdTime)
            {
                // 메딕킷 사용 RPC 호출
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
                Debug.Log("플레이어가 부활했습니다.");
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
            Debug.Log("죽은 플레이어 발견. E 키를 눌러 부활 가능.");
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
