using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class GameManager : Singleton<GameManager>
{
    HashTable playerCP;

    void Awake()
    {
        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;

        CreatePlayer();
    }

    void CreatePlayer()
    {
        // 출현 위치 정보를 배열에저장
        Transform[] points =
        GameObject.Find("PlayerSpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(0, points.Length);
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
    }
}
