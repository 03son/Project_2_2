using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UI_Button;
using Unity.VisualScripting;
using System;
using TMPro;

public class MainSettingScreen : UI_Button
{
    enum SettingGameObjects
    {
        VideoSetting,//���� ���� â
        InputkeySetting,//����Ű â
        SoundSetting//���� ���� â
    }
    enum Buttons
    {
        VideoSetting_Button,//���� ���� ��ư
        InputkeySetting_Button,//����Ű ��ư
        SoundSetting_Button//���� ���� ��ư
    }
    Buttons StringToEnum(string buttons)
    {
        return (Buttons)Enum.Parse(typeof(Buttons), buttons);
    }

    void Start()
    {
        Init();

        //���� â ������ �� ���� �ɼ��� ����Ʈ�� ����
        GetObject((int)Buttons.VideoSetting_Button).transform.GetChild(0).gameObject.SetActive(true);
        OnOffSetting((int)Buttons.VideoSetting_Button);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && this.gameObject.activeSelf)
        {
            //����ȭ�� ��ư 4�� Ȱ��ȭ
            GameObject.Find("UI_Button").transform.GetChild(1).gameObject.SetActive(true);

            //����â �ݱ�
            ClosePopupUI();
        }
    }
    //���� ���� â
    Array settingButtons;

    //����, ����Ű, ���� ��ư
    Array buttonNames;
    public override void Init()
    {
        Bind<GameObject>(typeof(SettingGameObjects));
        Bind<Button>(typeof(Buttons));

        //����, ����Ű, ���� OnOff ��ư �޾ƿ���
        buttonNames = Enum.GetValues(typeof(Buttons));
        foreach (int buttonNum in buttonNames)
            GetButton((int)buttonNum).gameObject.AddUIEvent(OnClickSettingMenuButton);

        //����, ����Ű, ���� ���μ��� â �޾ƿ���
        settingButtons = Enum.GetValues(typeof(SettingGameObjects));
    }

    //�� ����â On/Off
    void OnClickSettingMenuButton(PointerEventData button)
    {
        //Ŭ���� ��ư�� enum�� ���� ���� ��ư ����
        Buttons buttonName = StringToEnum(button.pointerEnter.name);

        switch (buttonName)
        { 
            //���� ����(0)
            case Buttons.VideoSetting_Button:
                OnOffSetting((int)Buttons.VideoSetting_Button);
                break;
            
            //����Ű(1)
            case Buttons.InputkeySetting_Button:
                OnOffSetting((int)Buttons.InputkeySetting_Button);
                break;

            //����(2)
            case Buttons.SoundSetting_Button:
                OnOffSetting((int)Buttons.SoundSetting_Button);
                break;
        }
    }
    //�ش� ���� â ���ݱ�
    void OnOffSetting(int num)
    {
        //������ ��ư�� int���̶� array�� �ִ� ���̶� ��, ��ġ�ϸ� Ȱ��ȭ ����ġ�ϸ� ��Ȱ��ȭ
        foreach (int number in settingButtons)
            if (num == number)
                GetObject(num).transform.GetChild(0).gameObject.SetActive(true);
            else
                GetObject(number).transform.GetChild(0).gameObject.SetActive(false);

        //��ư�� Image
        Image selectButton;
        
        //��ư�� Text
        TextMeshProUGUI textColor;

        //��ư�� ���İ� ����, ������ ��ư�� �ƴ� ��ư���� �������ϰ� ����
        foreach (int number in buttonNames)
        {
            if (num == number)
            {
                selectButton  = GetButton(num).gameObject.GetComponent<Image>();
                selectButton.color = 
                    new Color(selectButton.color.r, selectButton.color.g, selectButton.color.b, 1f);

                textColor = GetButton(num).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                textColor.faceColor = 
                    new Color32(textColor.faceColor.r , textColor.faceColor.g, textColor.faceColor.b, 255);
            }
            else
            {
                selectButton = GetButton(number).gameObject.GetComponent<Image>();
                selectButton.color =
                    new Color(selectButton.color.r, selectButton.color.g, selectButton.color.b, 0.3f);

                textColor = GetButton(number).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                textColor.faceColor = 
                    new Color32(textColor.faceColor.r, textColor.faceColor.g, textColor.faceColor.b, 100);
            }
        }
    }

}
