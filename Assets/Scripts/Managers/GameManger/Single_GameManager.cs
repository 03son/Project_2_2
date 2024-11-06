using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_GameManager : GameManager , IGameManager
{
    //�̱��÷��� ���ӸŴ���

    GameObject player;
    void Awake()
    {
        if (PhotonNetwork.IsConnected)
            return;

        GetComponent<Single_GameManager>().enabled = true;
        GetComponent<Multi_GameManager>().enabled = false;

        player = Resources.Load<GameObject>("Player");

        CreatePlayer();
    }

    public void CreatePlayer()
    {
        // ���� ��ġ ������ �迭������
        Transform[] points =
        GameObject.Find("PlayerSpawnPointGroup").gameObject.GetComponentsInChildren<Transform>();

        GameObject.Instantiate(player, points[1].position, points[1].rotation);
    }
}