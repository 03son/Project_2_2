using UnityEngine;
using Photon.Pun;

public class FuelInteract : MonoBehaviourPun, IInteractable
{
    public string requiredItem = "Sump"; // �ʿ��� ������ �̸�
    public float holdTime = 10f;         // Ȧ�� �ð�
    private bool isFuelAdded = false;    // ���� �߰� ����
    private float holdProgress = 0f;     // Ȧ�� ���� �ð�
    private bool isHolding = false;      // Ȧ�� ������ ����

    [Header("Audio Settings")]
    public AudioSource audioSource;        // AudioSource ������Ʈ
    public AudioClip fuelAddingSound;      // ���� ���� �� ����
    public AudioClip fuelAddedCompleteSound; // ���� ���� �Ϸ� ����
    public string GetInteractPrompt()
    {
        // ���ᰡ �̹� �߰��� ���¸� �ؽ�Ʈ�� ǥ������ ����
        return isFuelAdded ? "" : $"{requiredItem}�� ���� �����ϱ� ({holdProgress:F1}/{holdTime:F1}��)";
    }

    public void OnInteract()
    {
        if (isFuelAdded) return; // �̹� ���ᰡ �߰��� ���¸� �������� ����

        Inventory inventory = Inventory.instance;
        if (!inventory.HasItem(requiredItem)) // �ʿ��� ������ Ȯ��
        {
            Debug.Log($"{requiredItem}��(��) �ʿ��մϴ�.");
            return;
        }

        isHolding = true; // Ȧ�� ����

        // ���� ���� ���� ����
        if (fuelAddingSound != null && audioSource != null)
        {
            audioSource.clip = fuelAddingSound;
            audioSource.loop = true; // �ݺ� ���
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (isHolding && !isFuelAdded)
        {
            if (Input.GetKey(KeyCode.F)) // F Ű�� �� ������ �ִ� ���
            {
                holdProgress += Time.deltaTime; // ���� �ð� ����
                if (holdProgress >= holdTime) // Ȧ�� �ð��� �Ϸ�Ǹ�
                {
                    CompleteInteract(); // �۾� �Ϸ�
                }
            }
            else
            {
                CancelInteract(); // Ű�� ���� �۾� ���
            }

            // InteractionManager�� ������Ʈ�� �ǽð����� ������Ʈ�ϵ��� ȣ��
            InteractionManager.UpdatePrompt(this);
        }
    }

    private void CompleteInteract()
    {
        isHolding = false;
        holdProgress = 0f;

        // ���� ���� �Ϸ� ����
        if (audioSource != null)
        {
            audioSource.Stop(); // ���� �� �Ҹ� ����
            if (fuelAddedCompleteSound != null)
            {
                audioSource.PlayOneShot(fuelAddedCompleteSound); // �Ϸ� ���� ���
            }
        }

        Inventory inventory = Inventory.instance;

        // �κ��丮���� ������ ����
        Player_Equip playerEquip = FindObjectOfType<Player_Equip>();
        if (playerEquip != null)
        {
            playerEquip.RemoveEquippedItem(requiredItem);
            Debug.Log($"{requiredItem} �������� ���ŵǾ����ϴ�.");
        }

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_AddFuel", RpcTarget.All);
        }
        else
        {
            RPC_AddFuel();
        }
    }


    private void CancelInteract()
    {
        isHolding = false;
        holdProgress = 0f;

        // ���� �� �Ҹ� ����
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        Debug.Log("���� ���� ��ҵ�");
    }


    [PunRPC]
    private void RPC_AddFuel()
    {
        if (isFuelAdded) return; // �̹� ���ᰡ �߰��� ���¸� �������� ����

        isFuelAdded = true;
        HelicopterController helicopter = GetComponentInParent<HelicopterController>();

        if (PhotonNetwork.IsConnected)
        {
            helicopter?.photonView.RPC("AddFuel", RpcTarget.All); // ��Ƽ�÷��̿����� RPC ȣ��
        }
        else
        {
            helicopter?.AddFuel(); // �̱��÷��̿����� ���� ȣ��
        }

        Debug.Log("���ᰡ �߰��Ǿ����ϴ�.");
    }
}
