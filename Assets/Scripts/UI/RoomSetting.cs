using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using System;
using HashTable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;
using System.Security.Cryptography;

public class RoomSetting : MonoBehaviourPunCallbacks
{
    private HashTable playerCP;
    public GameObject[] player_RoomInfo = new GameObject[4];
    public TextMeshProUGUI roomName;

    [SerializeField] private string[] playerNickName = new string[4];
    [SerializeField] private int[] playerActorNumber = new int[4];

    void Awake()
    {
        playerCP = PhotonNetwork.CurrentRoom.CustomProperties;
    }

    void Start()
    {
        roomName.text = $"방 이름: {PhotonNetwork.CurrentRoom.Name}";
    }

    public void PlayerEnteredRoom(string newName, int newNumber)
    {
        Debug.Log("플레이어 입장");
        int emptyIndex = GetEmptySlotIndex();

        if (emptyIndex != -1)
        {
            UpdatePlayerData(emptyIndex, newName, newNumber);
        }
    }

    public void localPlayerLeftRoom()
    {
        //ResetAllPlayers();
        UpdateUIWithCurrentPlayerData();
    }

    public void PlayerLeftRoom(int actorNumber)
    {
        int index = FindPlayerIndex(actorNumber);
        if (index != -1)
        {
            UpdatePlayerData(index, "X", 0);
        }
    }

    /*
    public void UpdatePlayerNickName(string playerName, int actorNumber)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InitializeRoomData();
        }
        else
        {
            SyncPlayerData();
            AssignPlayerData(playerName, actorNumber);
        }
    }*/
    public void UpdatePlayerNickName(string playerName, int actorNumber)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InitializeRoomData(); // 방 초기화
        }
        else
        {
            SyncPlayerData(); // 모든 플레이어 정보를 동기화
            AssignPlayerData(playerName, actorNumber); // 들어온 플레이어의 데이터 할당
        }

        // 여기서 SyncPlayerData() 호출 후 모든 플레이어 정보를 다시 UI에 업데이트
        UpdateUIWithCurrentPlayerData();
    }
    public void UpdateUIWithCurrentPlayerData()
    {
        for (int i = 0; i < 4; i++)
        {
            player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);
        }
    }
    int GetEmptySlotIndex()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!playerCP.TryGetValue($"ActorNumber{i}", out object value) || (int)value == 0)
            {
                return i;
            }
        }
        return -1;
    }

    int FindPlayerIndex(int actorNumber)
    {
        for (int i = 0; i < 4; i++)
        {
            if (playerActorNumber[i] == actorNumber) return i;
        }
        return -1;
    }

    void UpdatePlayerData(int index, string name, int actorNumber)
    {
        playerNickName[index] = name;
        playerActorNumber[index] = actorNumber;

        player_RoomInfo[index].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(name);

        PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable
        {
            { $"PlayerNickName{index}", name },
            { $"ActorNumber{index}", actorNumber }
        });
    }

    void ResetAllPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            UpdatePlayerData(i, "X", 0);
        }
    }

    void InitializeRoomData()
    {
        for (int i = 0; i < 4; i++)
        {
            UpdatePlayerData(i, i == 0 ? PhotonNetwork.LocalPlayer.NickName : "X",
                             i == 0 ? PhotonNetwork.LocalPlayer.ActorNumber : 0);
        }
    }

    /*
    void SyncPlayerData()
    {
        for (int i = 0; i < 4; i++)
        {
            playerNickName[i] = (string)playerCP[$"PlayerNickName{i}"];
            playerActorNumber[i] = (int)playerCP[$"ActorNumber{i}"];

            player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);
        }
    }
    */
    public void SyncPlayerData()
    {
        for (int i = 0; i < 4; i++)
        {
            if (playerCP.TryGetValue($"PlayerNickName{i}", out object nickNameValue) &&
                playerCP.TryGetValue($"ActorNumber{i}", out object actorNumberValue))
            {
                playerNickName[i] = (string)nickNameValue;
                playerActorNumber[i] = (int)actorNumberValue;

                player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);
            }
            else
            {
                playerNickName[i] = "X"; // 플레이어가 없으면 "X"로 표시
                playerActorNumber[i] = 0;
                player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);
            }
        }
    }

    void AssignPlayerData(string playerName, int actorNumber)
    {
        int emptyIndex = GetEmptySlotIndex();

        if (emptyIndex != -1)
        {
            UpdatePlayerData(emptyIndex, playerName, actorNumber);
        }
    }

    /*
    HashTable playerCP;

    public GameObject[] player_RoomInfo = new GameObject[4];

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

    public void PlayerEnteredRoom(string newName, int newNumber)//만들어진 방에 입장 할 때 방에 있는 플레이어들 정보 가져오기
    {
        Debug.Log("플레이어 입장입장");
        for (int i = 0; i<4; i++)
        {
            if ((int)playerCP[$"ActorNumber{i}"] == 0)
            {
                playerNickName[i] = newName;
                playerActorNumber[i] = newNumber;

                player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);
                return;
            }
        }
       
    }

    public void localPlayerLeftRoom() //나 자신이 룸에서 나갈 때
    {
        for (int i = 0; i < 4; i++)
        {
            playerNickName[i] = "0";
            playerActorNumber[i] = 0;
        }
    }

    public void PlayerLeftRoom(int actorNumber)//남 플레이어가 룸을 나갈 때
    {
        for (int i = 0; i<4; i++)
        {
            if ((int)playerCP[$"ActorNumber{i}"] == actorNumber)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable {
                        {$"PlayerNickName{i}","0" },{$"ActorNumber{i}" ,0}
                    });

                playerNickName[i] = "0";
                playerActorNumber[i] = 0;

                player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo("X");
                return;
            }
        }
    }

    public void UpdatePlayerNickName(string playerName, int actorNumber)
    {
        if (PhotonNetwork.IsMasterClient) //처음 방 만들 때 마스터가 초기화
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable {
                        {$"PlayerNickName{0}",PhotonNetwork.LocalPlayer.NickName },{$"ActorNumber{0}" ,PhotonNetwork.LocalPlayer.ActorNumber}
                    });

            playerNickName[0] = PhotonNetwork.LocalPlayer.NickName;
            playerActorNumber[0] = PhotonNetwork.LocalPlayer.ActorNumber;
            player_RoomInfo[0].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[0]);

            for (int i = 1; i < 4; i++)
            {

                playerNickName[i] = "X";
                playerActorNumber[i] = 0;


                PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable {
                        {$"PlayerNickName{i}","0" },{$"ActorNumber{i}" ,0}
                    });
                player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);
            }
        }
        else //들어오는 사람들
        {
            for (int i = 0; i < 4; i++)//나머지 들어오는 사람들은 이미 접속해 있는 플레이어의 닉네임을 표시
            {
                playerNickName[i] = (string)playerCP[$"PlayerNickName{i}"];
                playerActorNumber[i] = (int)playerCP[$"ActorNumber{i}"];

                player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);

                if (playerActorNumber[i] == 0)
                {
                    playerNickName[i] = "X";
                    player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);
                }

            }
            for (int i =0;i<4; i++) //자신의 닉네임 저장, 표시
            {
                if ((int)playerCP[$"ActorNumber{i}"] == 0)
                {
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable {
                        {$"PlayerNickName{i}",playerName },{$"ActorNumber{i}" ,actorNumber}
                    });

                    playerNickName[i] = playerName;
                    playerActorNumber[i] = actorNumber;

                    player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);
                   return;
                }
            }
        }   
    }
    */
}
