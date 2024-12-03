using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerState : MonoBehaviourPun
{
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
            if (photonView.IsMine) // 로컬 플레이어만 상태를 변경
            {
                photonView.RPC(nameof(SyncState), RpcTarget.All, (int)value); // 상태 동기화
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

        // 상태 변경 확인용 디버그 메시지
        Debug.Log($"플레이어 상태가 {state}로 동기화되었습니다. (ActorNumber: {photonView.Owner.ActorNumber})");
    }
}
