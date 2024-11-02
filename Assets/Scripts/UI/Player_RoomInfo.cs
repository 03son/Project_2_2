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
   public Image Animal_Image;

    HashTable playerCP;

    public string Ready = "IsReady";

    public bool isReady = false;

   string AnimalName = "동물이름";

    Button ReadyButton;

    public int Actor_num;

    public RoomManager roomManager;

    public Button ChangeCharacterButton_L;
    public Button ChangeCharacterButton_R;

    [SerializeField] Sprite[] CharacterImage = new Sprite[5];

    int characterNumber;

    public enum _AniMalName
    {
        무작위,
        늑대,
        토끼,
        라쿤,
        고양이
    }
    void Start()
    {
        CharacterImage = GetComponentInParent<Transform>().GetComponentInParent<RoomManager>().CharacterImage;

        ReadyButton = transform.Find("ReadyButton").GetComponent<Button>();
        Animal_Image = transform.Find("Player_Image").GetComponent<Image>();

        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;

        //준비 버튼 초기화
        Image ReadyButtonCB = ReadyButton.GetComponent<Image>();
        ReadyButtonCB.color = new Color(255, 0, 0, 255);
        ReadyButton.GetComponent<Image>().color = ReadyButtonCB.color;

        Init();
    }

    public override void Init()
    {
        ReadyButton.gameObject.AddUIEvent(OnCickReadyButton);

        if (ChangeCharacterButton_L && ChangeCharacterButton_R)
        {
            ChangeCharacterButton_L.gameObject.AddUIEvent(ChangeCharacter_L);
            ChangeCharacterButton_R.gameObject.AddUIEvent(ChangeCharacter_R);
        }
    }

    public void UpdatePlayerInfo(string NickName ,int ActorNumber , string AnimalName) //플레이어 슬롯 정보 업데이트
    {
        setPlayerNickNameText(NickName);
        setAnimalNameText(AnimalName);

        Actor_num = ActorNumber;
    }
    void setPlayerNickNameText(string NickName)
    {
         transform.Find("PlayerNickName").gameObject.GetComponent<TextMeshProUGUI>().text = NickName;   
    }
   
    public void setAnimalNameText(string AnimalName)
    {
        if (AnimalName == "")
        {
            transform.Find("AnimalName").gameObject.GetComponent<TextMeshProUGUI>().text
            = AnimalName;
            return;
        }

        if (Enum.TryParse(AnimalName, out _AniMalName _AnimalName))
        {
            Animal_Image.sprite = CharacterImage[(int)_AnimalName];

            characterNumber = (int)_AnimalName;

            transform.Find("AnimalName").gameObject.GetComponent<TextMeshProUGUI>().text
             = AnimalName;
        }

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
            switch (characterNumber)
            {
                case 0:
                    characterNumber = 4;
                    break;
                case 1:
                    characterNumber = 0;
                    break;
                case 2:
                    characterNumber = 1;
                    break;
                case 3:
                    characterNumber = 2;
                    break;
                case 4:
                    characterNumber = 3;
                    break;

            }
            _AniMalName Name = (_AniMalName)characterNumber;
            setAnimalNameText(Name.ToString());
            UpdateCharacterHashTable(Name.ToString());
        }
    }   
    
    void ChangeCharacter_R(PointerEventData button)
    {
        if (Actor_num == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            switch (characterNumber)
            {
                case 0:
                    characterNumber = 1;
                    break;
                case 1:
                    characterNumber = 2;
                    break;
                case 2:
                    characterNumber = 3;
                    break;
                case 3:
                    characterNumber = 4;
                    break;
                case 4:
                    characterNumber = 0;
                    break;

            }
            _AniMalName Name = (_AniMalName)characterNumber;
            setAnimalNameText(Name.ToString());
            UpdateCharacterHashTable(Name.ToString());
        }
    }

    void UpdateCharacterHashTable(string AnimalName)//커스텀 프로퍼티에 자신 캐릭터 이름 등록
    {
        playerCP = new HashTable() { { "animalName", AnimalName } };
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
            //초록색
            ReadyButtonCB.color = new Color(0,255,0,255);
        }
        else
        {
            //빨간색
            ReadyButtonCB.color = new Color(255, 0, 0, 255);
        }
        ReadyButton.GetComponent<Image>().color = ReadyButtonCB.color;
    }

     

}
