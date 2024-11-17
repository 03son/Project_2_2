using Photon.Pun;
using UnityEngine;

public class AttachPoint : MonoBehaviourPun, IInteractable
{
    public string requiredItemName; // �ʿ��� ������ �̸� (��: "Propeller")
    public GameObject attachedItemPrefab; // ������ �������� ������
    private bool isAttached = false; // �̹� �����Ǿ����� ����

    // IInteractable �������̽��� ��ȣ�ۿ� ������Ʈ ��ȯ �޼���
    public string GetInteractPrompt()
    {
        if (isAttached)
            return ""; // �̹� �����Ǿ����� �� ���ڿ� ��ȯ
        else
            return $"{requiredItemName} �����ϱ�"; // ���� �������� �ʾ����� ������Ʈ ǥ��
    }

    public void OnInteract()
    {
        if (isAttached)
        {
            Debug.Log("�̹� ������");
            return;
        }

        Inventory inventory = Inventory.instance;

        if (inventory.HasItem(requiredItemName))
        {
            Debug.Log($"{requiredItemName} �������� �κ��丮�� ����. ���� ����");

            // ������ ����
            inventory.RemoveItem(requiredItemName);

            // ���� ���� ����ȭ (RPC ȣ��)
            photonView.RPC("RPC_AttachItem", RpcTarget.All);
            Debug.Log("RPC_AttachItem ȣ���"); // ȣ�� Ȯ�ο� �α�
        }
        else
        {
            Debug.Log($"{requiredItemName}��(��) �ʿ��մϴ�.");
        }
    }

    [PunRPC]
    void RPC_AttachItem()
    {
        Debug.Log($"{requiredItemName} ���� �Ϸ�");

        Instantiate(attachedItemPrefab, transform.position, transform.rotation, transform.parent);
        isAttached = true;

        SubmarineController submarine = GetComponentInParent<SubmarineController>();
        if (submarine != null)
        {
            submarine.AttachItem(requiredItemName);
            Debug.Log("����Կ� ������ ������ ���� ���� �Ϸ�");
        }
        else
        {
            Debug.LogWarning("����� ��Ʈ�ѷ��� �����ϴ�.");
        }
    }
}
