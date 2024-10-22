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
    public  void JoinedRoom() //������Ŭ���̾�Ʈ
    {
        Debug.Log("�濡 �����߽��ϴ�.");

        // ���� ��ȣ �ο�
        AssignPlayerNumber();
    }

    private void AssignPlayerNumber()
    {
        // ���� ���� �ο� ���� �������� ��ȣ �ο�
        int playerNumber = PhotonNetwork.CurrentRoom.PlayerCount-1;

        // Custom Properties�� ��ȣ ����
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["PlayerNumber"] = playerNumber;

        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerSolt[(int)playerNumber].NickName = PhotonNetwork.NickName;
            PlayerSolt[(int)playerNumber].UpdatePlayerInfo();
        }
    }

    // ���ο� �÷��̾ ������ �� �ݹ�
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} ����!");
      
        // ���� ���� �ο� ���� �������� ��ȣ �ο�
        int playerNumber = PhotonNetwork.CurrentRoom.PlayerCount - 1;

        // Custom Properties�� ��ȣ ����
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["PlayerNumber"] = playerNumber;

        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        PlayerSolt[(int)playerNumber].NickName = newPlayer.NickName;
        PlayerSolt[(int)playerNumber].UpdatePlayerInfo();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("���Ƥ�����������������");
    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

    }
}
