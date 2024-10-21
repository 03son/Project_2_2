using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class MultiPlayList : UI_Popup
{
    //�̸� �Է� ��ǲ�ʵ�
    GameObject RoomNameInputField;

    //�Է� �� ���� ��ư
    GameObject JoinCreateRoom;

    // RoomItem �������� �߰��� ScrollContent
    public Transform scrollContent;

    enum Objects
    {
        InputFieldRoomName,
        JoinCreateRoom,
        PhotonManager
    }
    enum Buttons
    {
        CreateRoom
    }
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(Objects));

        GetButton((int)Buttons.CreateRoom).gameObject.AddUIEvent(OnClickCreateRoomButton);

        GetObject((int)Objects.JoinCreateRoom).gameObject.AddUIEvent(Oncilck_Join_Createroom);

        RoomNameInputField = GetObject((int)Objects.InputFieldRoomName);
        JoinCreateRoom = GetObject((int)Objects.JoinCreateRoom);
    }
    void Start()
    {
        Init();

        RoomNameInputField.SetActive(false);
        JoinCreateRoom.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && this.gameObject.activeSelf)
        {
            //����ȭ�� ��ư 4�� Ȱ��ȭ
            GameObject.Find("UI_Button").transform.GetChild(1).gameObject.SetActive(true);

            //��Ƽ ����Ʈ ��Ȱ��ȭ
            gameObject.SetActive(false);
            //ClosePopupUI();

            //�������� ����
            PhotonManager.instance.DisConnentSever();
        }
    }

    void Oncilck_Join_Createroom(PointerEventData button)//�� �̸� �Է� �� ���� ��ư
    {
        string roomName = RoomNameInputField.GetComponent<TMP_InputField>().text;

        //���� ����
        string regexResult = Regex.Replace(roomName,@"\s", "");

        if (regexResult.Length > 0)
        {
            //������ �̸����� �� ����
            PhotonManager.instance.CreateRoom(regexResult);
            return;
        }

        Debug.Log("�̸��� �Է��ϼ���");
    }
    public void JoinRoom()
    {
        //�� ����
        gameObject.SetActive(false);
        //ClosePopupUI();
        UIManger.Instance.ShowPopupUI<Room>();
    }

    void OnClickCreateRoomButton(PointerEventData button) //�� ���� ��ư Ŭ��
    {
        //��ǲ�ʵ� ��/Ȱ��ȭ
        if (RoomNameInputField.activeSelf == false)
        {
            RoomNameInputField.SetActive(true);
            JoinCreateRoom.SetActive(true);
        }
        else
        {
            RoomNameInputField.SetActive(false);
            JoinCreateRoom.SetActive(false);
        }
       
    }
}
