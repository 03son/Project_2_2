using UnityEngine;

public class SimpleDoor : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen = false; // ���� �����ִ� ����
    private bool openDoor = false; // ���� �����ִ� ����

    [SerializeField] private AudioSource audioSource; // AudioSource ������Ʈ
    [SerializeField] private AudioClip openSound; // ������ �Ҹ� Ŭ��
    [SerializeField] private AudioClip closeSound; // ������ �Ҹ� Ŭ��

    void Start()
    {
        animator = GetComponent<Animator>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource ������Ʈ�� ������ �߰�
        }
    }

    public void OnInteract()
    {
        if (isOpen)
        {
            CloseDoor(); // �� �ݱ�
        }
        else
        {
            OpenDoor(); // �� ����
        }
    }

    public string GetInteractPrompt()
    {
        return isOpen ? "�� �ݱ�" : "�� ����"; // ������Ʈ �ؽ�Ʈ ��ȯ
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

        isOpen = true;

        // ������ �Ҹ� ���
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }

    void CloseDoor()
    {
        if (animator != null)
        {
            animator.SetBool("isOpen", false); // �� �ݱ� �ִϸ��̼� ����
        }
        else
        {
            transform.position -= -transform.forward * 1.5f; // ���� �������� �ణ �̵�
        }

        isOpen = false;

        // ������ �Ҹ� ���
        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }
}
