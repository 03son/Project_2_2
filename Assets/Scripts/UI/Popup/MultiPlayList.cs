using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using Photon.Pun;

public class MultiPlayList : UI_Popup
{
    //방 이름 입력 인풋필드
    GameObject RoomNameInputField;

    //입력 후 생성 버튼
    GameObject JoinCreateRoom;

    // RoomItem 프리팹이 추가될 ScrollContent
    public Transform scrollContent;

    enum Objects
    {
        InputFieldRoomName,
        JoinCreateRoom,
        PhotonManager,
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
            Go_Main();

        if (Input.GetKeyDown(KeyCode.Return) && RoomNameInputField.activeSelf)
            Go_Main();
    }

    void Oncilck_Join_Createroom(PointerEventData button)//방 이름 입력 후 입장 버튼
    {
        string roomName = RoomNameInputField.GetComponent<TMP_InputField>().text;

        //공백 제거
        string regexResult = Regex.Replace(roomName,@"\s", "");

        if (regexResult.Length > 0)
        {
            //지정된 이름으로 방 생성
            PhotonManager.instance.CreateRoom(regexResult);
            return;
        }

        Debug.Log("이름을 입력하세요");
    }
    public void JoinRoom()
    {
        //방 입장
        gameObject.SetActive(false);
    }

    void OnClickCreateRoomButton(PointerEventData button) //방 생성 버튼 클릭
    {
        //인풋필드 비/활성화
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

    void Go_Main()
    {
        //메인화면 버튼 4종 활성화
        GameObject.Find("UI_Button").transform.GetChild(1).gameObject.SetActive(true);

        //멀티 리스트 비활성화
        gameObject.SetActive(false);
        //ClosePopupUI();

        //서버연결 끊기
        PhotonManager.instance.DisConnentSever();
    }
}
