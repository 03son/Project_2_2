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
    Image Animal_Image;

    HashTable playerCP;

    public string Ready = "IsReady";

    public bool isReady = false;

   string AnimalName = "�����̸�";

    Button ReadyButton;

    public int Actor_num;

    public RoomManager roomManager;

    public Button ChangeCharacterButton_L;
    public Button ChangeCharacterButton_R;

    public override void Init()
    {
        ReadyButton.gameObject.AddUIEvent(OnCickReadyButton);

        if (ChangeCharacterButton_L && ChangeCharacterButton_R)
        {
            ChangeCharacterButton_L.gameObject.AddUIEvent(ChangeCharacter_L);
            ChangeCharacterButton_R.gameObject.AddUIEvent(ChangeCharacter_R);
        }
    }

    public void UpdatePlayerInfo(string NickName ,int ActorNumber , string AnimalName) //�÷��̾� ���� ���� ������Ʈ
    {
        setPlayerNickNameText(NickName);
        setAnimalNameText(AnimalName);

        Actor_num = ActorNumber;
    }
    void setPlayerNickNameText(string NickName)
    {
         transform.Find("PlayerNickName").gameObject.GetComponent<TextMeshProUGUI>().text = NickName;   
    }
   
    void setAnimalNameText(string AnimalName)
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

    void ChangeCharacter_L(PointerEventData button)
    {
        if (Actor_num == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Debug.Log("����");
            //roomManager.GetComponent<RoomManager>().AniMalName.
        }
    }   
    
    void ChangeCharacter_R(PointerEventData button)
    {
        if (Actor_num == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Debug.Log("������");
        }
    }
    void UpdateCharacterHashTable()//Ŀ���� ������Ƽ�� �ڽ� ĳ���� �̸� ���
    {
        playerCP = new HashTable() { { "animalName", "�����̸�" } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);
    }
    void UpdatePlayerReayState(bool ready)
    {
        playerCP = new HashTable() { { "isReady", ready } };
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
        Animal_Image = transform.Find("Player_Image").GetComponent<Image>();

        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;

        //�غ� ��ư �ʱ�ȭ
        Image ReadyButtonCB = ReadyButton.GetComponent<Image>();
        ReadyButtonCB.color = new Color(255, 0, 0, 255);
        ReadyButton.GetComponent<Image>().color = ReadyButtonCB.color;

        Init();
    }
}
