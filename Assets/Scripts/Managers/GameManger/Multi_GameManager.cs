using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class Multi_GameManager : GameManager
{
    //��Ƽ�÷��� ���ӸŴ���

    HashTable playerCP;

    void Awake()
    {
        if(!PhotonNetwork.IsConnected)
            return;

        GetComponent<Multi_GameManager>().enabled = true;
        GetComponent<Single_GameManager>().enabled = false;

        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;

        CreatePlayer();

        if (PhotonNetwork.IsMasterClient)
        {
           CreateEnemy();
        }
    }

    public override void CreatePlayer()
    {
        // ���� ��ġ ������ �迭������
        Transform[] points =
        GameObject.Find("PlayerSpawnPointGroup").gameObject.GetComponentsInChildren<Transform>();

        int index = 0;
        int[] Number = new int[4];
        foreach (int player in PhotonNetwork.CurrentRoom.Players.Keys)
        {
            Number[index] = player;
            index++;
        }
        Array.Sort(Number);//������������ ���ͳѹ��� ����

        int idx = 1;//0�� PlayerSpawnPointGroup ������Ʈ���� ���� ��
        foreach (int playerNumber in Number)
        {
            if (playerNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
                //PhotonNetwork.Instantiate($"Character/{playerCP["animalName"]}", points[idx].position, points[idx].rotation, 0);
            }
            idx++;
        }
       // if (PhotonNetwork.IsMasterClient)
         //   PhotonNetwork.InstantiateRoomObject("GlassCup (2)" , points[4].position, points[4].rotation, 0);
    }
    
    public override void CreateEnemy() //UI���� �������� �ۼ���
    {
        Transform[] points =
        GameObject.Find("EnemySpawnPoint").gameObject.GetComponentsInChildren<Transform>();
       // PhotonNetwork.Instantiate("UI_Resources_Enemy", points[1].position, points[1].rotation, 0);
        PhotonNetwork.InstantiateRoomObject("UI_Resources_Enemy", points[1].position, points[1].rotation, 0);
         
    }
   
}
