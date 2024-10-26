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
        roomName.text = $"방 이름: {PhotonNetwork.CurrentRoom.Name}";
    }
    public void PlayerEnteredRoom() //플레이어 입장
    {
        UpdatePlayerList();

    }

   public void UpdatePlayerList() //플레이어 리스트 업데이트
    {
        for (int i=0; i<4;i++) //초기화
        {
            playerNickName[i] = null;
            playerActorNumber[i] = 0;

            player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo("X");
        }

        int _index = 0;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)//접속한 플레이어 표시
        {
            playerNickName[_index] = player.Value.NickName;
            playerActorNumber[_index] = player.Key;

            player_RoomInfo[_index].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(player.Value.NickName);

            _index++;
        }
    }

    public void localPlayerLeftRoom()//플레이어가 나갈 때
    {
        UpdatePlayerList();
    }


    public override void OnRoomPropertiesUpdate(HashTable propertiesThatChanged)
    {

    }
}