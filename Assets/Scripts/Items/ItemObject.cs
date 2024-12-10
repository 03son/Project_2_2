using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public itemData item;

    private GlassCupThrower glassCupThrower;

    PhotonView PhotonView;


    void Start()
    {
        glassCupThrower = FindObjectOfType<GlassCupThrower>();
        if (glassCupThrower == null)
        {
        //   Debug.LogError("GlassCupThrower ������Ʈ�� ã�� �� �����ϴ�!");
        }
        if (PhotonNetwork.IsConnected)
        {
            PhotonView = GetComponent<PhotonView>();
        }
    }
    public string GetInteractPrompt()
    {
        return string.Format("�ݱ� {0}", item.ItemName);
    }

    public void OnInteract()
    {
        // ���Կ� �� ������ �ִ��� Ȯ�� �� ������ �߰�
        if (addSlot())
        {
            Inventory.instance.Additem(item);
            Debug.Log($"������ ȹ��: {item.ItemName}");

            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
                else
                {
                    PhotonView.RPC("PhotonDestroyItem", RpcTarget.Others);
                }
            }
            else
            {
                // �������� ȹ���� �� ����
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogWarning("�κ��丮�� �� ������ �����ϴ�. �������� ȹ���� �� �����ϴ�.");
        }
    }
    [PunRPC]
    public void PhotonDestroyItem()
    {
        if (PhotonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
    bool addSlot()
    {
        // �κ��丮�� �� ������ �ִ��� Ȯ��
        for (int i = 0; i < Inventory.instance.slots.Length; i++)
        {
            if (Inventory.instance.slots[i].item == null)
                return true;
        }
        return false;
    }
}
