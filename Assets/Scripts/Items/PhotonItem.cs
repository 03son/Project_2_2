using Photon.Pun;
using System;
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

    [Header("3인칭 아이템 위치")]
    public Transform thirdPersonHand; //3인칭 아이템 위치
    Transform __thirdPersonHand;
    public GameObject itemForOthers;
    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            GetComponent<PhotonItem>().enabled = false;
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
        GameObject item = PhotonNetwork.InstantiateRoomObject($"Prefabs/Items/{itemName}", pos, Quaternion.identity);
        item.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void DieAllThrowItem(itemData item , float Index)//죽을 때 전부 드랍
    {
        if (pv.IsMine)
        {
            string itemName = item.dropPerfab.name;
            v_dropPos = new Vector3(dropPos.position.x, dropPos.position.y + Index, dropPos.position.z);
            pv.RPC("DieAllPhotonThrowItem_", RpcTarget.MasterClient, itemName, v_dropPos);
        }
    }
    [PunRPC]
    public void DieAllPhotonThrowItem_(string itemName, Vector3 pos)
    {
        GameObject item = PhotonNetwork.InstantiateRoomObject($"Prefabs/Items/{itemName}", pos, Quaternion.identity);
        item.GetComponent<Rigidbody>().isKinematic = false;
    }


    public void RemoveEquippedItem(string itemName)
    {
        Debug.Log($"RemoveEquippedItem 호출됨. 제거하려는 아이템: {itemName}");

        if (equipItem != null)
        {
            Debug.Log("equipItem이 null이 아닙니다. 모든 자식에서 아이템을 검색합니다.");

            // equipItem 하위 모든 자식에서 ItemObject를 검색
            ItemObject[] itemObjects = equipItem.GetComponentsInChildren<ItemObject>();
            Debug.Log($"검색된 ItemObject 개수: {itemObjects.Length}");

            foreach (ItemObject itemObject in itemObjects)
            {
                Debug.Log($"탐색된 아이템: {itemObject.item.ItemName}");
                Debug.Log($"비교 중: {itemObject.item.ItemName} == {itemName}");

                if (PhotonNetwork.IsConnected)
                {
                    if (PhotonNetwork.IsMasterClient || pv.IsMine)
                    {
                        pv.RPC("ThirdPersonPhotonDestroyItem", RpcTarget.All, itemName, PhotonNetwork.LocalPlayer.ActorNumber);
                    }
                }
                else
                {
                    // 장착된 아이템 제거
                    Destroy(itemObject.gameObject);
                }
            }

            Debug.LogWarning($"equipItem의 모든 자식에서 {itemName} 이름을 가진 아이템을 찾을 수 없습니다.");
        }
        else
        {
            Debug.LogWarning("equipItem이 null입니다. 장착된 아이템이 없습니다.");
        }
    }
    [PunRPC]
    public void ThirdPersonPhotonDestroyItem(string itemName, int actorNumber)
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().Owner.ActorNumber == actorNumber)
            {
               // PhotonNetwork.Destroy(player.GetComponent<PhotonItem>().itemForOthers);
                Destroy(player.GetComponent<PhotonItem>().itemForOthers);
            }
        }
    }

    public void setPhotonEquipItem(string item)
    {
        if (PhotonNetwork.IsConnected)
        {
            // 3인칭 모델링에도 아이템 장착
            if (pv.IsMine)
            {
                pv.RPC("ThirdPersonHandItem", RpcTarget.Others, item, PhotonNetwork.LocalPlayer.ActorNumber);
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
                   // PhotonNetwork.Destroy(itemForOthers);
                    Destroy(itemForOthers);
                }

                __thirdPersonHand = player.GetComponent<PhotonItem>().thirdPersonHand;
                if (__thirdPersonHand != null)
                {
                    GameObject item = Resources.Load<GameObject>($"Prefabs/Items/{itemName}");
                    itemForOthers = Instantiate(item, __thirdPersonHand.position, Quaternion.identity);
                    //itemForOthers = PhotonNetwork.Instantiate($"Prefabs/Items/{itemName}", __thirdPersonHand.position, Quaternion.identity);

                    if (pv.IsMine)
                    {
                        itemForOthers.layer = LayerMask.NameToLayer("PlayerBody");
                    }
                    else
                    {
                        itemForOthers.layer = LayerMask.NameToLayer("Default");
                    }
                    itemForOthers.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);

                    // 3인칭 모델링의 왼손 위치에 장착
                    itemForOthers.transform.SetParent(__thirdPersonHand);
                    itemForOthers.transform.localPosition = new Vector3(-0.0045f,-0.0052f, -0.0127f );
                    itemForOthers.transform.localRotation = new Quaternion(-200f,20,0,0);
                    itemForOthers.GetComponent<Rigidbody>().isKinematic = true;
                }
                else
                {
                    Debug.Log("thirdPersonHand = null");
                }
                return;
            }
        }
    }

}
