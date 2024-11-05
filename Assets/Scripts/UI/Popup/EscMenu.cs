using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class EscMenu : UI_Popup
{
    GameObject gameObjectssss;

    public InGameUI inGameUI;
    enum GameObjects
    {

    }
    enum Buttons
    {
        Resume,
        Option,
        Exit,
        Close
    }
    public override void Init()
    {
        //UI ���ε�
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        //����ϱ�, �ɼ�, ������, �ݱ�
        GetButton((int)Buttons.Resume).gameObject.AddUIEvent(CloseEscMenu);
        GetButton((int)Buttons.Option).gameObject.AddUIEvent(Option);
        GetButton((int)Buttons.Exit).gameObject.AddUIEvent(Exit_InGame);
        GetButton((int)Buttons.Close).gameObject.AddUIEvent(CloseEscMenu);

    }
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Option(PointerEventData button)
    {
        UIManger.Instance.ShowPopupUI<MainSettingScreen>();
    }
    void Exit_InGame(PointerEventData button)
    {
        UIManger.Instance.ShowPopupUI<DoubleCheck_UI>();
    }
    void CloseEscMenu(PointerEventData button)//esc�޴� �ݱ�
    {
        inGameUI.CloseEscMenu();
    }
}
