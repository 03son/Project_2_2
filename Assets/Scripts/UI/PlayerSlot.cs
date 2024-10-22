using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class PlayerSlot : MonoBehaviourPunCallbacks
{
    public Player_RoomInfo[] PlayerSolt = new Player_RoomInfo[4];

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerSolt[i] = transform .GetChild(i).gameObject.GetComponent<Player_RoomInfo>();
        }
    }
    public  void JoinedRoom() //마스터클라이언트
    {
        Debug.Log("방에 입장했습니다.");

        // 고유 번호 부여
        AssignPlayerNumber();
    }

    private void AssignPlayerNumber()
    {
        // 현재 방의 인원 수를 기준으로 번호 부여
        int playerNumber = PhotonNetwork.CurrentRoom.PlayerCount-1;

        // Custom Properties에 번호 저장
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["PlayerNumber"] = playerNumber;

        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerSolt[(int)playerNumber].NickName = PhotonNetwork.NickName;
            PlayerSolt[(int)playerNumber].UpdatePlayerInfo();
        }
    }

    // 새로운 플레이어가 입장할 때 콜백
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} 입장!");
      
        // 현재 방의 인원 수를 기준으로 번호 부여
        int playerNumber = PhotonNetwork.CurrentRoom.PlayerCount - 1;

        // Custom Properties에 번호 저장
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["PlayerNumber"] = playerNumber;

        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        PlayerSolt[(int)playerNumber].NickName = newPlayer.NickName;
        PlayerSolt[(int)playerNumber].UpdatePlayerInfo();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("빠아ㅏㅏㅏㅏㅏ나가ㅏㄹ");
    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

    }
}
