using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
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
    playerState state;
    public playerState State
    {
        get => state;
        set 
        {
            switch (value)
            {
                case playerState.Survival:
                    state = playerState.Survival;
                    break;

                case playerState.Die:
                    state = playerState.Die;
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
