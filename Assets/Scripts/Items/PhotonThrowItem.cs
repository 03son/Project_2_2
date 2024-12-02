using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerState;

public class PhotonThrowItem : MonoBehaviour
{
    Transform dropPos;
    Vector3 v_dropPos;
    Transform t_dropPos;

    PhotonView pv;

    PlayerState playerState;
    PlayerState.playerState state;
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
        }
    }
    public void ThrowItem(itemData item)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject($"Prefabs/Items/{item.dropPerfab.name}", dropPos.position, Quaternion.identity);
        }
        else
        {
            if (pv.IsMine)
            {
                string itemName = item.dropPerfab.name;
                v_dropPos = dropPos.position;
                pv.RPC("PhotonThrowItem_", RpcTarget.MasterClient, itemName , v_dropPos);
            }
        }
    }
    [PunRPC]
    public void PhotonThrowItem_(string itemName , Vector3 pos)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject($"Prefabs/Items/{itemName}", pos, Quaternion.identity);
        }
    }
}
