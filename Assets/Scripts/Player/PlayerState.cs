using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerState : MonoBehaviourPun
{
   public static PlayerState instance;
    private void Awake()
    {
        instance = this;
    }
    public enum playerState
    {
        Survival,
        Die
    }

    private playerState state;

    public playerState State
    {
        get => state;
        set
        {
            if (photonView.IsMine) // ���� �÷��̾ ���¸� ����
            {
                photonView.RPC(nameof(SyncState), RpcTarget.All, (int)value); // ���� ����ȭ
            }
            state = value;
        }
    }

    public void GetState(out playerState _state)
    {
        _state = state;
    }

    [PunRPC]
    void SyncState(int newState)
    {
        state = (playerState)newState;

        // ���� ���� Ȯ�ο� ����� �޽���
        Debug.Log($"�÷��̾� ���°� {state}�� ����ȭ�Ǿ����ϴ�. (ActorNumber: {photonView.Owner.ActorNumber})");
    }
}
