using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonMonster : MonoBehaviourPun
{
    void Update()
    {
        /*
        if (PhotonNetwork.IsConnected)
        {
            // 마스터 클라이언트만 적 몬스터 제어
            if (PhotonNetwork.IsMasterClient)
            {
                Monstertransform();
            }
        }*/
        Monstertransform();
    }

    void Monstertransform()
    {
        photonView.RPC("SyncMonsterPosition", RpcTarget.All, transform.position, transform.localRotation);
    }

    [PunRPC]
    void SyncMonsterPosition(Vector3 position ,Quaternion quaternion)
    {
        if (PhotonNetwork.IsConnected)
        {
            // 다른 클라이언트에서 동기화
            if (!PhotonNetwork.IsMasterClient)
            {
                transform.position = position;
                transform.localRotation = quaternion;
            }
        }
    }
}
