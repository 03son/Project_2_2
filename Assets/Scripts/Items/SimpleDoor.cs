using UnityEngine;
using Photon.Pun; // Photon ��Ʈ��ŷ ���� ���ӽ����̽� �߰�

public class SimpleDoor : MonoBehaviour, IInteractable //IPunObservable
{
    private Animator animator;
    private bool isOpen = false; // ���� �����ִ� ����

    [SerializeField] private AudioSource audioSource; // AudioSource ������Ʈ
    [SerializeField] private AudioClip openSound; // ������ �Ҹ� Ŭ��
    [SerializeField] private AudioClip closeSound; // ������ �Ҹ� Ŭ��

    private PhotonView photonView; // PhotonView ����

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        photonView = GetComponentInParent<PhotonView>(); // PhotonView ������Ʈ ��������
    }

    public void OnInteract()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("ToggleDoor", RpcTarget.All, !isOpen);
        }
        else
        {
            // ���� ���۸� ���� (�̱��÷��� ����)
            ToggleDoor(!isOpen);
        }
    }

    public string GetInteractPrompt()
    {
        return isOpen ? "�� �ݱ�" : "�� ����"; // ������Ʈ �ؽ�Ʈ ��ȯ
    }

    [PunRPC]
    public void ToggleDoor(bool open)
    {
        animator = GetComponentInParent<Animator>();
        if (open) //T
        {
            OpenDoor(); // �� ����
        }
        else // F
        {
            CloseDoor(); // �� �ݱ�
        }
        isOpen = open;
    }

    void OpenDoor()
    {
        if (!isOpen) // �ߺ� ���� ����
        {
            if (animator != null)
            {
                // animator.SetTrigger("isOpen"); // �� ���� �ִϸ��̼� ����
                animator.SetBool("Open",true);
            }

            // ������ �Ҹ� ���
            if (audioSource != null && openSound != null)
            {
                audioSource.PlayOneShot(openSound);
            }
        }
    }

    void CloseDoor()
    {
        if (isOpen) // �ߺ� ���� ����
        {
            // ������ �Ҹ� ���
            if (audioSource != null && closeSound != null)
            {
                audioSource.PlayOneShot(closeSound);
            }
            if (animator != null)
            {
                //animator.SetTrigger("isClose"); // �� �ݱ� �ִϸ��̼� ����
                animator.SetBool("Open", false);
                return;
            }
        }
    }
    /*
    // ���� ����ȭ�� ���� �޼���
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ���� ���¸� ��Ʈ��ũ�� ����
            stream.SendNext(isOpen);
        }
        else
        {
            // ��Ʈ��ũ ���¸� ���ÿ� �ݿ�
            bool networkIsOpen = (bool)stream.ReceiveNext();

            // ���� ���� ����� ���� ó��
            if (networkIsOpen != isOpen)
            {
                isOpen = networkIsOpen;
                // ���¿� �´� ���� ����
                if (isOpen)
                {
                    OpenDoor();
                }
                else
                {
                    CloseDoor();
                }
            }
        }
    }
    */
}
