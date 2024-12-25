using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Pun.Demo.PunBasics;
using HashTable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using static Player_RoomInfo;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager instance;

    // 룸 목록에 대한 데이터를 저장하기 위한 딕셔너리 자료형
    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();

    public string PlayerNickName = null;

    HashTable playerCP;

    // 룸 목록을 표시할 프리팹
    public GameObject roomItemPrefab;

    public GameObject MultiList;

    public GameObject Room;

    public Transform RoomListPrefabPos;

    private Transform room_Player;

    public GameObject BG;

    RoomManager roomSetting;

    #region 텍스트 변수

    //서버 연결 상태 텍스트
    public TextMeshProUGUI StatusText;

    //로딩 텍스트
    public TextMeshProUGUI LoadingText;
    #endregion
    private Dictionary<int, GameObject> playerItems = new Dictionary<int, GameObject>();
    void Awake()
    {
        instance = this;

        string playerPrefs_NickName = PlayerPrefs.GetString("PlayerNickName").Length > 0 ? PlayerPrefs.GetString("PlayerNickName") : null;
        PlayerNickName = playerPrefs_NickName != null ? playerPrefs_NickName : null;

        //마스터 클라이언트의 씬 자동 동기화 옵션
        PhotonNetwork.AutomaticallySyncScene = true;

        //게임 버전
        PhotonNetwork.GameVersion = Application.version;

        //초당 패키지 전송 횟수
        PhotonNetwork.SendRate = 60;

        //초당 서버 데이터 전송 횟수
        PhotonNetwork.SerializationRate = 30;

        PhotonNetwork.AddCallbackTarget(this);
    }

    void Start()
    {
        roomSetting = BG.GetComponent<RoomManager>();
        LoadingText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        }
    }

    

    public void ConnectSever() //서버 연결
    {
        if (!PhotonNetwork.IsConnected)
        {
            playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
            playerCP = new HashTable() { { "isReady", null } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);

            playerCP = new HashTable() { { "animalName", null } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);

            //설정 된 값대로 포톤 서버 접속
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public void DisConnentSever()//서버 연결 종료
    {
        if (PhotonNetwork.IsConnected)
        {
            //서버 나가기
            PhotonNetwork.Disconnect();
        }
    }

    public void leaveRoom() //방 나가기
    {
        PhotonNetwork.LeaveRoom();
    }

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 4 },null);
    }

    public IEnumerator SetLoadingText()//로딩중 텍스트
    {
        LoadingText.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            if (i == 4)
                i = 0;

            string repeatedcom = new string('.', i);
            LoadingText.text = $"서버연결중{repeatedcom}";
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    #region 콜백 함수
    public override void OnConnected()
    {
        // 서버 접속 함수
        Debug.Log("서버 접속 성공");

        //닉네임 설정
        PhotonNetwork.NickName = PlayerNickName;
        Debug.Log($"플레이어 닉네임 : {PhotonNetwork.NickName}");
    }//서버 연결
    public override void OnConnectedToMaster()//고유 ID 마스터 서버 접속
    {
        //로비 접속
        Debug.Log("마스터 서버에 접속");
        PhotonNetwork.JoinLobby();
    }
    public override void OnCreatedRoom()
    {
        //방 생성
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }//방 생성

    public override void OnJoinedRoom()//룸 입장
    {
        Debug.Log("룸에 입장");
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        playerCP = new HashTable() { { "animalName", null } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);

        MultiList.GetComponent<MultiPlayList>().JoinRoom();

        Room.SetActive(true);

        roomSetting.PlayerEnteredRoom();

    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList) //방 목록 수신
    {
        // 삭제된 RoomItem 프리팹을 저장할 임시변수
        GameObject tempRoom = null;
        foreach (var roomInfo in roomList)
        {
            // 룸이 삭제된 경우
            if (roomInfo.RemovedFromList == true)
            {
                // 딕셔너리에서 룸 이름으로 검색해 저장된 RoomItem 프리팹를 추출
                rooms.TryGetValue(roomInfo.Name, out tempRoom);
                // RoomItem 프리팹 삭제
                Destroy(tempRoom);
                // 딕셔너리에서 해당 룸 이름의 데이터를 삭제
                rooms.Remove(roomInfo.Name);
            }
            else // 룸 정보가 변경된 경우
            {
                // 룸 이름이 딕셔너리에 없는 경우 새로 추가
                if (rooms.ContainsKey(roomInfo.Name) == false)
                {
                    // RoomInfo 프리팹을 scrollContent 하위에 생성
                    GameObject roomPrefab = Instantiate(roomItemPrefab, RoomListPrefabPos);
                    // 룸 정보를 표시하기 위해 RoomInfo 정보 전달
                    roomPrefab.GetComponent<RoomData>().RoomInfo = roomInfo;
                    
                    // 딕셔너리 자료형에 데이터 추가
                     rooms.Add(roomInfo.Name, roomPrefab);
                   
                }
                else 
                {
                    rooms.TryGetValue(roomInfo.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = roomInfo;
                }
            }
            Debug.Log($"Room={roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})");
        }
    }
    public override void OnLeftLobby()
    {
        Debug.Log("로비 나감");
    }//로비 퇴장
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속");

        StopCoroutine(SetLoadingText());
        LoadingText.gameObject.SetActive(false);

        MultiList.gameObject.SetActive(true);
    }//로비 입장
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}님이 입장했습니다.");

        roomSetting.PlayerEnteredRoom();


    }//남이 방을 입장
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (SceneManager.GetActiveScene().name == "Main_Screen")
        {
            roomSetting.localPlayerLeftRoom();
        }
        Debug.Log($"{otherPlayer.NickName}님이 퇴장했습니다.");
    }//남이 방을 퇴장
    public override void OnLeftRoom() //내가 방에서 나갔을 때
    {
        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
        playerCP = new HashTable() { { "isReady", null } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);

        playerCP = new HashTable() { { "animalName", null } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);

        Debug.Log("방에서 나갔습니다.");
        StartCoroutine(SetLoadingText());
        
    }
    public override void OnDisconnected(DisconnectCause cause)//서버와 연결이 끊어졌을 때
    {
        Debug.Log("서버 연결 종료");
    }
    #endregion
}
