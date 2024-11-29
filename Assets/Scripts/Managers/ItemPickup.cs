using UnityEngine;
using Photon.Pun; // Photon ���� Ŭ���� ���

public class ItemPickup : MonoBehaviourPun
{
    public string itemName; // �� �������� ���� �̸�

    // ������ ��ȣ�ۿ� �޼���
    public void Interact()
    {
        // �̹� ȹ��� �������̸� ���� �� ��
        if (ItemManager.Instance.GetItemState(itemName)) return;

        // ������ ȹ�� ó��
        ItemManager.Instance.SetItemState(itemName, true);

        Debug.Log($"{itemName}��(��) ȹ���߽��ϴ�!");
    }

    [PunRPC]
    public void RPC_HandleItemPickup()
    {
        // ������ ��Ȱ��ȭ
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} �������� ��Ȱ��ȭ�Ǿ����ϴ�.");
    }
}
