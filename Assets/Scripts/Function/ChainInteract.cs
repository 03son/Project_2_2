using UnityEngine;
using Photon.Pun;

public class ChainInteract : MonoBehaviourPun, IInteractable
{
    public string requiredItem = "Cutter"; // �ʿ��� ������ �̸�
    public float holdTime = 10f;           // Ȧ�� �ð�
    private bool isChainRemoved = false;   // �罽 ���� ����
    private float holdProgress = 0f;       // Ȧ�� ���� �ð�
    private bool isHolding = false;        // Ȧ�� ������ ����

    public string GetInteractPrompt()
    {
        if (isChainRemoved)
            return ""; // �罽�� �̹� ���ŵǾ����� �ؽ�Ʈ ����
        return $"{requiredItem}�� �罽 �����ϱ� ({holdProgress:F1}/{holdTime:F1}��)";
    }

    public void OnInteract()
    {
        if (isChainRemoved) return;

        Inventory inventory = Inventory.instance;
        if (!inventory.HasItem(requiredItem))
        {
            Debug.Log($"{requiredItem}��(��) �ʿ��մϴ�.");
            return;
        }

        isHolding = true; // Ȧ�� ����
    }

    private void Update()
    {
        if (isHolding && !isChainRemoved)
        {
            if (Input.GetKey(KeyCode.F)) // F Ű�� �� ������ ��
            {
                holdProgress += Time.deltaTime;
                if (holdProgress >= holdTime)
                {
                    CompleteInteract(); // �۾� �Ϸ�
                }
            }
            else
            {
                CancelInteract(); // Ű�� ���� ��� ���� ���
            }

            // ������Ʈ ������Ʈ�� InteractionManager�� �ǽð����� ȣ���ϵ��� ����
            InteractionManager.UpdatePrompt(this);
        }
    }

    private void CompleteInteract()
    {
        isHolding = false;
        holdProgress = 0f;

        Inventory inventory = Inventory.instance;
        inventory.RemoveItem(requiredItem); // ������ ���

        if (PhotonNetwork.IsConnected)
        {
            // ��Ƽ�÷��̿����� RPC ȣ��
            photonView.RPC("RPC_RemoveChain", RpcTarget.All);
        }
        else
        {
            // �̱��÷��̿����� ���� �޼��� ȣ��
            RPC_RemoveChain();
        }
    }

    private void CancelInteract()
    {
        isHolding = false;
        holdProgress = 0f;
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
