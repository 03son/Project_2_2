using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_GameManager : GameManager
{
    //�̱��÷��� ���ӸŴ���

    GameObject player;
    GameObject Enemy;
    GameObject �䳢;
    void Awake()
    {
        if (PhotonNetwork.IsConnected)
            return;

        GetComponent<Single_GameManager>().enabled = true;
        GetComponent<Multi_GameManager>().enabled = false;

        player = Resources.Load<GameObject>("Player");
        Enemy = Resources.Load<GameObject>("UI_Resources_Enemy");
        �䳢 = Resources.Load<GameObject>("�䳢");

        CreatePlayer();
        CreateEnemy();
    }

    public override void CreatePlayer()
    {
        // ���� ��ġ ������ �迭������
        Transform[] points =
        GameObject.Find("PlayerSpawnPointGroup").gameObject.GetComponentsInChildren<Transform>();

      //  GameObject.Instantiate(player, points[1].position, points[1].rotation);
        GameObject.Instantiate(�䳢 , points[1].position, points[1].rotation);
    }
    public override void CreateEnemy() //UI���� �������� �ۼ���
    {
        Transform[] points =
         GameObject.Find("EnemySpawnPoint").gameObject.GetComponentsInChildren<Transform>();

        Instantiate(Enemy, points[1].position, points[1].rotation);
    }
}
