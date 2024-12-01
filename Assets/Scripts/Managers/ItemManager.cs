using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemManager : MonoBehaviourPun
{
    public static ItemManager Instance;

    private Dictionary<string, bool> itemStates = new Dictionary<string, bool>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ������ ���¸� �����ϰ� ����ȭ�ϴ� �޼���
    public void SetItemState(string itemName, bool isPickedUp, PhotonView playerPhotonView)
    {
        if (!itemStates.ContainsKey(itemName))
        {
            itemStates.Add(itemName, isPickedUp);
        }
        else
        {
            itemStates[itemName] = isPickedUp;
        }

        // ��� Ŭ���̾�Ʈ�� ���� ����ȭ
        photonView.RPC("SyncItemState", RpcTarget.AllBuffered, itemName, isPickedUp, playerPhotonView.ViewID);
    }

    // ������ ���� ����ȭ (RPC�� ȣ��)
    [PunRPC]
    private void SyncItemState(string itemName, bool isPickedUp, int playerViewID)
    {
        if (!itemStates.ContainsKey(itemName))
        {
            itemStates.Add(itemName, isPickedUp);
        }
        else
        {
            itemStates[itemName] = isPickedUp;
        }

        // �������� ȹ��� ���, �ش� �������� ��Ȱ��ȭ�ϰ� �÷��̾ ����
        GameObject item = GameObject.Find(itemName);
        if (item != null)
        {
            item.SetActive(!isPickedUp); // ȹ��� �������̸� ��Ȱ��ȭ

            if (isPickedUp)
            {
                // �÷��̾� ��ü�� ã�� �������� �� ��ġ�� ����
                PhotonView playerPhotonView = PhotonView.Find(playerViewID);
                if (playerPhotonView != null)
                {
                    GameObject player = playerPhotonView.gameObject;
                    // �̹� �տ� �����ϴ� �κ��� �����Ǿ� �����Ƿ�, �� �Լ��� ȣ��
                    Player_Equip playerEquip = player.GetComponent<Player_Equip>();
                    if (playerEquip != null)
                    {
                    //    playerEquip.RPC_SetEquipItemForOthers(item.name);
                    }
                }
            }
        }
    }

    // ������ ���� ��������
    public bool GetItemState(string itemName)
    {
        return itemStates.ContainsKey(itemName) && itemStates[itemName];
    }
}
