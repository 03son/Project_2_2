using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player_RoomInfo;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class Player : MonoBehaviour
{
    HashTable playerCP;

    PhotonView pv;

    GameObject mainCam;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)//�̱� �÷���
        {
            FindCam();
            return;
        }

        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
        pv = GetComponent<PhotonView>();
        if (pv.IsMine) //��Ƽ �÷���
        {
            FindCam();

            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("animalName"))//ĳ���� �Ҵ�
            {
                PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("animalName", out object _animalName);
                Debug.Log((string)_animalName);
            }

        }
    }
    void Start()
    {
        if (PhotonNetwork.IsConnected)//��Ƽ�� ����
        {
            notMine();
        }
     
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FindCam()
    {
        GameObject.Find("Main Camera").gameObject.transform.SetParent(this.transform);
        GameObject.Find("Follow Cam").gameObject.transform.SetParent(this.transform);
        GameObject.Find("EquipCamera").gameObject.transform.SetParent(this.transform);
    }

    void notMine() //�ڱ� �ڽ��� �ƴϸ� ��ũ��Ʈ�� �κ��丮 UI ��Ȱ��ȭ
    {
        if (!pv.IsMine)
        {
            GetComponent<Inventory>().enabled = false;
            GetComponent<InteractionManager>().enabled = false;
            GetComponent<Player_Equip>().enabled = false;
            GetComponent<PlayerMove>().enabled = false;
        }
    }
}
