using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using static UI_Button;
using UnityEngine.EventSystems;

public class Room : UI_Popup
{
    public Button exitButton;

    void Start()
    {
        exitButton.gameObject.AddUIEvent(ExitRoomButton);    
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

        //멀티 리스트 화면 열기
       // UIManger.Instance.ShowPopupUI<MultiPlayList>();
    }

}
