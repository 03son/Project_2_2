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

public class Room : UI_Popup
{
    public GameObject PlayerSlot;

    enum Buttons
    { 
        ExitButton
    }
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.ExitButton).gameObject.AddUIEvent(ExitRoomButton);
    }
    public void JoinPlayer()
    {
        if (PhotonNetwork.IsMasterClient)//마스터클라이언트가 방을 만들면서 입장
        {
            Debug.Log("방장 플레이어가 입장 하였습니다.");
            Debug.Log($"플레이어 이름 : {PhotonNetwork.LocalPlayer.NickName},ActorNumber : {PhotonNetwork.MasterClient.ActorNumber}");

            PlayerSlot.GetComponent<PlayerSlot>().JoinedRoom();
        }        
    }
  
    void Start()
    {
        Init();

        JoinPlayer();
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
        //룸에서 나가기
        PhotonManager.instance.leaveRoom();

        //로비 화면 닫기
        ClosePopupUI();
    }

}
