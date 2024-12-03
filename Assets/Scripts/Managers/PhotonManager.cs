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

    // �� ��Ͽ� ���� �����͸� �����ϱ� ���� ��ųʸ� �ڷ���
    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();

    public string PlayerNickName = null;

    HashTable playerCP;

    // �� ����� ǥ���� ������
    public GameObject roomItemPrefab;

    public GameObject MultiList;

    public GameObject Room;

    public Transform RoomListPrefabPos;

    private Transform room_Player;

    public GameObject BG;

    RoomManager roomSetting;

    #region �ؽ�Ʈ ����

    //���� ���� ���� �ؽ�Ʈ
    public TextMeshProUGUI StatusText;

    //�ε� �ؽ�Ʈ
    public TextMeshProUGUI LoadingText;
    #endregion
    private Dictionary<int, GameObject> playerItems = new Dictionary<int, GameObject>();
    void Awake()
    {
        instance = this;

        string playerPrefs_NickName = PlayerPrefs.GetString("PlayerNickName").Length > 0 ? PlayerPrefs.GetString("PlayerNickName") : null;
        PlayerNickName = playerPrefs_NickName != null ? playerPrefs_NickName : null;

        //������ Ŭ���̾�Ʈ�� �� �ڵ� ����ȭ �ɼ�
        PhotonNetwork.AutomaticallySyncScene = true;

        //���� ����
        PhotonNetwork.GameVersion = Application.version;

        //�ʴ� ��Ű�� ���� Ƚ��
        PhotonNetwork.SendRate = 60;

        //�ʴ� ���� ������ ���� Ƚ��
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

    

    public void ConnectSever() //���� ����
    {
        if (!PhotonNetwork.IsConnected)
        {
            playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
            playerCP = new HashTable() { { "isReady", null } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);

            playerCP = new HashTable() { { "animalName", null } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);

            //���� �� ����� ���� ���� ����
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public void DisConnentSever()//���� ���� ����
    {
        if (PhotonNetwork.IsConnected)
        {
            //���� ������
            PhotonNetwork.Disconnect();
        }
    }

    public void leaveRoom() //�� ������
    {
        PhotonNetwork.LeaveRoom();
    }

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 4 },null);
    }

    public IEnumerator SetLoadingText()//�ε��� �ؽ�Ʈ
    {
        LoadingText.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            if (i == 4)
                i = 0;

            string repeatedcom = new string('.', i);
            LoadingText.text = $"����������{repeatedcom}";
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    #region �ݹ� �Լ�
    public override void OnConnected()
    {
        // ���� ���� �Լ�
        Debug.Log("���� ���� ����");

        //�г��� ����
        PhotonNetwork.NickName = PlayerNickName;
        Debug.Log($"�÷��̾� �г��� : {PhotonNetwork.NickName}");
    }//���� ����
    public override void OnConnectedToMaster()//���� ID ������ ���� ����
    {
        //�κ� ����
        Debug.Log("������ ������ ����");
        PhotonNetwork.JoinLobby();
    }
    public override void OnCreatedRoom()
    {
        //�� ����
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }//�� ����

    public override void OnJoinedRoom()//�� ����
    {
        Debug.Log("�뿡 ����");
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
    public override void OnRoomListUpdate(List<RoomInfo> roomList) //�� ��� ����
    {
        // ������ RoomItem �������� ������ �ӽú���
        GameObject tempRoom = null;
        foreach (var roomInfo in roomList)
        {
            // ���� ������ ���
            if (roomInfo.RemovedFromList == true)
            {
                // ��ųʸ����� �� �̸����� �˻��� ����� RoomItem �����ո� ����
                rooms.TryGetValue(roomInfo.Name, out tempRoom);
                // RoomItem ������ ����
                Destroy(tempRoom);
                // ��ųʸ����� �ش� �� �̸��� �����͸� ����
                rooms.Remove(roomInfo.Name);
            }
            else // �� ������ ����� ���
            {
                // �� �̸��� ��ųʸ��� ���� ��� ���� �߰�
                if (rooms.ContainsKey(roomInfo.Name) == false)
                {
                    // RoomInfo �������� scrollContent ������ ����
                    GameObject roomPrefab = Instantiate(roomItemPrefab, RoomListPrefabPos);
                    // �� ������ ǥ���ϱ� ���� RoomInfo ���� ����
                    roomPrefab.GetComponent<RoomData>().RoomInfo = roomInfo;
                    
                    // ��ųʸ� �ڷ����� ������ �߰�
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
        Debug.Log("�κ� ����");
    }//�κ� ����
    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ����");

        StopCoroutine(SetLoadingText());
        LoadingText.gameObject.SetActive(false);

        MultiList.gameObject.SetActive(true);
    }//�κ� ����
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}���� �����߽��ϴ�.");

        roomSetting.PlayerEnteredRoom();


    }//���� ���� ����
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (SceneManager.GetActiveScene().name == "Main_Screen")
        {
            roomSetting.localPlayerLeftRoom();
        }
        Debug.Log($"{otherPlayer.NickName}���� �����߽��ϴ�.");
    }//���� ���� ����
    public override void OnLeftRoom() //���� �濡�� ������ ��
    {
        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
        playerCP = new HashTable() { { "isReady", null } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);

        playerCP = new HashTable() { { "animalName", null } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);

        Debug.Log("�濡�� �������ϴ�.");
        StartCoroutine(SetLoadingText());
        
    }
    public override void OnDisconnected(DisconnectCause cause)//������ ������ �������� ��
    {
        Debug.Log("���� ���� ����");
    }
    #endregion
}
