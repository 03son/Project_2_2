using Photon.Pun;
using UnityEngine;
using UnityEngine.Playables; // PlayableDirector 사용

public class SubmarineStart : MonoBehaviourPun, IInteractable
{
    public SubmarineController submarineController; // SubmarineController 참조
    public PlayableDirector playableDirector; // PlayableDirector 참조

    PhotonView pv;
    private bool playerInRange = false;

    private void Start()
    {
        if (submarineController == null)
        {
            Debug.LogError("SubmarineController가 할당되지 않았습니다. Inspector에서 연결하세요.");
        }

        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>(); // PlayableDirector 가져오기
        }

        pv = GetComponent<PhotonView>();
    }

    public string GetInteractPrompt()
    {
        return "F 키를 눌러 잠수함 시동";
    }

    public void OnInteract()
    {
        if (submarineController != null && submarineController.CanStart())
        {
            //submarineController.StartSubmarine();

            Debug.Log("잠수함 탈출 시퀀스 시작!");
            if (PhotonNetwork.IsConnected)
            {
                pv.RPC("Rpc_SubmarineStart", RpcTarget.All);
            }
            else
            {
                submarineController.StartSubmarine();
                // 3초 후 타임라인 재생
                Invoke("PlayEscapeTimeline", 3.0f);
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
        GameInfo.IsGameFinish = true;
        submarineController.StartSubmarine();
        // 3초 후 타임라인 재생
        Invoke("PlayEscapeTimeline", 3.0f);
    }

    private void PlayEscapeTimeline()
    {
        if (playableDirector != null)
        {
            //playableDirector.Play(); // Timeline 재생
            Debug.Log("탈출 타임라인 재생 시작");
        }
        else
        {
            //Debug.LogError("PlayableDirector가 없습니다.");
        }

        if (Multi_GameManager.instance.diePlayerCount == 0)
        {
            GameInfo.endingNumber = 0; //전원 탈출
        }
        else if (Multi_GameManager.instance.diePlayerCount < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            GameInfo.endingNumber = 1; //일부 생존
        }
        Multi_GameManager.instance.StartCoroutine(Multi_GameManager.instance.GoEndingVideo());
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
}
