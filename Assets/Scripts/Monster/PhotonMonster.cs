using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice;
using Photon.Realtime;

public class PhotonMonster : MonoBehaviourPun
{
    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (transform.hasChanged)
                {
                    StartCoroutine(MosterSync());
                }
            }
        }
    }

    IEnumerator MosterSync()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Monstertransform();
    }

    void Monstertransform()
    {
        photonView.RPC("SyncMonsterPosition", RpcTarget.Others, transform.position, transform.localRotation);
    }

    [PunRPC]
    public void SyncMonsterPosition(Vector3 position ,Quaternion quaternion)
    {
        if (PhotonNetwork.IsConnected)
        {
            // 부드럽게 위치를 보간
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, Time.deltaTime * 10f);
        }
    }
}
