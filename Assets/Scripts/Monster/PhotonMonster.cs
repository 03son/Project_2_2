using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice;
using Photon.Realtime;
using UnityEngine.UIElements;

public class PhotonMonster : MonoBehaviourPun
{
    //����ȭ �۾� ��
    /*
    MonsterAI MonsterAI;

    private void Start()
    {
        MonsterAI = gameObject.GetComponent<MonsterAI>();
    }
    void Update()
    {
        // Master Client�� ��ġ�� ȸ�� �����͸� ����
        if (photonView.IsMine)
        {
            SyncMonsterTransform();
        }
    }
    IEnumerator SyncMonsterTransform()
    {
        while (true)
        {
            photonView.RPC("SyncMonster", RpcTarget.Others, transform.position, transform.rotation);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    [PunRPC]
    public void SyncMonster(Vector3 newPosition, Quaternion newRotation)
    {
        // �ε巴�� ��ġ�� ����
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 5f);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 5f);
    }
    */
}
