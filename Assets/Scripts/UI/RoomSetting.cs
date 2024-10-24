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
        roomName.text = $"���̸� : {PhotonNetwork.CurrentRoom.Name}";
    }


    public void PlayerEnteredRoom()//������� �濡 ���� �� �� �濡 �ִ� �÷��̾�� ���� ��������
    {
        for (int i = 0; i< 4; i++)
        {
            Debug.Log($"�濡 �ִ� �÷��̾� �̸� : {playerCP[$"PlayerNickName{i}"]}  ���͹�ȣ : {playerCP[$"ActorNumber{i}"]}");
        }
    }

    public void UpdatePlayerNickName(string playerName, int actorNumber)
    {
        for (int i = 0; i<4; i++)
        {
            //�г��� ������ ��������� �г��� �߰�
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
