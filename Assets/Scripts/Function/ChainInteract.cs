using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class ChainInteract : MonoBehaviourPun, IInteractable
{
    public string requiredItem = "���ܱ�"; // �ʿ��� ������ �̸�
    public float holdTime = 10f;           // Ȧ�� �ð�
    private bool isChainRemoved = false;   // �罽 ���� ����
    private float holdProgress = 0f;       // Ȧ�� ���� �ð�
    private bool isHolding = false;        // Ȧ�� ������ ����

    [Header("UI Settings")]
    public Image holdTimeBar;              // UI Ÿ�ӹ� �̹��� �߰� (���׶��� ǥ�õǴ� Ÿ�ӹ�)

    [Header("Audio Settings")]
    public AudioSource audioSource;        // AudioSource ������Ʈ
    public AudioClip cuttingSound;         // �罽 ���� �� ����
    public AudioClip cutCompleteSound;     // ���� �Ϸ� ����

    public string GetInteractPrompt()
    {
        if (isChainRemoved)
            return ""; // �罽�� �̹� ���ŵǾ����� �ؽ�Ʈ ����
        return $"{requiredItem}�� �罽 �����ϱ� ({holdProgress:F1}/{holdTime:F1}��)";
    }

    public void OnInteract()
    {
        if (isChainRemoved) return;

        Player_Equip playerEquip = FindObjectOfType<Player_Equip>();
        if (playerEquip == null || !playerEquip.HasEquippedItem(requiredItem))
        {
            Debug.Log($"{requiredItem}��(��) �ʿ��մϴ�.");
            return;
        }

        isHolding = true; // Ȧ�� ����

        // Ÿ�ӹ� �ʱ�ȭ �� Ȱ��ȭ
        if (holdTimeBar != null)
        {
            holdTimeBar.fillAmount = 0f; // Ÿ�ӹٸ� �ʱ� ���·� ����
            holdTimeBar.gameObject.SetActive(true); // Ÿ�ӹ� Ȱ��ȭ
        }

        // ���� �� ���� ����
        if (cuttingSound != null && audioSource != null)
        {
            audioSource.clip = cuttingSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (isHolding && !isChainRemoved)
        {
            if (Input.GetKey(KeyCode.F)) // F Ű�� �� ������ ��
            {
                holdProgress += Time.deltaTime;

                // Ÿ�ӹ� ������Ʈ
                if (holdTimeBar != null)
                {
                    holdTimeBar.fillAmount = holdProgress / holdTime; // ���� �ð��� ���� Ÿ�ӹ� ä���
                }

                if (holdProgress >= holdTime)
                {
                    CompleteInteract(); // �۾� �Ϸ�
                }
            }
            else
            {
                CancelInteract(); // Ű�� ���� ��� ���� ���
            }
        }
    }

    private void CompleteInteract()
    {
        isHolding = false;
        holdProgress = 0f;

        // Ÿ�ӹ� ��Ȱ��ȭ
        if (holdTimeBar != null)
        {
            holdTimeBar.gameObject.SetActive(false);
        }

        // ���� �Ϸ� ����
        if (audioSource != null)
        {
            audioSource.Stop(); // ���� �� �Ҹ� ����
            if (cutCompleteSound != null)
            {
                audioSource.PlayOneShot(cutCompleteSound); // �Ϸ� ���� ���
            }
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                PhotonItem photonItem = player.GetComponent<PhotonItem>();
                if (photonItem != null)
                {
                    photonItem.RemoveEquippedItem(requiredItem);
                    Debug.Log($"{requiredItem} �������� ���ŵǾ����ϴ�.");
                }
            }
        }

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_RemoveChain", RpcTarget.All);
        }
        else
        {
            RPC_RemoveChain();
        }
    }

    private void CancelInteract()
    {
        isHolding = false;
        holdProgress = 0f;

        // Ÿ�ӹ� ��Ȱ��ȭ
        if (holdTimeBar != null)
        {
            holdTimeBar.gameObject.SetActive(false);
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        Debug.Log("�罽 ���� ��ҵ�");
    }

    [PunRPC]
    private void RPC_RemoveChain()
    {
        if (isChainRemoved) return;

        isChainRemoved = true;
        HelicopterController helicopter = GetComponentInParent<HelicopterController>();

        if (PhotonNetwork.IsConnected)
        {
            helicopter?.photonView.RPC("RemoveChain", RpcTarget.All);
        }
        else
        {
            helicopter?.RemoveChain();
        }

        Debug.Log("�罽�� ���ŵǾ����ϴ�.");
    }
}