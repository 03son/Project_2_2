using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HashTable = ExitGames.Client.Photon.Hashtable;
using System;
using JetBrains.Annotations;
using Photon.Pun.Demo.PunBasics;
using System.Reflection;
using static UnityEngine.Rendering.VolumeComponent;
using UnityEngine.EventSystems;

public class RoomManager : MonoBehaviourPunCallbacks
{
    HashTable playerCP;
    HashTable roomCP;

    public GameObject[] player_RoomInfo = new GameObject[4];
    public TextMeshProUGUI roomName;

    [SerializeField] string[] playerNickName = new string[4];
    [SerializeField] int[] playerActorNumber = new int[4];

    public Sprite[] CharacterImage = new Sprite[5];

    public Sprite nullPlayerImage;

    public Button start_testversion;//�׽�Ʈ
    void Awake()
    {   
        if (PhotonNetwork.IsConnected)
        {
            roomCP = PhotonNetwork.CurrentRoom.CustomProperties;
            playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            start_testversion.gameObject.SetActive(true);
            start_testversion.gameObject.AddUIEvent(start_test);
        }
        else
        {
            start_testversion.gameObject.SetActive(false);
        }

        if (GameInfo.IsGameFinish == true)
        {
            SetRoomName();
            StartCoroutine(playerList());
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }
    void start_test(PointerEventData button) //�׽�Ʈ�� ���۹�ư(�ο� ���X)
    {
        if (PhotonNetwork.IsMasterClient)
        {
           if (ReadyPlayer())
           {
                // ���� �ᰡ ���ο� �÷��̾ ������ ���ϰ� ��
                PhotonNetwork.CurrentRoom.IsOpen = false;

                PhotonView photonView = GetComponent<PhotonView>();//�׽�Ʈ

                photonView.RPC("LoadGame", RpcTarget.All);
           }
        }
    }

    bool ReadyPlayer()
    {
        switch (PhotonNetwork.CurrentRoom.PlayerCount)
        {
            case 2:
                if (player_RoomInfo[0].GetComponent<Player_RoomInfo>().isReady &&
                    player_RoomInfo[1].GetComponent<Player_RoomInfo>().isReady)
                    return true;
                break;
            case 3:
                if (player_RoomInfo[0].GetComponent<Player_RoomInfo>().isReady &&
                    player_RoomInfo[1].GetComponent<Player_RoomInfo>().isReady &&
                    player_RoomInfo[2].GetComponent<Player_RoomInfo>().isReady)
                    return true;
                break;
            case 4:
                if (player_RoomInfo[0].GetComponent<Player_RoomInfo>().isReady &&
                    player_RoomInfo[1].GetComponent<Player_RoomInfo>().isReady &&
                    player_RoomInfo[2].GetComponent<Player_RoomInfo>().isReady &&
                    player_RoomInfo[3].GetComponent<Player_RoomInfo>().isReady)
                    return true;
                break;

        }
        return false;
    }
    void SetRoomName()
    {
        roomName.text = $"�� �̸�: {PhotonNetwork.CurrentRoom.Name}";
    }

    public void PlayerEnteredRoom() //�÷��̾� ����
    {
        SetRoomName();
        StartCoroutine(playerList());
    }

    IEnumerator playerList()
    {
        yield return new WaitForSecondsRealtime(0.001f);
        UpdatePlayerList();
    }

    public void UpdatePlayerList() //�÷��̾� ����Ʈ ������Ʈ
    {
        for (int i = 0; i < 4; i++) //�ʱ�ȭ
        {
            playerNickName[i] = null;
            playerActorNumber[i] = 0;

            player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo("", 0, ""); //�г���, ���ͳѹ�, �����̸�
            player_RoomInfo[i].GetComponent<Player_RoomInfo>().setAnimalNameText("");

            //�غ� ����
            player_RoomInfo[i].GetComponent<Player_RoomInfo>().isReady = false;
            player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdateReadyUI();
            player_RoomInfo[i].GetComponent<Player_RoomInfo>().Animal_Image.sprite = nullPlayerImage;
        }

        string animalName = "������"; //�⺻��
        int _index = 0;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)//������ �÷��̾� ǥ��
        {
            playerNickName[_index] = player.Value.NickName;
            playerActorNumber[_index] = player.Key;

            if (player.Value.CustomProperties.ContainsKey("isReady"))
            {
                player.Value.CustomProperties.TryGetValue("isReady", out object isReadyValue);

                bool _isReady = (bool)isReadyValue;
                player_RoomInfo[_index].GetComponent<Player_RoomInfo>().isReady = _isReady;
                player_RoomInfo[_index].GetComponent<Player_RoomInfo>().UpdateReadyUI();

                Debug.Log($"{player.Value.NickName}__{player.Value.ActorNumber}__{isReadyValue}");
            }

            if (player.Value.CustomProperties.ContainsKey("animalName"))
            {
                player.Value.CustomProperties.TryGetValue("animalName", out object _animalName);
                animalName = (string)_animalName;
            }
            else
            {
                animalName = "������";
            }
            player_RoomInfo[_index].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(player.Value.NickName, player.Key, animalName);
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
            if (PhotonNetwork.IsMasterClient)
            {
                // ���� �ᰡ ���ο� �÷��̾ ������ ���ϰ� ��
                PhotonNetwork.CurrentRoom.IsOpen = false;

                PhotonView photonView = GetComponent<PhotonView>();//�׽�Ʈ

                photonView.RPC("LoadGame", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    public void LoadGame()
    {
        LoadingSceneManager.InGameLoading(GameInfo.InGameScenes, 1);

        GameInfo.IsMasterClient = PhotonNetwork.IsMasterClient ? true : false;
    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, HashTable changedProps)
    {
        if (changedProps.ContainsKey("animalName"))
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                for (int index = 0; index < 4; index++)
                {
                    int Number = player_RoomInfo[index].GetComponent<Player_RoomInfo>().Actor_num;
                    if (targetPlayer.ActorNumber == Number)
                    {
                        targetPlayer.CustomProperties.TryGetValue("animalName", out object _animalName);
                        player_RoomInfo[index].GetComponent<Player_RoomInfo>().setAnimalNameText((string)_animalName);
                        break;
                    }
                }
            }
        }
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
           //GameStart();
            return;
        }
    }
}