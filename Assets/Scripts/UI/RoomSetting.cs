using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using System;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class RoomSetting : MonoBehaviourPunCallbacks
{
    HashTable playerCP;

    public TextMeshProUGUI roomName;

    [SerializeField]
    string[] playerNickName = new string[4];

    [SerializeField]
    int[] playerActorNumber = new int[4];

    private void Awake()
    {
       playerCP = PhotonNetwork.CurrentRoom.CustomProperties;
    }

    void Start()
    {
        roomName.text = $"방이름 : {PhotonNetwork.CurrentRoom.Name}";
    }


    public void PlayerEnteredRoom()//만들어진 방에 입장 할 때 방에 있는 플레이어들 정보 가져오기
    {
        for (int i = 0; i< 4; i++)
        {
            Debug.Log($"방에 있는 플레이어 이름 : {playerCP[$"PlayerNickName{i}"]}  액터번호 : {playerCP[$"ActorNumber{i}"]}");
        }
    }

    public void UpdatePlayerNickName(string playerName, int actorNumber)
    {
        for (int i = 0; i<4; i++)
        {
            //닉네임 슬롯이 비어있으면 닉네임 추가
            if (playerNickName[i] == "")
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable {
                    {$"PlayerNickName{i}",playerName },{$"ActorNumber{i}" ,actorNumber}
                });

                playerNickName[i] = playerName;
                playerActorNumber[i] = actorNumber;

                return;
            }
            else if (playerActorNumber[i] == actorNumber)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable {
                    {$"PlayerNickName{i}","" },{$"ActorNumber{i}" ,0}
                });

                playerNickName[i] = "";
                playerActorNumber[i] = 0;
                return;
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    { 
    
    }
}
