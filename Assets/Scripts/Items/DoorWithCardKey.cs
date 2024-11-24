using UnityEngine;

public class DoorWithCardKey : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen = false;
    private Player_Equip playerEquip; // Player_Equip ��ũ��Ʈ ����

    [SerializeField] private AudioSource audioSource; // AudioSource ������Ʈ
    [SerializeField] private AudioClip openSound; // �� ���� �Ҹ�
    [SerializeField] private AudioClip closeSound; // �� ���� �Ҹ�
    [SerializeField] private float autoCloseDelay = 5f; // �� �� �� �ڵ����� ������ ����

    private float closeTimer = 0f;
    private bool autoClosing = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip ��ũ��Ʈ ã��

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // ���� ���� �ְ� �ڵ� ������ Ȱ��ȭ�� ��� Ÿ�̸� üũ
        if (isOpen && autoClosing)
        {
            closeTimer -= Time.deltaTime;
            if (closeTimer <= 0f)
            {
                CloseDoor();
            }
        }
    }

    public void OnInteract()
    {
        if (!isOpen && playerEquip != null && playerEquip.HasEquippedCardKey())
        {
            OpenDoor();
            return;
        }

        if (isOpen)
        {
            Debug.Log("���� �̹� ���� �ֽ��ϴ�.");
        }
        else
        {
            Debug.Log("���� �� �� �����ϴ�. CardKey�� �ʿ��մϴ�.");
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        autoClosing = true; // �ڵ� ���� Ȱ��ȭ
        closeTimer = autoCloseDelay; // Ÿ�̸� �ʱ�ȭ

        // ���� ���������� �̵�
        Vector3 moveDirection = -transform.forward * 1.5f; // ���������� 1.5 ���� �̵�
        transform.position += moveDirection;

        if (animator != null)
        {
            animator.SetBool("isOpen", true); // �ִϸ��̼� ����
        }

        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound); // ������ �Ҹ� ���
        }

        Debug.Log("���� ���Ƚ��ϴ�.");
    }

    void CloseDoor()
    {
        isOpen = false;
        autoClosing = false; // �ڵ� ���� ��Ȱ��ȭ

        // ���� �������� �̵� (���� ��ġ ����)
        Vector3 moveDirection = -transform.forward * -1.5f; // �������� 1.5 ���� �̵�
        transform.position += moveDirection;


        if (animator != null)
        {
            animator.SetBool("isOpen", false); // ������ �ִϸ��̼� ����
        }

        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound); // ������ �Ҹ� ���
        }

        Debug.Log("���� �������ϴ�.");
    }

    public string GetInteractPrompt()
    {
        if (!isOpen)
        {
            return "CardKey�� ����Ͽ� �� ����";
        }
        else
        {
            return "���� ���� �ֽ��ϴ�.";
        }
    }
}
