using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public GameObject IngameUI;

    PhotonView pv;
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            GameObject.Find("Main Camera").gameObject.transform.SetParent(this.transform);
            GameObject.Find("Follow Cam").gameObject.transform.SetParent(this.transform);
            GameObject.Find("EquipCamera").gameObject.transform.SetParent(this.transform);
        }
    }
    void Start()
    {
        notMine();
    }

    // Update is called once per frame
    void Update()
    {

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
