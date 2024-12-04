using Photon.Pun;
using UnityEngine;

public class Door : MonoBehaviourPunCallbacks, IInteractable
{
    private Animator animator;
    private bool isOpen = false;
    private bool openDoor = false;
    private Player_Equip playerEquip; // Player_Equip ��ũ��Ʈ ����

    [SerializeField] private AudioSource audioSource; // AudioSource ������Ʈ
    [SerializeField] private AudioClip lockedSound; // ����ִ� �Ҹ� Ŭ��
    [SerializeField] private AudioClip openSound; // ������ �Ҹ� Ŭ��

    void Start()
    {
        animator = GetComponent<Animator>();
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip ������Ʈ�� ���� ������Ʈ ã��

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource ������Ʈ�� ������ �߰�
        }
    }

    public void OnInteract()
    {
        if (openDoor == true)
        {
            photonView.RPC("RPC_OpenDoor", RpcTarget.All); // �� ���� ����ȭ
            Debug.Log("���� �����ϴ�.");
            openDoor = false;
            return;
        }

        // ���� �������� �ʰ�, Player_Equip���� ���谡 ������ �������� Ȯ��
        if (!isOpen && playerEquip != null && playerEquip.HasEquippedKey())
        {
            isOpen = true;
            Debug.Log("���� ����� �����߽��ϴ�.");
            openDoor = true;

            playerEquip.photonView.RPC("RemoveEquippedItem", RpcTarget.All, "Key");


            // �� ��� ���� ���¸� ��� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("RPC_UnlockDoor", RpcTarget.All);

            return;
            isOpen = true;
            Debug.Log("���� ����� �����߽��ϴ�.");
            openDoor = true;

            playerEquip.photonView.RPC("RemoveEquippedItem", RpcTarget.All, "Key");


            // �� ��� ���� ���¸� ��� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("RPC_UnlockDoor", RpcTarget.All);

            return;
        }
        else if (!isOpen)
        {
            Debug.Log("��� ���Դϴ�. ���谡 �ʿ��մϴ�.");
            if (audioSource != null && lockedSound != null)
            {
                audioSource.PlayOneShot(lockedSound); // ����ִ� �Ҹ� ���
            }
        }

        if (isOpen && openDoor == false)
        {
            photonView.RPC("RPC_CloseDoor", RpcTarget.All); // �� �ݱ� ����ȭ
            Debug.Log("���� �����ϴ�.");
            openDoor = true;
            return;
        }
    }

    public string GetInteractPrompt()
    {
        if (isOpen == false)
        {
            return "�� �������";
        }
        else if (openDoor == false)
        {
            return "�� �ݱ�";
        }
        else if (openDoor == true)
        {
            return "�� ����";
        }
        return "???"; // �⺻ ��ȯ�� �߰�
    }

    [PunRPC]
    void RPC_OpenDoor()
    {
        if (animator != null)
        {
            animator.SetBool("Open", true); // �� ���� �ִϸ��̼� ����
        }
        else
        {
            transform.position += -transform.forward * 1.5f; // ���� ���������� �ణ �̵�
        }

        // ������ �Ҹ� ���
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }

    [PunRPC]
    void RPC_CloseDoor()
    {
        if (animator != null)
        {
            animator.SetBool("Open", false); // �� �ݱ� �ִϸ��̼� ����
        }
        else
        {
            transform.position -= -transform.forward * 1.5f; // ���� �������� �ణ �̵�
        }
    }

    [PunRPC]
    void RPC_UnlockDoor()
    {
        isOpen = true;
    }
}