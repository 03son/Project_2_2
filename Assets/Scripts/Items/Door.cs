using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen = false;
    private bool openDoor = false;
    private Player_Equip playerEquip; // Player_Equip ��ũ��Ʈ ����

    void Start()
    {
        animator = GetComponent<Animator>();
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip ������Ʈ�� ���� ������Ʈ ã��
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
            //OpenDoor();
            // ���� ��ݻ��¸� �������� ����
            openDoor = true;
            return;
        }
        else if (!isOpen)
        {
            Debug.Log("��� ���Դϴ�. ���谡 �ʿ��մϴ�.");
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
            // Animator�� ���� ��� ���� ������ �������� �̵���Ű�� ���� (������ �̵�)
            transform.position += -transform.forward * 1.5f; // ���� ���������� �ణ �̵�
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
            // Animator�� ���� ��� ���� ������ �������� �̵���Ű�� ���� (������ �̵�)
            transform.position -= -transform.forward * 1.5f; // ���� �������� �ణ �̵�
        }
    }
}