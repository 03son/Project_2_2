using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    private RoomInfo _roomInfo;
   
    // ������ �ִ� TMP_Text�� ������ ����
    private TMP_Text roomInfoText;

    // ������ �ִ� TMP_Text�� �÷��̾� �� ǥ��
    private TMP_Text roomInfoPlayerCount;

    // PhotonManager ���� ����
    private PhotonManager photonManager;
   
    // ������Ƽ ����
    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            // �� ���� ǥ��
            roomInfoText.text = $"{_roomInfo.Name}";

            //�÷��̾� �ο� �� ǥ��
            roomInfoPlayerCount.text = $"({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
            
            // ��ư Ŭ�� �̺�Ʈ�� �Լ� ����
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
            // ������ ����
            //photonManager.SetUserId();
            // �� ����
            PhotonNetwork.JoinRoom(roomName);
            Debug.Log("������ �� �̸� : " + roomName);
        }
    }
}
