using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public enum playerState
    { 
        ����,
        ����
    }
    playerState state;
    public playerState State
    {
        get => state;
        set 
        {
            switch (value)
            {
                case playerState.����:
                    state = playerState.����;
                    break;

                case playerState.����:
                    state = playerState.����;
                    break;
            }
            state = value;
        }
    }

    public void GetState(out playerState _state)
    {
        _state = state;
    }
}
