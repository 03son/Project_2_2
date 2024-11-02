using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class Multi_GameManager : GameManager
{
    HashTable playerCP;

    protected override void Awake()
    {
        if(!PhotonNetwork.IsConnected)
            return;

        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;

        CreatePlayer();
    }

    protected override void CreatePlayer()
    {
        // 출현 위치 정보를 배열에저장
        Transform[] points =
        GameObject.Find("PlayerSpawnPointGroup").gameObject.GetComponentsInChildren<Transform>();

        int index = 0;
        int[] Number = new int[4];
        foreach (int player in PhotonNetwork.CurrentRoom.Players.Keys)
        {
            Number[index] = player;
            index++;
        }
        Array.Sort(Number);//오름차순으로 액터넘버를 정렬

        int idx = 1;//0은 PlayerSpawnPointGroup 오브젝트에서 스폰 됨
        foreach (int playerNumber in Number)
        {
            if (playerNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
            }
            idx++;
        }
    }
}
