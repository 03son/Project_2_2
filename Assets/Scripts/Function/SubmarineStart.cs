using Photon.Pun;
using UnityEngine;
using UnityEngine.Video; // VideoPlayer ���

public class SubmarineStart : MonoBehaviourPun, IInteractable
{
    public SubmarineController submarineController; // SubmarineController ����
    public VideoPlayer videoPlayer; // ���� �÷��̾�
    public VideoClip escapeVideoClip; // Ż�� ���� Ŭ��

    PhotonView pv;
    private bool playerInRange = false;

    private void Start()
    {
        if (submarineController == null)
        {
            Debug.LogError("SubmarineController�� �Ҵ���� �ʾҽ��ϴ�. Inspector���� �����ϼ���.");
        }

        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>(); // VideoPlayer�� ������ �߰�
        }

        pv = GetComponent<PhotonView>();

        videoPlayer.playOnAwake = false; // �ڵ� ��� ��Ȱ��ȭ
        videoPlayer.loopPointReached += OnVideoEnd; // ���� ���� �� �̺�Ʈ ����
    }

    public string GetInteractPrompt()
    {
        return "F Ű�� ���� ����� �õ�";
    }

    public void OnInteract()
    {
        if (submarineController != null && submarineController.CanStart())
        {
            submarineController.StartSubmarine();
            
            Debug.Log("����� Ż�� ������ ����!");
            if (PhotonNetwork.IsConnected)
            {
                pv.RPC("Rpc_SubmarineStart", RpcTarget.All);
            }
            else
            {
                // 3�� �� ���� ���
                Invoke("PlayEscapeVideo", 3.0f);
            }
        }
        else
        {
            Debug.Log("�ʿ��� ��ǰ�� ��� �������� �ʾҽ��ϴ�.");
        }
    }
    [PunRPC]
    public void Rpc_SubmarineStart()
    {
        // 3�� �� ���� ���
        Invoke("PlayEscapeVideo", 3.0f);
    }
    private void PlayEscapeVideo()
    {
        if (escapeVideoClip != null)
        {
            videoPlayer.clip = escapeVideoClip;
            videoPlayer.Play();
            Debug.Log("Ż�� ���� ��� ����");
        }
        else
        {
            Debug.LogError("Ż�� ���� Ŭ���� �����ϴ�.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("����� �õ� ���� ������ ����. F Ű�� ���� �õ� ����.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("�õ� ���� �������� ���.");
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Ż�� ���� ����. ���� ����.");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
