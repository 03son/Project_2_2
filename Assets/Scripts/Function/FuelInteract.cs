using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.UI; // UI ���� ���ӽ����̽� �߰�
using UnityEngine.EventSystems; // Layout ���� ���ӽ����̽� �߰�

public class FuelInteract : MonoBehaviourPun, IInteractable
{
    public string requiredItem = "��� ������"; // �ʿ��� ������ �̸�
    public float holdTime = 10f;         // Ȧ�� �ð�
    private bool isFuelAdded = false;    // ���� �߰� ����
    private float holdProgress = 0f;     // Ȧ�� ���� �ð�
    private bool isHolding = false;      // Ȧ�� ������ ����

    [Header("Audio Settings")]
    public AudioSource audioSource;        // AudioSource ������Ʈ
    public AudioClip fuelAddingSound;      // ���� ���� �� ����
    public AudioClip fuelAddedCompleteSound; // ���� ���� �Ϸ� ����

    [Header("UI Settings")]
    public Image holdTimeBar; // ���� ��Ȳ�� ǥ���� Ÿ�ӹ� UI �̹���

    public string GetInteractPrompt()
    {
        // ���ᰡ �̹� �߰��� ���¸� �ؽ�Ʈ�� ǥ������ ����
        return isFuelAdded ? "" : $"{requiredItem}�� ���� �����ϱ� ({holdProgress:F1}/{holdTime:F1}��)";
    }

    public void OnInteract()
    {
        if (isFuelAdded) return; // �̹� ���ᰡ �߰��� ���¸� �������� ����

        // Player_Equip���� ���� ������ �������� �������� Ȯ��
        Player_Equip playerEquip = FindObjectOfType<Player_Equip>();
        if (playerEquip == null || !playerEquip.HasEquippedItem(requiredItem))
        {
            Debug.Log($"{requiredItem}��(��) �ʿ��մϴ�.");
            return;
        }

        isHolding = true; // Ȧ�� ����



        // Ÿ�ӹ� UI Ȱ��ȭ
        if (holdTimeBar != null)
        {
            holdTimeBar.fillAmount = 0f; // �ʱ�ȭ
            holdTimeBar.gameObject.SetActive(true);
            Debug.Log("FuelInteract - HoldTimeBar Initialized");
        }
        else
        {
            Debug.LogError("FuelInteract - HoldTimeBar is null!");
        }

        // ���� ���� ���� ����
        if (fuelAddingSound != null && audioSource != null)
        {
            audioSource.clip = fuelAddingSound;
            audioSource.loop = true; // �ݺ� ���
            audioSource.Play();
        }
    }

    private void LateUpdate()
    {
        if (isHolding && !isFuelAdded)
        {
            if (Input.GetKey(KeyCode.F))
            {
                holdProgress += Time.deltaTime;

                if (holdTimeBar != null)
                {
                    holdTimeBar.fillAmount = holdProgress / holdTime;
                    Debug.Log($"FuelInteract (LateUpdate) - FillAmount: {holdTimeBar.fillAmount}");
                }

                if (holdProgress >= holdTime)
                {
                    CompleteInteract();
                }
            }
            else
            {
                CancelInteract();
            }
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

        // Ÿ�ӹ� UI ��Ȱ��ȭ
        if (holdTimeBar != null)
        {
            holdTimeBar.gameObject.SetActive(false);
        }

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

        // Ÿ�ӹ� UI ��Ȱ��ȭ
        if (holdTimeBar != null)
        {
            holdTimeBar.gameObject.SetActive(false);
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
