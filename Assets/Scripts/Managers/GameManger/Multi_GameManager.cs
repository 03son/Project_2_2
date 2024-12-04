using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using HashTable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class Multi_GameManager : GameManager
{
   public static Multi_GameManager instance;
    //��Ƽ�÷��� ���ӸŴ���

    HashTable playerCP;
    PhotonView pv;

    void Awake()
    {
        instance = this;

        if (!PhotonNetwork.IsConnected)
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
            if (playerNumber == PhotonNetwork.LocalPlayer.ActorNumber)//내 캐릭터 생성
            {
                if ((string)playerCP["animalName"] != "무작위")
                {
                    PhotonNetwork.Instantiate($"Character/{playerCP["animalName"]}", points[idx].position, points[idx].rotation, 0);
                }
                else if ((string)playerCP["animalName"] == "무작위")//무작위 일 때
                {
                    string[] name = { "토끼", "늑대", "라쿤", "고양이" };
                    int Index = Random.Range(0, name.Length);
                    PhotonNetwork.Instantiate($"Character/{name[Index]}", points[idx].position, points[idx].rotation, 0);
                }
            }
            idx++;
        }
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                pv = player.GetComponent<PhotonView>();
            }
        }
    }
    
    public override void CreateEnemy() //UI���� �������� �ۼ���
    {
        Transform[] points =
        GameObject.Find("EnemySpawnPoint").gameObject.GetComponentsInChildren<Transform>();
       // PhotonNetwork.Instantiate("UI_Resources_Enemy", points[1].position, points[1].rotation, 0);
        PhotonNetwork.InstantiateRoomObject("UI_Resources_Enemy", points[1].position, points[1].rotation, 0);
         
    }

}
