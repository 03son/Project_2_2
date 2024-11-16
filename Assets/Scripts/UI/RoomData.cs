using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    private RoomInfo _roomInfo;
   
    // 하위에 있는 TMP_Text를 저장할 변수
    private TMP_Text roomInfoText;

    // 하위에 있는 TMP_Text에 플레이어 수 표시
    private TMP_Text roomInfoPlayerCount;

    // PhotonManager 접근 변수
    private PhotonManager photonManager;
   
    // 프로퍼티 정의
    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            // 룸 정보 표시
            roomInfoText.text = $"{_roomInfo.Name}";

            //플레이어 인원 수 표시
            roomInfoPlayerCount.text = $"({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
            
            // 버튼 클릭 이벤트에 함수 연결
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            OnEnterRoom(_roomInfo.Name));
        }
    }
    void Awake()
    {
        roomInfoText = transform.GetChild(0).GetComponent<TMP_Text>();
        roomInfoPlayerCount = transform.GetChild(1).GetComponent<TMP_Text>();

        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
    }
    void OnEnterRoom(string roomName)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            // 유저명 설정
            //photonManager.SetUserId();
            // 룸 접속
            PhotonNetwork.JoinRoom(roomName);
            Debug.Log("접속할 방 이름 : " + roomName);
        }
    }
}
