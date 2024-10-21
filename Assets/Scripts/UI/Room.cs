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
        //�뿡�� ������
        PhotonManager.instance.leaveRoom();

        //�κ� ȭ�� �ݱ�
        ClosePopupUI();

        //��Ƽ ����Ʈ ȭ�� ����
       // UIManger.Instance.ShowPopupUI<MultiPlayList>();
    }

}
