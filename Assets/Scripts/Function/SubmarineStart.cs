using Photon.Pun;
using UnityEngine;
using UnityEngine.Playables; // PlayableDirector ���

public class SubmarineStart : MonoBehaviourPun, IInteractable
{
    public SubmarineController submarineController; // SubmarineController ����
    public PlayableDirector playableDirector; // PlayableDirector ����

    PhotonView pv;
    private bool playerInRange = false;

    private void Start()
    {
        if (submarineController == null)
        {
            Debug.LogError("SubmarineController�� �Ҵ���� �ʾҽ��ϴ�. Inspector���� �����ϼ���.");
        }

        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>(); // PlayableDirector ��������
        }

        pv = GetComponent<PhotonView>();
    }

    public string GetInteractPrompt()
    {
        return "F Ű�� ���� ����� �õ�";
    }

    public void OnInteract()
    {
        if (submarineController != null && submarineController.CanStart())
        {
            //submarineController.StartSubmarine();

            Debug.Log("����� Ż�� ������ ����!");
            if (PhotonNetwork.IsConnected)
            {
                pv.RPC("Rpc_SubmarineStart", RpcTarget.All);
            }
            else
            {
                submarineController.StartSubmarine();
                // 3�� �� Ÿ�Ӷ��� ���
                Invoke("PlayEscapeTimeline", 3.0f);
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
        GameInfo.IsGameFinish = true;
        submarineController.StartSubmarine();
        // 3�� �� Ÿ�Ӷ��� ���
        Invoke("PlayEscapeTimeline", 3.0f);
    }

    private void PlayEscapeTimeline()
    {
        if (playableDirector != null)
        {
            //playableDirector.Play(); // Timeline ���
            Debug.Log("Ż�� Ÿ�Ӷ��� ��� ����");
        }
        else
        {
            //Debug.LogError("PlayableDirector�� �����ϴ�.");
        }

        if (Multi_GameManager.instance.diePlayerCount == 0)
        {
            GameInfo.endingNumber = 0; //���� Ż��
        }
        else if (Multi_GameManager.instance.diePlayerCount < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            GameInfo.endingNumber = 1; //�Ϻ� ����
        }
        Multi_GameManager.instance.StartCoroutine(Multi_GameManager.instance.GoEndingVideo());
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
}
