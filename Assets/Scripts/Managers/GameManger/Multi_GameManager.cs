using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using Photon.Voice.PUN;
using static UnityEngine.Rendering.DebugUI;
using HashTable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;
using Photon.Voice.Unity;
using UnityEngine.SceneManagement;

public class Multi_GameManager : GameManager
{
   public static Multi_GameManager instance;
   public int diePlayerCount = 0;

    //��Ƽ�÷��� ���ӸŴ���
    private VoiceConnection voiceConnection;

    HashTable playerCP;
    HashTable roomCP;
    PhotonView pv;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        instance = this;

        GetComponent<Multi_GameManager>().enabled = true;
        GetComponent<Single_GameManager>().enabled = false;

        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
        roomCP = PhotonNetwork.CurrentRoom.CustomProperties;

        roomCP = new HashTable() {{ "diePlayerCount",diePlayerCount } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomCP);

        CreatePlayer();

        if (PhotonNetwork.IsMasterClient)
        {
           CreateEnemy();
        }

        voiceConnection = FindObjectOfType<VoiceConnection>();
        Debug.Log(voiceConnection);
        StartCoroutine(PlayerEixt());
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
                if ((string)playerCP["animalName"] == null || (string)playerCP["animalName"] == "무작위")
                {
                    string[] name = { "토끼", "늑대", "라쿤", "고양이" };
                    int Index = Random.Range(0, name.Length);
                    PhotonNetwork.Instantiate($"Character/{name[Index]}", points[idx].position, points[idx].rotation, 0);
                }
                else if ((string)playerCP["animalName"] != "무작위")
                {
                    PhotonNetwork.Instantiate($"Character/{playerCP["animalName"]}", points[idx].position, points[idx].rotation, 0);
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

    IEnumerator PlayerEixt()
    {
        yield return new WaitForSecondsRealtime(5);

        if (diePlayerCount >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            if (PhotonNetwork.IsMasterClient)
                StartCoroutine(GoEndingVideo());

            GameInfo.IsGameFinish = true;
        }
        StartCoroutine(PlayerEixt());
    }
    public override void PlayerDie(bool die) //죽을 때 마다 카운트
    {
        if (die) //플레이어가 죽었을 때
        {
            diePlayerCount += 1;

            if (diePlayerCount == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                GameInfo.endingNumber = 2; //전원 사망
                StartCoroutine(GoEndingVideo());

                GameInfo.IsGameFinish = true;
            }
        }
        if(!die) // 부활 했을 때
        {
            diePlayerCount -= 1;
            Debug.Log("부활");
        }
    }

    public IEnumerator GoEndingVideo()//엔딩 이벤트로 이동
    {
        yield return new WaitForSecondsRealtime(3);
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.DestroyAll();

        yield return new WaitForSecondsRealtime(0.01f);
        SceneManager.LoadScene("EndingEventVideo_Screen");
        //LoadingSceneManager.InGameLoading("Main_Screen",1);
    }
}
