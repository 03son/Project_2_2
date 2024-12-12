using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Single_GameManager : GameManager
{
    //�̱��÷��� ���ӸŴ���

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
        Player = Resources.Load<GameObject>("Character/�䳢");

        CreatePlayer();
        CreateEnemy();
    }

    public override void CreatePlayer()
    {
        // ���� ��ġ ������ �迭������
        Transform[] points =
        GameObject.Find("PlayerSpawnPointGroup").gameObject.GetComponentsInChildren<Transform>();
        Instantiate(Player, points[1].position, points[1].rotation);
    }
    public override void CreateEnemy() //UI���� �������� �ۼ���
    {
        Transform[] points =
         GameObject.Find("EnemySpawnPoint").gameObject.GetComponentsInChildren<Transform>();

        Instantiate(Enemy, points[1].position, points[1].rotation);
    }

    public override void PlayerDie(bool die)
    {
        if (die) //�÷��̾ �׾��� ��
        {
            diePlayerCount += 1;

            if (diePlayerCount == 1)
            {
                GameInfo.endingNumber = 2; //���� ���
                StartCoroutine(GoEndingVideo());

                GameInfo.IsGameFinish = false;
            }
        }
        if (!die) // ��Ȱ ���� ��
        {
            diePlayerCount -= 1;
            Debug.Log("��Ȱ");
        }
    }
    public IEnumerator GoEndingVideo()//���� �̺�Ʈ�� �̵�
    {
        yield return new WaitForSecondsRealtime(3);
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
            PhotonNetwork.DestroyAll();

        yield return new WaitForSecondsRealtime(0.01f);
        SceneManager.LoadScene("EndingEventVideo_Screen");
        //LoadingSceneManager.InGameLoading("Main_Screen",1);
    }
}
