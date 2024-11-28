using UnityEngine;
using Photon.Pun;

public class HelicopterController : MonoBehaviourPun
{
    private bool isChainRemoved = false; // �罽 ���� ����
    private bool isFuelAdded = false;   // ���� ���� ����
    private bool isStarted = false;     // ��� �õ� ����
    private Player_Equip playerEquip; // �÷��̾��� Player_Equip ��ũ��Ʈ ����

    public AudioSource audioSource;
    public AudioClip startSound;

    private void Start()
    {
        // AudioSource�� ������ �ڵ����� �߰�
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    // �罽 ���� �޼���
    [PunRPC]
    public void RemoveChain()
    {
        if (isChainRemoved) return;

        isChainRemoved = true;
        Debug.Log("�罽�� ���ŵǾ����ϴ�.");

        // �罽 ������Ʈ ��Ȱ��ȭ
        Transform chainTransform = transform.Find("Chain");
        if (chainTransform != null)
        {
            chainTransform.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("�罽 ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    // ���� ���� �޼���
    [PunRPC]
    public void AddFuel()
    {
        if (isFuelAdded) return;

        isFuelAdded = true;
        Debug.Log("���ᰡ ���ԵǾ����ϴ�.");
    }

    // �õ� ���� ���� Ȯ��
    public bool CanStart()
    {
        return isChainRemoved && isFuelAdded;
    }

    // ��� �õ� �޼���
    public bool StartHelicopter()
    {
        if (isStarted) return false; // �̹� �õ��� �ɸ� ���¶�� false ��ȯ

        if (CanStart())
        {
            isStarted = true;
            Debug.Log("��� �õ� ����!");

            // ����� ���
            if (audioSource != null && startSound != null)
            {
                audioSource.clip = startSound;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource �Ǵ� StartSound�� �������� �ʾҽ��ϴ�.");
            }

            // ��� Ż�� ���� ����
            Invoke(nameof(EscapeSequence), 3.0f);
            return true; // �õ��� ���������� true ��ȯ
        }
        else
        {
            Debug.Log("�罽�� ���� ���¸� Ȯ���ϼ���.");
            return false; // �õ� ������ �������� ������ false ��ȯ
        }
    }


    private void EscapeSequence()
    {
        Debug.Log("��� Ż�� ����!");
        // Timeline ���� �Ǵ� �� ��ȯ ó��
        // ���⿡ ���ϴ� Ż�� ���� ���� �߰�
    }

    // �̱��÷��̿� ��Ƽ�÷��� ����
    public void RemoveChainLocal()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC(nameof(RemoveChain), RpcTarget.All);
        }
        else
        {
            RemoveChain();
        }
    }

    public void AddFuelLocal()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC(nameof(AddFuel), RpcTarget.All);
        }
        else
        {
            AddFuel();
        }
    }

    public void StartHelicopterLocal()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC(nameof(StartHelicopter), RpcTarget.All);
        }
        else
        {
            StartHelicopter();
        }
    }

    


}
