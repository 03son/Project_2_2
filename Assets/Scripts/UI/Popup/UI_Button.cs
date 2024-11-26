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
    static bool PlayerNickNameText = false;

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
        TMP_VersionText,
        PlayerNickName_Text
    }
    enum Images
    { 
    Logo_Image
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
            string NickNameRegexResult = Regex.Replace(playerNickName, @"\s", ""); //���� ����

            if (NickNameRegexResult.Length > 0)
            {
                PlayerNickName = NickNameRegexResult;
                G_ApplyButton.gameObject.SetActive(true); //���� ��ư Ȱ��ȭ

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SetPlayerNickName();
                    return;
                }
            }
            else
                G_ApplyButton.gameObject.SetActive(false);//���� ��ư ��Ȱ��ȭ
        }
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

        GetImage((int)Images.Logo_Image).gameObject.GetComponent<Image>().enabled = true;

        MultiList.gameObject.SetActive(false);

        Get<GameObject>((int)GameObjects.MainScreenButtons).SetActive(false);

        //�÷��̾� �г��� �ؽ�Ʈ ���� 1���� ��Ȱ��ȭ���� Ȱ��ȭ�ϱ�
        if (!PlayerNickNameText)
        {
            Get<TMP_Text>((int)Texts.PlayerNickName_Text).gameObject.SetActive(false);
            PlayerNickNameText = true;
        }
        else //���Ĵ� ��� Ȱ�� ����
        {
            Get<TMP_Text>((int)Texts.PlayerNickName_Text).gameObject.SetActive(true);
            Get<TMP_Text>((int)Texts.PlayerNickName_Text).text = $"�÷��̾� �г��� : {PlayerPrefs.GetString("PlayerNickName")}";
        }

        ApplyButton = G_ApplyButton.GetComponent<Button>();

        InputNickName = setPlayerNickName.gameObject.GetComponentInChildren<TMP_InputField>();

        ApplyButton.onClick.AddListener(SetPlayerNickName);

        //�г����� ���� �� ������ �г��� �Է� ȭ���� false, �г����� ������ true
        setPlayerNickName.SetActive(PhotonManager.instance.PlayerNickName == null ? true : false);
        Get<GameObject>((int)GameObjects.MainScreenButtons).SetActive(PhotonManager.instance.PlayerNickName != null ? true : false);
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
                StartSingleGame();
                break;

            // ��Ƽ ��ư
            case Buttons.MultiButton:
                OpenMultiRoomList();
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

    void StartSingleGame()
    {
        Debug.Log("�̱� ����");
        LoadingSceneManager.InGameLoading(GameInfo.InGameScenes,1);
    }
    //��Ƽ �� ����Ʈ ����
    void OpenMultiRoomList()
    {
        //����ȭ�� ��ư ������Ʈ ��Ȱ��ȭ
        Get<GameObject>((int)GameObjects.MainScreenButtons).SetActive(false);

        PhotonManager.instance.StartCoroutine(PhotonManager.instance.SetLoadingText());

        //UIManger.Instance.ShowPopupUI<MultiPlayList>();

        //���� ����
        PhotonManager.instance.ConnectSever();
    }

    //���� â ����
    void OpenSettingScreen()
    {
        //����ȭ�� ��ư ������Ʈ ��Ȱ��ȭ
        Get<GameObject>((int)GameObjects.MainScreenButtons).SetActive(false);

        //���� â ����
        UIManger.Instance.ShowPopupUI<MainSettingScreen>();
    }

    void SetPlayerNickName()
    {
        //���� �޴����� �г��� ����
        PhotonManager.instance.PlayerNickName = PlayerNickName;
        PlayerPrefs.SetString("PlayerNickName", PlayerNickName);

        //�г��� �Է� â ��Ȱ��ȭ
        setPlayerNickName.SetActive(false);

        //����ȭ�� ��ư Ȱ��ȭ
        Get<GameObject>((int)GameObjects.MainScreenButtons).SetActive(true);

        //�÷��̾� �г��� �ؽ�Ʈ Ȱ��ȭ
        Get<TMP_Text>((int)Texts.PlayerNickName_Text).gameObject.SetActive(true);

        //�÷��̾� �г��� ǥ��
        Get<TMP_Text>((int)Texts.PlayerNickName_Text).text = $"�÷��̾� �г��� : {PlayerPrefs.GetString("PlayerNickName")}";

        //�ΰ� ����
        GetImage((int)Images.Logo_Image).gameObject.GetComponent<Image>().enabled = true;
    }
    //���� ����
    void GameExit()
    {
        //�����Ͻðڽ��ϱ�? �˾�UI ����
        UIManger.Instance.ShowPopupUI<DoubleCheck_UI>();
    }
}
