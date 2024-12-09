using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using static UI_Button;
using UnityEngine.EventSystems;
using Photon.Pun.UtilityScripts;
using System.Runtime.InteropServices;
using ExitGames.Client.Photon;
using Photon.Pun.Demo.PunBasics;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class Room : UI_Popup
{
    HashTable playerCP;

    public GameObject BG;

    RoomManager roomSetting;

    enum Buttons
    { 
        ExitButton
    }
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.ExitButton).gameObject.AddUIEvent(ExitRoomButton);

        roomSetting = BG.gameObject.GetComponent<RoomManager>();
    }
  
    void Start()
    {
        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;

        Init();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && this.gameObject.activeSelf)
        {
            ExitRoom();
        }
    }

     void ExitRoomButton(PointerEventData button)
    {
        ExitRoom();
    }
    void ExitRoom()
    {
        GameInfo.IsGameFinish = false;

        //룸에서 나가기
        PhotonManager.instance.leaveRoom();

        gameObject.SetActive(false);

    }

}
