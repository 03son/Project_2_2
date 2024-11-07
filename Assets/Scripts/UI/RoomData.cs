using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    private RoomInfo _roomInfo;
   
    // ������ �ִ� TMP_Text�� ������ ����
    private TMP_Text roomInfoText;

    // ������ �ִ� TMP_Text�� �÷��̾� �� ǥ��
    private TMP_Text roomInfoPlayerCount;

    //���� �� ǥ���� �ǳڰ� �ؽ�Ʈ
    private GameObject IsGamePlay;

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

            IsGamePlay.SetActive(!_roomInfo.IsOpen);
            gameObject.GetComponent<Button>().enabled = !!_roomInfo.IsOpen;

            // ��ư Ŭ�� �̺�Ʈ�� �Լ� ����
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            OnEnterRoom(_roomInfo.Name));
        }
    }
    void Awake()
    {
        roomInfoText = transform.GetChild(0).GetComponent<TMP_Text>();
        roomInfoPlayerCount = transform.GetChild(1).GetComponent<TMP_Text>();
        IsGamePlay = transform.GetChild(2).gameObject;

        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
    }
    void OnEnterRoom(string roomName)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            // �� ����
            PhotonNetwork.JoinRoom(roomName);
            Debug.Log("������ �� �̸� : " + roomName);
        }
    }
}
