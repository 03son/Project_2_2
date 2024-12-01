using UnityEngine;

public class WoodenPlank : MonoBehaviour, IInteractable
{
    private Player_Equip playerEquip; // Player_Equip ��ũ��Ʈ ����
    private bool isRemoving = false; // ���� ���� ����
    private float holdTime = 2.5f; // Ȧ�� �ð�
    private float holdCounter = 0f; // ���� ������ �ִ� �ð�

    [SerializeField] private AudioSource audioSource; // AudioSource ������Ʈ
    [SerializeField] private AudioClip holdSound; // Ű�� ������ ���� �� ȿ����

    void Start()
    {
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip ��ũ��Ʈ ã��
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isRemoving)
        {
            // Ÿ�̸� ����
            holdCounter += Time.deltaTime;

            // ȿ���� ���
            if (!audioSource.isPlaying && holdSound != null)
            {
                audioSource.loop = true;
                audioSource.clip = holdSound;
                audioSource.Play();
            }

            // Ÿ�̸� �Ϸ� �� ���� ����
            if (holdCounter >= holdTime)
            {
                RemovePlank();
            }
        }
    }

    public void OnInteract()
    {
        if (playerEquip != null && playerEquip.HasEquippedCrowBar())
        {
            if (!isRemoving)
            {
                StartRemoving(); // ���� ����
            }
        }
        else
        {
            Debug.Log("CrowBar�� �ʿ��մϴ�.");
        }
    }

    public string GetInteractPrompt()
    {
        return "F Ű�� ���� ���� ���� ����";
    }

    private void StartRemoving()
    {
        isRemoving = true;
        holdCounter = 0f;
    }

    private void StopRemoving()
    {
        isRemoving = false;

        // ȿ���� ����
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void RemovePlank()
    {
        StopRemoving();
        Debug.Log("���ڰ� ���ŵǾ����ϴ�.");

        // CrowBar ����
        if (playerEquip != null)
        {
            playerEquip.RemoveEquippedItem("CrowBar"); // CrowBar ���� �޼��� ȣ��
        }

        Destroy(gameObject); // ���� ����
    }

    void OnTriggerExit(Collider other)
    {
        // �÷��̾ ������ ����� ���� �ߴ�
        if (other.CompareTag("Player"))
        {
            StopRemoving();
        }
    }
}
