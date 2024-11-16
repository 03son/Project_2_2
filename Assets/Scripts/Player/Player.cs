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
        if (!PhotonNetwork.IsConnected)//싱글 플레이
        {
            FindCam();
            return;
        }

        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
        pv = GetComponent<PhotonView>();
        if (pv.IsMine) //멀티 플레이
        {
            FindCam();

            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("animalName"))//캐릭터 할당
            {
                PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("animalName", out object _animalName);
                Debug.Log((string)_animalName);
            }

        }
    }
    void Start()
    {
        if (PhotonNetwork.IsConnected)//멀티일 때만
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

    void notMine() //자기 자신이 아니면 스크립트와 인벤토리 UI 비활성화
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
