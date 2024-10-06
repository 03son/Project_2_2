using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UI_Button : UI_Popup
{
    public enum GameObjects
    {
        MainScreenButtons
    }
    enum Buttons
    {
        SingleButton,
        MultiButton,
        SettingButton,
        ExitButton,
    }
    enum Texts
    { 
        TMP_VersionText
    }
    enum Images
    { 
    
    }

    Buttons StringToEnum(string buttons)
    {
        return (Buttons)Enum.Parse(typeof(Buttons), buttons);
    }
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        //UI ���ε�
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        //���� ���� ���� ǥ��
        Get<TMP_Text>((int)Texts.TMP_VersionText).text = $"Version {Application.version}";

        //����ȭ�� ��ư 4�� �̺�Ʈ �Ҵ�
        Array buttonNames = Enum.GetValues(typeof(Buttons));
        foreach (int buttonNum in buttonNames)
            GetButton((int)buttonNum).gameObject.AddUIEvent(OnMainButtonClicked);
        
    }
    
    //�̱�, ��Ƽ, ����, ���� �̺�Ʈ
    void OnMainButtonClicked(PointerEventData button)
    {
        //Ŭ���� ��ư�� string name �����͸� enum �����ͷ� ��ȯ
        Buttons buttonName = StringToEnum(button.pointerEnter.name);

        switch (buttonName)
        {
            //�̱� ��ư
            case Buttons.SingleButton:
                Debug.Log((string)button.pointerEnter.name);
                break;

            // ��Ƽ ��ư
            case Buttons.MultiButton:
                Debug.Log((string)button.pointerEnter.name);
                break;

            // ���� ��ư
            case Buttons.SettingButton:
                OpenSettingScreen();
                break;

            // ���� ���� ��ư
            case Buttons.ExitButton:
                GameExit();
                break;
        }
    }

    //���� â ����
    void OpenSettingScreen()
    {
        //����ȭ�� ��ư ������Ʈ ��Ȱ��ȭ
        Get<GameObject>((int)GameObjects.MainScreenButtons).SetActive(false);

        //���� â ����
        UIManger.Instance.ShowPopupUI<MainSettingScreen>();
    }
    //���� ����
    void GameExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
