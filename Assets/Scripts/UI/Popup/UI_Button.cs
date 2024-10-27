using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Text.RegularExpressions;

public class UI_Button : UI_Popup
{
    public GameObject MultiList;

    public GameObject setPlayerNickName;

    TMP_InputField InputNickName;

    public GameObject G_ApplyButton;
    Button ApplyButton;

    public string PlayerNickName;

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

    void Update()
    {
        if (setPlayerNickName.gameObject.activeSelf)
        {
            string playerNickName = InputNickName.GetComponent<TMP_InputField>().text;
            string NickNameRegexResult = Regex.Replace(playerNickName, @"\s", ""); //공백 제거

            if (NickNameRegexResult.Length > 0)
            {
                PlayerNickName = NickNameRegexResult;
                G_ApplyButton.gameObject.SetActive(true); //적용 버튼 활성화

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SetPlayerNickName();
                    return;
                }
            }
            else
                G_ApplyButton.gameObject.SetActive(false);//적용 버튼 비활성화
        }
    }

    public override void Init()
    {
        base.Init();

        //UI 바인딩
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        //현재 개발 버전 표시
        Get<TMP_Text>((int)Texts.TMP_VersionText).text = $"Version {Application.version}";

        //메인화면 버튼 4종 이벤트 할당
        Array buttonNames = Enum.GetValues(typeof(Buttons));
        foreach (int buttonNum in buttonNames)
            GetButton((int)buttonNum).gameObject.AddUIEvent(OnMainButtonClicked);


        MultiList.gameObject.SetActive(false);

        Get<GameObject>((int)GameObjects.MainScreenButtons).SetActive(false);

        ApplyButton = G_ApplyButton.GetComponent<Button>();

        InputNickName = setPlayerNickName.gameObject.GetComponentInChildren<TMP_InputField>();

        ApplyButton.onClick.AddListener(SetPlayerNickName);
    }
    
    //싱글, 멀티, 설정, 종료 이벤트
    void OnMainButtonClicked(PointerEventData button)
    {
        //클릭한 버튼의 string name 데이터를 enum 데이터로 변환
        Buttons buttonName = StringToEnum(button.pointerEnter.name);

        switch (buttonName)
        {
            //싱글 버튼
            case Buttons.SingleButton:
                Debug.Log((string)button.pointerEnter.name);
                break;

            // 멀티 버튼
            case Buttons.MultiButton:
                OpenMultiRoomList();
                break;

            // 설정 버튼
            case Buttons.SettingButton:
                OpenSettingScreen();
                break;

            // 게임 종료 버튼
            case Buttons.ExitButton:
                GameExit();
                break;
        }
    }

    //멀티 방 리스트 띄우기
    void OpenMultiRoomList()
    {
        //메인화면 버튼 오브젝트 비활성화
        Get<GameObject>((int)GameObjects.MainScreenButtons).SetActive(false);

        PhotonManager.instance.StartCoroutine(PhotonManager.instance.SetLoadingText());

        //UIManger.Instance.ShowPopupUI<MultiPlayList>();

        //서버 연결
        PhotonManager.instance.ConnectSever();
    }

    //설정 창 띄우기
    void OpenSettingScreen()
    {
        //메인화면 버튼 오브젝트 비활성화
        Get<GameObject>((int)GameObjects.MainScreenButtons).SetActive(false);

        //설정 창 띄우기
        UIManger.Instance.ShowPopupUI<MainSettingScreen>();
    }

    void SetPlayerNickName()
    {
        //포톤 메니저에 닉네임 전송
        PhotonManager.instance.PlayerNickName = PlayerNickName;

        //닉네임 입력 창 비활성화
        setPlayerNickName.SetActive(false);

        //메인화면 버튼 활성화
        Get<GameObject>((int)GameObjects.MainScreenButtons).SetActive(true);
    }
    //게임 종료
    void GameExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
