using Photon.Pun;
using UnityEngine;
using UnityEngine.Video; // VideoPlayer 사용

public class SubmarineStart : MonoBehaviourPun, IInteractable
{
    public SubmarineController submarineController; // SubmarineController 참조
    public VideoPlayer videoPlayer; // 비디오 플레이어
    public VideoClip escapeVideoClip; // 탈출 비디오 클립

    PhotonView pv;
    private bool playerInRange = false;

    private void Start()
    {
        if (submarineController == null)
        {
            Debug.LogError("SubmarineController가 할당되지 않았습니다. Inspector에서 연결하세요.");
        }

        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>(); // VideoPlayer가 없으면 추가
        }

        pv = GetComponent<PhotonView>();

        videoPlayer.playOnAwake = false; // 자동 재생 비활성화
        videoPlayer.loopPointReached += OnVideoEnd; // 비디오 종료 시 이벤트 연결
    }

    public string GetInteractPrompt()
    {
        return "F 키를 눌러 잠수함 시동";
    }

    public void OnInteract()
    {
        if (submarineController != null && submarineController.CanStart())
        {
            submarineController.StartSubmarine();
            
            Debug.Log("잠수함 탈출 시퀀스 시작!");
            if (PhotonNetwork.IsConnected)
            {
                pv.RPC("Rpc_SubmarineStart", RpcTarget.All);
            }
            else
            {
                // 3초 후 비디오 재생
                Invoke("PlayEscapeVideo", 3.0f);
            }
        }
        else
        {
            Debug.Log("필요한 부품이 모두 부착되지 않았습니다.");
        }
    }
    [PunRPC]
    public void Rpc_SubmarineStart()
    {
        // 3초 후 비디오 재생
        Invoke("PlayEscapeVideo", 3.0f);
    }
    private void PlayEscapeVideo()
    {
        if (escapeVideoClip != null)
        {
            videoPlayer.clip = escapeVideoClip;
            videoPlayer.Play();
            Debug.Log("탈출 비디오 재생 시작");
        }
        else
        {
            Debug.LogError("탈출 비디오 클립이 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("잠수함 시동 가능 지점에 도달. F 키를 눌러 시동 가능.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("시동 가능 지점에서 벗어남.");
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("탈출 비디오 종료. 게임 종료.");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
