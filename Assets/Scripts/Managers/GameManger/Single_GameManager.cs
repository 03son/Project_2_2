using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Single_GameManager : GameManager
{
    //싱글플레이 게임매니저

    public static Single_GameManager instance;

     int diePlayerCount = 0;

    GameObject Enemy;
    GameObject Player;
    void Awake()
    {
        if (PhotonNetwork.IsConnected)
            return;

        instance = this;

        GetComponent<Single_GameManager>().enabled = true;
        GetComponent<Multi_GameManager>().enabled = false;
  
        Enemy = Resources.Load<GameObject>("UI_Resources_Enemy");
        Player = Resources.Load<GameObject>("Character/토끼");

        CreatePlayer();
        CreateEnemy();
    }

    public override void CreatePlayer()
    {
        // 출현 위치 정보를 배열에저장
        Transform[] points =
        GameObject.Find("PlayerSpawnPointGroup").gameObject.GetComponentsInChildren<Transform>();
        Instantiate(Player, points[1].position, points[1].rotation);
    }
    public override void CreateEnemy() //UI씬을 기준으로 작성함
    {
        Transform[] points =
         GameObject.Find("EnemySpawnPoint").gameObject.GetComponentsInChildren<Transform>();

        Instantiate(Enemy, points[1].position, points[1].rotation);
    }

    public override void PlayerDie(bool die)
    {
        if (die) //플레이어가 죽었을 때
        {
            diePlayerCount += 1;

            if (diePlayerCount == 1)
            {
                GameInfo.endingNumber = 2; //전원 사망
                StartCoroutine(GoEndingVideo());

                GameInfo.IsGameFinish = false;
            }
        }
        if (!die) // 부활 했을 때
        {
            diePlayerCount -= 1;
            Debug.Log("부활");
        }
    }
    public IEnumerator GoEndingVideo()//엔딩 이벤트로 이동
    {
        yield return new WaitForSecondsRealtime(3);
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
            PhotonNetwork.DestroyAll();

        yield return new WaitForSecondsRealtime(0.01f);
        SceneManager.LoadScene("EndingEventVideo_Screen");
        //LoadingSceneManager.InGameLoading("Main_Screen",1);
    }
}
