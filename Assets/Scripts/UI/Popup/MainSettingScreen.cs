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
        VideoSetting,//비디오 설정 창
        InputkeySetting,//조작키 창
        SoundSetting//사운드 설정 창
    }
    enum Buttons
    {
        VideoSetting_Button,//비디오 설정 버튼
        InputkeySetting_Button,//조작키 버튼
        SoundSetting_Button//사운드 설정 버튼
    }
    Buttons StringToEnum(string buttons)
    {
        return (Buttons)Enum.Parse(typeof(Buttons), buttons);
    }

    void Start()
    {
        Init();

        //설정 창 열었을 때 비디오 옵션이 디폴트로 열림
        GetObject((int)Buttons.VideoSetting_Button).transform.GetChild(0).gameObject.SetActive(true);
        OnOffSetting((int)Buttons.VideoSetting_Button);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && this.gameObject.activeSelf)
        {
            //메인화면 버튼 4종 활성화
            GameObject.Find("UI_Button").transform.GetChild(1).gameObject.SetActive(true);

            //설정창 닫기
            ClosePopupUI();
        }
    }
    //세부 설정 창
    Array settingButtons;

    //비디오, 조작키, 사운드 버튼
    Array buttonNames;
    public override void Init()
    {
        Bind<GameObject>(typeof(SettingGameObjects));
        Bind<Button>(typeof(Buttons));

        //비디오, 조작키, 사운드 OnOff 버튼 받아오기
        buttonNames = Enum.GetValues(typeof(Buttons));
        foreach (int buttonNum in buttonNames)
            GetButton((int)buttonNum).gameObject.AddUIEvent(OnClickSettingMenuButton);

        //비디오, 조작키, 사운드 세부설정 창 받아오기
        settingButtons = Enum.GetValues(typeof(SettingGameObjects));
    }

    //상세 설정창 On/Off
    void OnClickSettingMenuButton(PointerEventData button)
    {
        //클릭한 버튼과 enum의 값에 따라 버튼 실행
        Buttons buttonName = StringToEnum(button.pointerEnter.name);

        switch (buttonName)
        { 
            //비디오 설정(0)
            case Buttons.VideoSetting_Button:
                OnOffSetting((int)Buttons.VideoSetting_Button);
                break;
            
            //조작키(1)
            case Buttons.InputkeySetting_Button:
                OnOffSetting((int)Buttons.InputkeySetting_Button);
                break;

            //사운드(2)
            case Buttons.SoundSetting_Button:
                OnOffSetting((int)Buttons.SoundSetting_Button);
                break;
        }
    }
    //해당 설정 창 여닫기
    void OnOffSetting(int num)
    {
        //선택한 버튼의 int값이랑 array에 있는 값이랑 비교, 일치하면 활성화 불일치하면 비활성화
        foreach (int number in settingButtons)
            if (num == number)
                GetObject(num).transform.GetChild(0).gameObject.SetActive(true);
            else
                GetObject(number).transform.GetChild(0).gameObject.SetActive(false);

        //버튼의 Image
        Image selectButton;
        
        //버튼의 Text
        TextMeshProUGUI textColor;

        //버튼의 알파값 조정, 선택한 버튼이 아닌 버튼들은 불투명하게 변경
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
