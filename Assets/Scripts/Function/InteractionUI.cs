using UnityEngine;
using Photon.Pun; // Photon 사용

public class InteractionUI : MonoBehaviourPun
{
    public GameObject uiImage; // UI Image를 드래그로 연결
    private bool isPlayerInRange = false;
    private PhotonView localPlayerView; // 로컬 플레이어의 PhotonView

    void Update()
    {
        // 로컬 플레이어만 UI를 제어할 수 있음
        if (localPlayerView != null && localPlayerView.IsMine && isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            uiImage.SetActive(!uiImage.activeSelf); // UI 상태를 토글
        }
    }

    // 플레이어가 상호작용 영역에 들어왔을 때
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView playerView = other.GetComponent<PhotonView>();
            if (playerView != null && playerView.IsMine) // 로컬 플레이어인지 확인
            {
                localPlayerView = playerView; // 로컬 플레이어의 PhotonView 저장
                isPlayerInRange = true; // 상호작용 가능 상태로 설정
                uiImage.SetActive(true); // UI 자동 활성화
            }
        }
    }

    // 플레이어가 상호작용 영역을 떠났을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView playerView = other.GetComponent<PhotonView>();
            if (playerView != null && playerView.IsMine) // 로컬 플레이어인지 확인
            {
                localPlayerView = null; // 로컬 플레이어와의 연결 해제
                isPlayerInRange = false; // 상호작용 불가능 상태로 설정
                uiImage.SetActive(false); // UI 비활성화
            }
        }
    }
}
