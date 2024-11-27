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
            // ������ Ŭ���̾�Ʈ�� �� ���� ����
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
            // �ٸ� Ŭ���̾�Ʈ���� ����ȭ
            if (!PhotonNetwork.IsMasterClient)
            {
                transform.position = position;
                transform.localRotation = quaternion;
            }
        }
    }
}
