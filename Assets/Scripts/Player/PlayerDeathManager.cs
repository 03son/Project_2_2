using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDeathManager : MonoBehaviourPunCallbacks
{
    public GameObject deathEffect; // 죽음 효과 (애니메이션, 파티클 등)

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

        //1인칭 활성화
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
        //UI 다 끄기 - 몬스터 공격 이벤트
        SetUICanvas.OpenUI("");

        yield return new WaitForSecondsRealtime(2);

        //관전 카메라 활성화
        CameraInfo.UseObserverCam();

        //1인칭 UI 끄고 관전 UI켜기
        SetUICanvas.OpenUI("Observer");
    }
}
