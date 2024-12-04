using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class WoodenPlank : MonoBehaviour, IInteractable
{
    private Player_Equip playerEquip; // Player_Equip ��ũ��Ʈ ����
    private bool isRemoving = false; // ���� ���� ����
    private float holdTime = 2.5f; // Ȧ�� �ð�
    private float holdCounter = 0f; // ���� ������ �ִ� �ð�

    [SerializeField] private AudioSource audioSource; // AudioSource ������Ʈ
    [SerializeField] private AudioClip holdSound; // Ű�� ������ ���� �� ȿ����
    [SerializeField] private Image progressBar; // ���� ���α׷��� �� UI

    void Start()
    {
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip ��ũ��Ʈ ã��
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false); // �ʱ� ��Ȱ��ȭ
        }
    }

    void Update()
    {
        // F Ű�� ������ ���� ���� Ÿ�̸� ����
        if (isRemoving && Input.GetKey(KeyCode.F))
        {
            // Ÿ�̸� ����
            holdCounter += Time.deltaTime;

            // UI ������Ʈ
            if (progressBar != null)
            {
                progressBar.fillAmount = holdCounter / holdTime;
            }

            // ȿ���� ���
            if (!audioSource.isPlaying && holdSound != null)
            {
                audioSource.loop = true; // �ݺ� ���
                audioSource.clip = holdSound;
                audioSource.Play();
            }

            // Ÿ�̸� �Ϸ� �� ���� ����
            if (holdCounter >= holdTime)
            {
                RemovePlank();
            }
        }
        else if (isRemoving && Input.GetKeyUp(KeyCode.F))
        {
            // Ű�� ���� ���� �ߴ�
            StopRemoving();
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

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true); // Ÿ�̸� UI Ȱ��ȭ
            progressBar.fillAmount = 0f; // �ʱ�ȭ
        }
    }

    private void StopRemoving()
    {
        isRemoving = false;

        // ȿ���� ����
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // UI ��Ȱ��ȭ
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
        }

        // Ÿ�̸� �ʱ�ȭ
        holdCounter = 0f;
    }

    private void RemovePlank()
    {
        StopRemoving();
        Debug.Log("���ڰ� ���ŵǾ����ϴ�.");
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                PhotonItem photonItem = player.GetComponent<PhotonItem>();
                if (photonItem != null)
                {
                    photonItem.RemoveEquippedItem("��������");
                    Debug.Log($"{"��������"} �������� ���ŵǾ����ϴ�.");
                }
            }
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
