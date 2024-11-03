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
        //UI 바인딩
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));


        GetButton((int)Buttons.Exit).gameObject.AddUIEvent(Exit_InGame);
        
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

    }
    void Exit_InGame(PointerEventData button)
    {
        UIManger.Instance.ShowPopupUI<DoubleCheck_UI>();
    }
    void CloseEscMenu(PointerEventData button)//esc메뉴 닫기
    {

    }
}
