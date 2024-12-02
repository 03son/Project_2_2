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

    // 아이템 상태를 설정하고 동기화하는 메서드
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

        // 모든 클라이언트에 상태 동기화
        photonView.RPC("SyncItemState", RpcTarget.AllBuffered, itemName, isPickedUp, playerPhotonView.ViewID);
    }

    // 아이템 상태 동기화 (RPC로 호출)
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

        // 아이템이 획득된 경우, 해당 아이템을 비활성화하고 플레이어에 장착
        GameObject item = GameObject.Find(itemName);
        if (item != null)
        {
            item.SetActive(!isPickedUp); // 획득된 아이템이면 비활성화

            if (isPickedUp)
            {
                // 플레이어 객체를 찾고 아이템을 손 위치에 장착
                PhotonView playerPhotonView = PhotonView.Find(playerViewID);
                if (playerPhotonView != null)
                {
                    GameObject player = playerPhotonView.gameObject;
                    // 이미 손에 장착하는 부분이 구현되어 있으므로, 그 함수를 호출
                    Player_Equip playerEquip = player.GetComponent<Player_Equip>();
                    if (playerEquip != null)
                    {
                    //    playerEquip.RPC_SetEquipItemForOthers(item.name);
                    }
                }
            }
        }
    }

    // 아이템 상태 가져오기
    public bool GetItemState(string itemName)
    {
        return itemStates.ContainsKey(itemName) && itemStates[itemName];
    }
}
