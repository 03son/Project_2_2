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
