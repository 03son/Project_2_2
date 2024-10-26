using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine;
using System.Collections;
using HashTable = ExitGames.Client.Photon.Hashtable;
using System;
using JetBrains.Annotations;
using Photon.Pun.Demo.PunBasics;
using System.Reflection;
using static UnityEngine.Rendering.VolumeComponent;

public class RoomManager : MonoBehaviourPunCallbacks
{
    HashTable playerCP;
    HashTable roomCP;

    public GameObject[] player_RoomInfo = new GameObject[4];
    public TextMeshProUGUI roomName;

    [SerializeField] string[] playerNickName = new string[4];
    [SerializeField] int[] playerActorNumber = new int[4];

    void Awake()
    {
        roomCP = PhotonNetwork.CurrentRoom.CustomProperties;
        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
    }

    void Start()
    {
        roomName.text = $"�� �̸�: {PhotonNetwork.CurrentRoom.Name}";
    }
    public void PlayerEnteredRoom() //�÷��̾� ����
    {
        UpdatePlayerList();

    }

   public void UpdatePlayerList() //�÷��̾� ����Ʈ ������Ʈ
    {
        for (int i=0; i<4;i++) //�ʱ�ȭ
        {
            playerNickName[i] = null;
            playerActorNumber[i] = 0;

            player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo("X");
        }

        int _index = 0;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)//������ �÷��̾� ǥ��
        {
            playerNickName[_index] = player.Value.NickName;
            playerActorNumber[_index] = player.Key;

            player_RoomInfo[_index].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(player.Value.NickName);

            _index++;
        }
    }

    public void localPlayerLeftRoom()//�÷��̾ ���� ��
    {
        UpdatePlayerList();
    }


    public override void OnRoomPropertiesUpdate(HashTable propertiesThatChanged)
    {

    }
}