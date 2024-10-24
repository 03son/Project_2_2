using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using System;
using HashTable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;

public class RoomSetting : MonoBehaviourPunCallbacks
{
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

        for (int i =0; i< 4; i++)
        {
            playerNickName[i] = "0";
            playerActorNumber[i] = 0;

            PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable {
                    {$"PlayerNickName{i}","0" },{$"ActorNumber{i}" ,0}
                });
        }
    }

    void Start()
    {
        roomName.text = $"���̸� : {PhotonNetwork.CurrentRoom.Name}";
    }


    public void PlayerEnteredRoom()//������� �濡 ���� �� �� �濡 �ִ� �÷��̾�� ���� ��������
    {
        for (int i = 0; i< 4; i++)
        {
            Debug.Log($"�濡 �ִ� �÷��̾� �̸� : {playerCP[$"PlayerNickName{i}"]}  ���͹�ȣ : {playerCP[$"ActorNumber{i}"]}");
            if ( (string)playerCP[$"PlayerNickName{i}"] != "0" ) // �̹� ���� �ִ� �÷��̾� ǥ��
            {//<=== ���ľ� ��!!!! <=== ���ľ� ��!!!!<===���ľ� ��!!!!
               // Debug.Log($"�濡 �ִ� �÷��̾� �̸� : {playerCP[$"PlayerNickName{i}"]}  ���͹�ȣ : {playerCP[$"ActorNumber{i}"]}");
                playerNickName[i] = (string)playerCP[$"PlayerNickName{i}"];
                playerActorNumber[i] = (int)playerCP[$"ActorNumber{i}"];

                player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);
                //break;
            }
            else if((int)playerCP[$"ActorNumber{i}"] == 0)
            {
                Debug.Log("���������κ��ھ�����");
                PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable {
                    {$"PlayerNickName{i}",PhotonNetwork.LocalPlayer.NickName },{$"ActorNumber{i}" ,PhotonNetwork.LocalPlayer.ActorNumber}
                });

                playerNickName[i] = PhotonNetwork.LocalPlayer.NickName;
                playerActorNumber[i] = PhotonNetwork.LocalPlayer.ActorNumber;

                player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);
                break;
            }
        }
    }

    public void PlayerLeftRoom(int actorNumber)//�����Ͱ� �ƴ� �÷��̾ ���� ���� ��
    {
        for (int i=0; i < 4; i++ )
        {
            if (playerActorNumber[i] == actorNumber)
            {
              
                PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable {
                    {$"PlayerNickName{i}","0" },{$"ActorNumber{i}" ,0}
                });
              
                playerNickName[i] = "0";   
                playerActorNumber[i] = 0;

                player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo("X");
                break;
            }
        }
    }

    public void UpdatePlayerNickName(string playerName, int actorNumber)
    {
        for (int i = 0; i<4; i++)
        {
            //�г��� ������ ��������� �г��� �߰�
            if (playerNickName[i] == "0" && playerActorNumber[i] == 0)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable {
                    {$"PlayerNickName{i}",playerName },{$"ActorNumber{i}" ,actorNumber}
                });

                playerNickName[i] = playerName;
                playerActorNumber[i] = actorNumber;

                player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);
                break;
            }
            else if (!playerCP.ContainsKey($"PlayerNickName{i}") && !playerCP.ContainsKey($"ActorNumber{i}"))
            {
                Debug.Log("2�� °");
                PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable {
                    {$"PlayerNickName{i}",(string)playerCP[$"PlayerNickName{i}"] },{$"ActorNumber{i}" ,(int)playerCP[$"ActorNumber{i}"]}
                });

                playerNickName[i] = (string)playerCP[$"PlayerNickName{i}"];
                playerActorNumber[i] = (int)playerCP[$"ActorNumber{i}"];
                player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(playerNickName[i]);
                break;
            }
            else if (playerActorNumber[i] == actorNumber)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new HashTable {
                    {$"PlayerNickName{i}","0" },{$"ActorNumber{i}" ,0}
                });

                playerNickName[i] = "0";
                playerActorNumber[i] = 0;

                player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo("X");
                break;
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

    }
}
