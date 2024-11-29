using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_GameManager : GameManager
{
    //싱글플레이 게임매니저

    GameObject player;
    GameObject Enemy;
    GameObject 토끼;
    void Awake()
    {
        if (PhotonNetwork.IsConnected)
            return;

        GetComponent<Single_GameManager>().enabled = true;
        GetComponent<Multi_GameManager>().enabled = false;

        player = Resources.Load<GameObject>("Player");
        Enemy = Resources.Load<GameObject>("UI_Resources_Enemy");
        토끼 = Resources.Load<GameObject>("토끼");

        CreatePlayer();
        CreateEnemy();
    }

    public override void CreatePlayer()
    {
        // 출현 위치 정보를 배열에저장
        Transform[] points =
        GameObject.Find("PlayerSpawnPointGroup").gameObject.GetComponentsInChildren<Transform>();

      //  GameObject.Instantiate(player, points[1].position, points[1].rotation);
        GameObject.Instantiate(토끼 , points[1].position, points[1].rotation);
    }
    public override void CreateEnemy() //UI씬을 기준으로 작성함
    {
        Transform[] points =
         GameObject.Find("EnemySpawnPoint").gameObject.GetComponentsInChildren<Transform>();

        Instantiate(Enemy, points[1].position, points[1].rotation);
    }
}
