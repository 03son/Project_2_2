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
        //   Debug.LogError("GlassCupThrower 컴포넌트를 찾을 수 없습니다!");
        }
        if (PhotonNetwork.IsConnected)
        {
            PhotonView = GetComponent<PhotonView>();
        }
    }
    public string GetInteractPrompt()
    {
        return string.Format("줍기 {0}", item.ItemName);
    }

    public void OnInteract()
    {
        // 슬롯에 빈 공간이 있는지 확인 후 아이템 추가
        if (addSlot())
        {
            Inventory.instance.Additem(item);
            Debug.Log($"아이템 획득: {item.ItemName}");

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
                // 아이템을 획득한 후 제거
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogWarning("인벤토리에 빈 슬롯이 없습니다. 아이템을 획득할 수 없습니다.");
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
        // 인벤토리에 빈 슬롯이 있는지 확인
        for (int i = 0; i < Inventory.instance.slots.Length; i++)
        {
            if (Inventory.instance.slots[i].item == null)
                return true;
        }
        return false;
    }
}
