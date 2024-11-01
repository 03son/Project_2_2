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
using static RoomManager;

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
        roomName.text = $"방 이름: {PhotonNetwork.CurrentRoom.Name}";
    }

    public void PlayerEnteredRoom() //플레이어 입장
    {
        SetRoomName();
        StartCoroutine(playerList());
    }

    IEnumerator playerList()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        UpdatePlayerList();
    }

    public void UpdatePlayerList() //플레이어 리스트 업데이트
    {
        for (int i = 0; i < 4; i++) //초기화
        {
            playerNickName[i] = null;
            playerActorNumber[i] = 0;

            player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdatePlayerInfo("", 0, ""); //닉네임, 액터넘버, 동물이름
            player_RoomInfo[i].GetComponent<Player_RoomInfo>().setAnimalNameText("");

            //준비 해제
            player_RoomInfo[i].GetComponent<Player_RoomInfo>().isReady = false;
            player_RoomInfo[i].GetComponent<Player_RoomInfo>().UpdateReadyUI();
            player_RoomInfo[i].GetComponent<Player_RoomInfo>().Animal_Image.sprite = nullPlayerImage;
        }

        string animalName = "무작위"; //기본값
        int _index = 0;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)//접속한 플레이어 표시
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
                animalName = "무작위";
            }
            player_RoomInfo[_index].GetComponent<Player_RoomInfo>().UpdatePlayerInfo(player.Value.NickName, player.Key, animalName);
            _index++;
        }

    }

    public void localPlayerLeftRoom()//플레이어가 나갈 때
    {
        UpdatePlayerList();

    }

    int[] playerNumber;
    void GameStart()
    {
        //4명 모두가 준비 상태이면 인게임으로 입장
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

            Debug.Log($"{targetPlayer.NickName}의 준비 상태: {_isReady}");

            //프로퍼티가 바뀐 플레이어의 액터넘버를 비교해서 찾아서 토글
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