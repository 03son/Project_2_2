using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player_RoomInfo : UI_Popup
{
   string NickName = "X";
   string AnimalName = "�����̸�";

    public override void Init()
    {
        transform.Find("ReadyButton").gameObject.AddUIEvent(OnCickReadyButton);
    }

    public void UpdatePlayerInfo(string NickName) //�÷��̾� ���� ���� ������Ʈ
    {
        setPlayerNickNameText(NickName);
        setAnimalNameText();
    }
    void setPlayerNickNameText(string NickName)
    {
        transform.Find("PlayerNickName").gameObject.GetComponent<TextMeshProUGUI>().text
            = NickName;
    }
    void setAnimalNameText()
    {
        transform.Find("AnimalName").gameObject.GetComponent<TextMeshProUGUI>().text
            = AnimalName;
    }
    void OnCickReadyButton(PointerEventData button)
    {
        Debug.Log("�غ��ư");
    }
    
    void Start()
    {
        Init();

        UpdatePlayerInfo("X");
    }

}
