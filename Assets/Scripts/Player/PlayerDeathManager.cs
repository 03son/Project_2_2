using System.Collections;
using UnityEngine;
using Photon.Pun;

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
            playerState.State = PlayerState.playerState.Die;
            playerState.GetState(out state);
            Debug.Log(state);
            Die();
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
}
