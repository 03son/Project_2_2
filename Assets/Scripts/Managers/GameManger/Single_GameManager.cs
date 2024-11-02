using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_GameManager : GameManager
{
    GameObject player;
    protected override void Awake()
    {
        if (PhotonNetwork.IsConnected)
            return;

        player = Resources.Load<GameObject>("Player");

        CreatePlayer();
    }

    protected override void CreatePlayer()
    {
        // 출현 위치 정보를 배열에저장
        Transform[] points =
        GameObject.Find("PlayerSpawnPointGroup").gameObject.GetComponentsInChildren<Transform>();

        GameObject.Instantiate(player, points[1].position, points[1].rotation);
    }
}
