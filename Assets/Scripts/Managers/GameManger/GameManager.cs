using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected virtual void Awake()
    {
        if (PhotonNetwork.IsConnected)//멀티 플레이
        {
            GetComponent<Multi_GameManager>().enabled = true;
            GetComponent<Single_GameManager>().enabled = false;
        }
        else //싱글 플레이
        {
            GetComponent<Single_GameManager>().enabled = true;
            GetComponent<Multi_GameManager>().enabled = false;
        }
    }
    protected virtual void Start()
    { 
    
    }
    protected virtual void CreatePlayer()
    {
        //플레이어 생성
    }
}
