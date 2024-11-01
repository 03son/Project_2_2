using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen = false;
    private Player_Equip playerEquip; // Player_Equip ��ũ��Ʈ ����

    void Start()
    {
        animator = GetComponent<Animator>();
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip ������Ʈ�� ���� ������Ʈ ã��
    }

    public void OnInteract()
    {
        // ���� �������� �ʰ�, Player_Equip���� ���谡 ������ �������� Ȯ��
        if (!isOpen && playerEquip != null && playerEquip.HasEquippedKey())
        {
            Debug.Log("���� ���Ƚ��ϴ�.");
            OpenDoor();
        }
        else if (!isOpen)
        {
            Debug.Log("��� ���Դϴ�. ���谡 �ʿ��մϴ�.");
        }
    }

    public string GetInteractPrompt()
    {
        return "�� ����";
    }

    void OpenDoor()
    {
        isOpen = true;
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
}