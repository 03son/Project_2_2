using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.Linq;


public class PlayerDeathManager : MonoBehaviourPunCallbacks
{
    public GameObject deathEffect; // 사망 효과 (애니메이션, 파티클 등)

    PlayerState.playerState state;
    PlayerState playerState;

    PhotonView pv;
    private Animator animator; // Animator 추가

    private void Awake()
    {
        playerState = GetComponent<PlayerState>();

        playerState.State = PlayerState.playerState.Survival;
        playerState.GetState(out state);
        Debug.Log(state);

        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기

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

        // 메인 카메라 활성화
        CameraInfo.UseMainCam();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && pv.IsMine)
        {
            Debug.Log("적과 충돌하여 사망 상태로 전환.");

            // RPC 호출 시 ActorNumber 전달
            photonView.RPC("SyncDieState", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }


    [PunRPC]
    void SyncDieState(int actorNumber)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            Debug.Log("로컬 플레이어가 죽은 상태로 전환됩니다.");
            playerState.State = PlayerState.playerState.Die; // 로컬 플레이어 상태 변경
            Die(); // 로컬에서 Die 메서드 호출
        }
        else
        {
            Debug.Log($"ActorNumber {actorNumber}의 상태를 동기화.");
            PlayerDeathManager targetPlayer = PhotonNetwork.PlayerList
                .FirstOrDefault(p => p.ActorNumber == actorNumber)?
                .TagObject as PlayerDeathManager;

            if (targetPlayer != null)
            {
                targetPlayer.playerState.State = PlayerState.playerState.Die; // 다른 클라이언트 동기화
            }
        }
    }

    void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death"); // Death 애니메이션 트리거 발동
            Debug.Log("Death 애니메이션 발동");
        }

        StartCoroutine(die());
    }

    IEnumerator die()
    {
        // UI 및 효과 적용
        SetUICanvas.OpenUI("");

        yield return new WaitForSecondsRealtime(2);

        // 사망 효과 추가 (필요시)
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // 관찰 카메라 활성화
        CameraInfo.UseObserverCam();

        // 관찰자 UI 띄우기
        SetUICanvas.OpenUI("Observer");
    }

    public void Survival()
    {
        if (animator != null)
        {
            animator.SetTrigger("Survival"); // Survival 애니메이션 트리거 발동
            Debug.Log("Survival 애니메이션 발동");
        }

        // UI 복구
        CameraInfo.UseMainCam(); // 메인 카메라 활성화
        SetUICanvas.OpenUI("HUD"); // HUD UI 활성화
    }


}