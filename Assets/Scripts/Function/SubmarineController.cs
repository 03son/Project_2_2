using Photon.Pun;
using UnityEngine;

public class SubmarineController : MonoBehaviourPun
{
    private bool isPropellerAttached = false;
    private int attachedBatteries = 0; // ���͸� ��
    public int requiredBatteries = 2; // �ʿ��� ���͸� ��

    private bool isStarted = false;

    public AudioSource audioSource; // ����� �Ҹ�
    public AudioClip startSound; // �õ� �Ҹ�

    PhotonView pv;
    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        pv = GetComponent<PhotonView>();
    }

    public void AttachItem(string itemName)
    {
        if (itemName == "Propeller")
        {
            isPropellerAttached = true;
            Debug.Log("�����緯�� �����Ǿ����ϴ�.");
        }
        else if (itemName == "Battery")
        {
            attachedBatteries++;
            Debug.Log($"���͸��� �����Ǿ����ϴ�. ���� ���͸� ��: {attachedBatteries}/{requiredBatteries}");
        }
    }

    public bool CanStart()
    {
        return isPropellerAttached && attachedBatteries >= requiredBatteries;
    }

    public void Submarine()
    {
        pv.RPC("StartSubmarine", RpcTarget.All);
    }
    [PunRPC]
    public void StartSubmarine()
    {
        if (isStarted)
        {
            Debug.Log("������� �̹� �۵� ���Դϴ�.");
            return;
        }

        if (CanStart())
        {
            isStarted = true;
            Debug.Log("����� �õ� ����!");
            if (audioSource != null && startSound != null)
            {
                audioSource.clip = startSound;
                audioSource.Play();
            }
        }
        else
        {
            Debug.Log("�ʿ��� ��ǰ�� ��� �������� �ʾҽ��ϴ�.");
        }
    }
}
