using UnityEngine;

public class Door : MonoBehaviour, IInteractable
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
            OpenDoor();
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

            // Player_Equip�� RemoveEquippedItem ȣ��
            Inventory.instance.RemoveItem("Key");
            playerEquip.setEquipItem("Key");

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
            Closedoor();
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

    void OpenDoor()
    {
        if (animator != null)
        {
            animator.SetBool("isOpen", true); // �� ���� �ִϸ��̼� ����
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

    void Closedoor()
    {
        if (animator != null)
        {
            animator.SetBool("isClose", true); // �� �ݱ� �ִϸ��̼� ����
        }
        else
        {
            transform.position -= -transform.forward * 1.5f; // ���� �������� �ణ �̵�
        }
    }
}
