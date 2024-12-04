using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerState;

public class PhotonItem : MonoBehaviourPun
{
    Transform dropPos;
    Vector3 v_dropPos;
    Transform t_dropPos;

    PhotonView pv;

    PlayerState playerState;
    PlayerState.playerState state;

    public GameObject equipItem;

    [Header("3??? ?????? ???")]
    public Transform thirdPersonHand; //3??? ?????? ???
    Transform __thirdPersonHand;
    public GameObject itemForOthers;
    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Destroy(this.gameObject);
        }
        pv = GetComponent<PhotonView>();
        playerState = GetComponent<PlayerState>();

        if (pv.IsMine)
        {
            dropPos = GameObject.Find("ItemDropPos").GetComponent<Transform>();
            equipItem = GameObject.Find("handitemattach").gameObject;
        }
    }
    public void ThrowItem(itemData item)
    {
        if (pv.IsMine)
        {
            string itemName = item.dropPerfab.name;
            v_dropPos = dropPos.position;
            pv.RPC("PhotonThrowItem_", RpcTarget.MasterClient, itemName, v_dropPos);
        }
    }
    [PunRPC]
    public void PhotonThrowItem_(string itemName, Vector3 pos)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject($"Prefabs/Items/{itemName}", pos, Quaternion.identity);
        }
    }

    public void RemoveEquippedItem(string itemName)
    {
        Debug.Log($"RemoveEquippedItem ????. ????????? ??????: {itemName}");

        if (equipItem != null)
        {
            Debug.Log("equipItem?? null?? ?????. ??? ??ии??? ???????? ???????.");

            // equipItem ???? ??? ??ии??? ItemObject?? ???
            ItemObject[] itemObjects = equipItem.GetComponentsInChildren<ItemObject>();
            Debug.Log($"????? ItemObject ????: {itemObjects.Length}");

            foreach (ItemObject itemObject in itemObjects)
            {
                Debug.Log($"????? ??????: {itemObject.item.ItemName}");
                Debug.Log($"?? ??: {itemObject.item.ItemName} == {itemName}");

                if (PhotonNetwork.IsConnected)
                {
                    if (PhotonNetwork.IsMasterClient || pv.IsMine)
                    {
                        pv.RPC("ThirdPersonPhotonDestroyItem", RpcTarget.All, itemName, PhotonNetwork.LocalPlayer.ActorNumber);
                    }
                }
                else
                {
                    // ?????? ?????? ????
                    Destroy(itemObject.gameObject);
                }
            }

            Debug.LogWarning($"equipItem?? ??? ??ии??? {itemName} ????? ???? ???????? ??? ?? ???????.");
        }
        else
        {
            Debug.LogWarning("equipItem?? null????. ?????? ???????? ???????.");
        }
    }
    [PunRPC]
    public void ThirdPersonPhotonDestroyItem(string itemName, int actorNumber)
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().Owner.ActorNumber == actorNumber)
            {
                PhotonNetwork.Destroy(player.GetComponent<PhotonItem>().itemForOthers);
            }
        }
    }

    public void setPhotonEquipItem(string item)
    {
        if (PhotonNetwork.IsConnected)
        {
            // 3??? ???????? ?????? ????
            if (pv.IsMine)
            {
                if (PhotonNetwork.IsMasterClient || pv.IsMine)
                {
                    pv.RPC("ThirdPersonHandItem", RpcTarget.All, item, PhotonNetwork.LocalPlayer.ActorNumber);
                }
            }
        }
    }
    [PunRPC]
    public void ThirdPersonHandItem(string itemName, int actorNumber)
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().Owner.ActorNumber == actorNumber)
            {
                if (itemForOthers != null)
                {
                    PhotonNetwork.Destroy(itemForOthers);
                }

                __thirdPersonHand = player.GetComponent<PhotonItem>().thirdPersonHand;
                if (__thirdPersonHand != null)
                {
                    itemForOthers = PhotonNetwork.Instantiate($"Prefabs/Items/{itemName}", __thirdPersonHand.position, Quaternion.identity);

                    if (pv.IsMine)
                    {
                        itemForOthers.layer = LayerMask.NameToLayer("LocalPlayerBody");
                    }
                    else
                    {
                        itemForOthers.layer = LayerMask.NameToLayer("Default");
                    }
                    itemForOthers.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

                    // 3??? ?????? ??? ????? ????
                    itemForOthers.transform.SetParent(__thirdPersonHand);
                    itemForOthers.transform.localPosition = Vector3.zero;
                    itemForOthers.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    Debug.Log("thirdPersonHand = null");
                }
            }
        }
    }

}
