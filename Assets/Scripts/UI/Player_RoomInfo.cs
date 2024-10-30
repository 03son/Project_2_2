using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Photon.Pun;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class Player_RoomInfo : UI_Popup
{
    HashTable playerCP;

    public string Ready = "IsReady";

    public bool isReady = false;

   string AnimalName = "�����̸�";

   Button ReadyButton;

    public int Actor_num;

    public override void Init()
    {
        ReadyButton.gameObject.AddUIEvent(OnCickReadyButton);
    }

    public void UpdatePlayerInfo(string NickName ,int ActorNumber) //�÷��̾� ���� ���� ������Ʈ
    {
        setPlayerNickNameText(NickName);
        setAnimalNameText();

        Actor_num = ActorNumber;
    }
    void setPlayerNickNameText(string NickName)
    {
         transform.Find("PlayerNickName").gameObject.GetComponent<TextMeshProUGUI>().text = NickName;   
    }
   
    void setAnimalNameText()
    {
        transform.Find("AnimalName").gameObject.GetComponent<TextMeshProUGUI>().text
            = AnimalName;
    }
    void OnCickReadyButton(PointerEventData button)
    {
        if (Actor_num == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            ToggleReady();
        }
    }

     void ToggleReady()
    {
        isReady = !isReady;
        UpdatePlayerReayState(isReady);
        UpdateReadyUI();
    }

    void UpdatePlayerReayState(bool ready)
    {
        playerCP = new HashTable() { {"isReady",ready } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);
    }
   public void UpdateReadyUI()
    {
        Image ReadyButtonCB = ReadyButton.GetComponent<Image>();

        if (isReady)
        {
            //�ʷϻ�
            ReadyButtonCB.color = new Color(0,255,0,255);
        }
        else
        {
            //������
            ReadyButtonCB.color = new Color(255, 0, 0, 255);
        }
        ReadyButton.GetComponent<Image>().color = ReadyButtonCB.color;
    }
    void Start()
    {
        ReadyButton = transform.Find("ReadyButton").GetComponent<Button>();

        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;

        //�غ� ��ư �ʱ�ȭ
        Image ReadyButtonCB = ReadyButton.GetComponent<Image>();
        ReadyButtonCB.color = new Color(255, 0, 0, 255);
        ReadyButton.GetComponent<Image>().color = ReadyButtonCB.color;

        Init();
    }
}
