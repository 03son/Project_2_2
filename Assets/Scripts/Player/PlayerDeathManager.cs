using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDeathManager : MonoBehaviourPunCallbacks
{
    public GameObject deathEffect; // ���� ȿ�� (�ִϸ��̼�, ��ƼŬ ��)

    PlayerState.playerState state;
    PlayerState playerState;

    PhotonView pv;
    private void Awake()
    {
        playerState = GetComponent<PlayerState>();

        playerState.State = PlayerState.playerState.Survival;
        playerState.GetState(out state);
        Debug.Log(state);

        pv = GetComponent<PhotonView>();

        GameObject[] mainCameras = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject Cam in mainCameras)
        {
            switch (Cam.name)
            {
                case "Main Camera":
                    CameraInfo.MainCam = Cam.GetComponent<Camera>();
                    break;

                case "ObserverCamera":
                    CameraInfo.ObserverCam = Cam.GetComponent<Camera>();
                    break;
            }
        }

        //1��Ī Ȱ��ȭ
        CameraInfo.UseMainCam();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && pv.IsMine)
        {
            playerState.State = PlayerState.playerState.Die;
            playerState.GetState(out state);
            Debug.Log(state);
            Die();
        }
    }

    void Die()
    {
        StartCoroutine(die());
    }

    IEnumerator die()
    {
        //UI �� ���� - ���� ���� �̺�Ʈ
        SetUICanvas.OpenUI("");

        yield return new WaitForSecondsRealtime(2);

        //���� ī�޶� Ȱ��ȭ
        CameraInfo.UseObserverCam();

        //1��Ī UI ���� ���� UI�ѱ�
        SetUICanvas.OpenUI("Observer");
    }
}

   /* void Start()
   {
        /*   playerState = GetComponent<PlayerState>();
           //state = GetComponent<PlayerState>().State;

           GetComponent<PlayerState>().State = PlayerState.playerState.����;
           playerState.GetState(out state);
           Debug.Log(state);
       } 

       private void OnTriggerEnter(Collider other)
       {
           if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
           {
               GetComponent<PlayerState>().State = PlayerState.playerState.����;
               Debug.Log(GetComponent<PlayerState>().State);
           }
       } */


/*
public void Die()
{
    if (isDead) return;

    isDead = true;
    deathPosition = transform.position;

    // ���� ȿ�� ǥ��
    if (deathEffect != null)
        Instantiate(deathEffect, deathPosition, Quaternion.identity);

    // ���� ���¸� �ٸ� Ŭ���̾�Ʈ�� ����ȭ
    //photonView.RPC("RPC_Die", RpcTarget.All, deathPosition);

    // ���� �÷��̾��� �����Ӱ� ��ȣ�ۿ� ���߱�
    GetComponent<PlayerMove>().enabled = false;
    GetComponent<PlayerDash>().enabled = false;
}

//[PunRPC]
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

//[PunRPC]
void RPC_Revive()
{
    isDead = false;
    GetComponent<PlayerMove>().enabled = true;
    GetComponent<PlayerDash>().enabled = true;
    Debug.Log("�÷��̾ ��Ȱ�߽��ϴ�.");
}

} */
