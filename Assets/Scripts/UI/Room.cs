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
        if (PhotonNetwork.IsMasterClient)//������Ŭ���̾�Ʈ�� ���� ����鼭 ����
        {
            Debug.Log("���� �÷��̾ ���� �Ͽ����ϴ�.");
            Debug.Log($"�÷��̾� �̸� : {PhotonNetwork.LocalPlayer.NickName},ActorNumber : {PhotonNetwork.MasterClient.ActorNumber}");

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
        //�뿡�� ������
        PhotonManager.instance.leaveRoom();

        //�κ� ȭ�� �ݱ�
        ClosePopupUI();
    }

}
