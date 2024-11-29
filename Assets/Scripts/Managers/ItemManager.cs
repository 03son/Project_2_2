using System.Collections.Generic; // Dictionary ����� ���� �ʿ�
using UnityEngine; // Unity �⺻ API
using Photon.Pun; // Photon ��Ʈ��ũ ����ȭ�� ���� �ʿ�

public class ItemManager : MonoBehaviourPun // Photon ����ȭ�� ���� photonView ���
{
    // �̱��� �������� ���� ������ �� �ֵ��� ����
    public static ItemManager Instance;

    // ������ ���¸� ������ Dictionary (�̸�, ȹ�� ����)
    private Dictionary<string, bool> itemStates = new Dictionary<string, bool>();

    private void Awake()
    {
        // �̱��� �ʱ�ȭ
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �ߺ��� ItemManager�� ������ ����
        }
    }

    // ������ ���¸� ���� (ȹ�� ó��)
    public void SetItemState(string itemName, bool isPickedUp)
    {
        if (!itemStates.ContainsKey(itemName))
        {
            itemStates.Add(itemName, isPickedUp); // ���ο� �������̸� �߰�
        }
        else
        {
            itemStates[itemName] = isPickedUp; // ���� �������̸� ���� ������Ʈ
        }

        // ��� Ŭ���̾�Ʈ�� ���� ����ȭ
        photonView.RPC("SyncItemState", RpcTarget.AllBuffered, itemName, isPickedUp);
    }

    // ������ ���� ����ȭ (RPC�� ȣ��)
    [PunRPC]
    private void SyncItemState(string itemName, bool isPickedUp)
    {
        if (!itemStates.ContainsKey(itemName))
        {
            itemStates.Add(itemName, isPickedUp);
        }
        else
        {
            itemStates[itemName] = isPickedUp;
        }

        // �ش� ������ ��Ȱ��ȭ ó��
        GameObject item = GameObject.Find(itemName);
        if (item != null)
        {
            item.SetActive(!isPickedUp); // ȹ��� �������̸� ��Ȱ��ȭ
        }
    }

    // ������ ���� ��������
    public bool GetItemState(string itemName)
    {
        return itemStates.ContainsKey(itemName) && itemStates[itemName];
    }

    [PunRPC] // RPC ��Ʈ����Ʈ �߰�
    public void RPC_HandleItemPickup()
    {
        // ������ ��Ȱ��ȭ ó��
        gameObject.SetActive(false);
    }
}
