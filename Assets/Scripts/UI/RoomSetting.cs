using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine;
using System.Collections;
using HashTable = ExitGames.Client.Photon.Hashtable;
using System;
using JetBrains.Annotations;
using Photon.Pun.Demo.PunBasics;
using System.Reflection;
using static UnityEngine.Rendering.VolumeComponent;
using static RoomManager;

public class RoomManager : MonoBehaviourPunCallbacks
{
    HashTable playerCP;
    HashTable roomCP;

    public GameObject[] player_RoomInfo = new GameObject[4];
    public TextMeshProUGUI roomName;

    [SerializeField] string[] playerNickName = new string[4];
    [SerializeField] int[] playerActorNumber = new int[4];

    void Awake()
    {
        roomCP = PhotonNetwork.CurrentRoom.CustomProperties;
        playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
    }

    private void Start()
    {

    }

    void SetRoomName()
    {
        roomName.text = $"�� �̸�: {PhotonNetwork.CurrentRoom.Name}";
    }

    public enum AniMalName
    {
        ������,
        ����,
        �䳢,
        ����,
        �����
    }

    public void PlayerEnteredRoom() //�÷��̾� ����
    {
        SetRoomName();
        StartCoroutine(playerList());
    }

    IEnumerator playerList()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        UpdatePlayerList();
    }

    public void UpdatePlayerList() //�÷��̾� ����Ʈ ������Ʈ
    {
        for (int i = 0; i < 4; i++) //�ʱ�ȭ
        {
            playerNickName[i] = null;
            playerActorNumber[i] = 0;

            player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo("", 0, ""); //�г���, ���ͳѹ�, �����̸�

            //�غ� ����
            player_RoomInfo[i].GetComponent<Player_RoomInfo>().isReady = false;
            player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdateReadyUI();
        }

        string animalName = AniMalName.������.ToString();
        int _index = 0;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)//������ �÷��̾� ǥ��
        {
            playerNickName[_index] = player.Value.NickName;
            playerActorNumber[_index] = player.Key;

            if (player.Value.CustomProperties.ContainsKey("animalName"))
            {
                player.Value.CustomProperties.TryGetValue("animalName", out object _animalName);
                animalName = (string)_animalName;
            }

            player_RoomInfo[_index].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(player.Value.NickName, player.Key, animalName);

            if (player.Value.CustomProperties.ContainsKey("isReady"))
            {
                player.Value.CustomProperties.TryGetValue("isReady", out object isReadyValue);

                bool _isReady = (bool)isReadyValue;
                player_RoomInfo[_index].GetComponent<Player_RoomInfo>().isReady = _isReady;
                player_RoomInfo[_index].GetComponent<Player_RoomInfo>().UpdateReadyUI();

                Debug.Log($"{player.Value.NickName}__{player.Value.ActorNumber}__{isReadyValue}");
            }
            _index++;
        }

    }

    public void localPlayerLeftRoom()//�÷��̾ ���� ��
    {
        UpdatePlayerList();

    }

    int[] playerNumber;
    void GameStart()
    {
        //4�� ��ΰ� �غ� �����̸� �ΰ������� ����
        if (
            player_RoomInfo[0].GetComponent<Player_RoomInfo>().isReady &&
            player_RoomInfo[1].GetComponent<Player_RoomInfo>().isReady &&
            player_RoomInfo[2].GetComponent<Player_RoomInfo>().isReady &&
            player_RoomInfo[3].GetComponent<Player_RoomInfo>().isReady
            )
        {
            LoadingSceneManager.InGameLoading("1", 1);
            return;
        }
    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, HashTable changedProps)
    {
        if (changedProps.ContainsKey("isReady"))
        {
            bool _isReady = (bool)changedProps["isReady"];

            Debug.Log($"{targetPlayer.NickName}�� �غ� ����: {_isReady}");

            //������Ƽ�� �ٲ� �÷��̾��� ���ͳѹ��� ���ؼ� ã�Ƽ� ���
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                for (int index = 0; index < 4; index++)
                {
                    int Number = player_RoomInfo[index].GetComponent<Player_RoomInfo>().Actor_num;
                    if (targetPlayer.ActorNumber == Number)
                    {
                        player_RoomInfo[index].GetComponent<Player_RoomInfo>().isReady = _isReady;
                        player_RoomInfo[index].GetComponent<Player_RoomInfo>().UpdateReadyUI();
                        break;
                    }
                }
            }
            GameStart();
        }
    }
}