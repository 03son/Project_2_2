using Photon.Pun;
using UnityEngine;
using UnityEngine.Video; // VideoPlayer를 사용하기 위해 추가
using System.Collections; // 코루틴을 사용하기 위해 추가

public class SubmarineStart : MonoBehaviourPun, IInteractable
{
    public SubmarineController submarineController; // Inspector에서 연결할 수 있도록 public으로 설정
    public VideoPlayer videoPlayer; // 동영상을 재생할 VideoPlayer
    public VideoClip escapeVideoClip; // 탈출 시퀀스 비디오 클립

    private bool playerInRange = false;

    private void Start()
    {
        if (submarineController == null)
        {
            Debug.LogError("SubmarineController가 할당되지 않았습니다. Inspector에서 연결해 주세요.");
        }

        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>(); // VideoPlayer가 없으면 추가
        }

        videoPlayer.playOnAwake = false; // 게임 시작 시 자동 재생 방지
        videoPlayer.loopPointReached += OnVideoEnd; // 비디오 재생 종료 시 이벤트 연결
    }

    // IInteractable 인터페이스 구현
    public string GetInteractPrompt()
    {
        return "를 눌러 잠수함 시동";
    }

    public void OnInteract()
    {
        if (submarineController != null && submarineController.CanStart())
        {
            submarineController.StartSubmarine();
            Debug.Log("탈출 시퀀스가 시작됩니다! (애니메이션 대체 디버그)");

            // 3초 후 탈출 시퀀스 비디오 재생 시작
            StartCoroutine(PlayEscapeVideoWithDelay());
        }
        else
        {
            Debug.Log("모든 부품이 부착되지 않았습니다. 잠수함을 시작할 수 없습니다.");
        }
    }

    private IEnumerator PlayEscapeVideoWithDelay()
    {
        yield return new WaitForSeconds(3f); // 3초 대기

        if (escapeVideoClip != null)
        {
            videoPlayer.clip = escapeVideoClip;
            videoPlayer.gameObject.SetActive(true); // VideoPlayer 활성화
            videoPlayer.Play(); // 비디오 재생
        }
        else
        {
            Debug.LogError("탈출 비디오 클립이 할당되지 않았습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("잠수함 시동 지점에 도달. F 키를 눌러 시동을 걸 수 있습니다.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("시동 지점에서 벗어남.");
        }
    }

    // 비디오 재생 종료 시 호출되는 메서드
    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("탈출 비디오가 종료되었습니다. 게임을 종료합니다.");
        Application.Quit(); // 게임 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
