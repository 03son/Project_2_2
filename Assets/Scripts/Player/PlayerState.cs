using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public enum playerState
    { 
        »ıÁ¸,
        Á×À½
    }
    playerState state;
    public playerState State
    {
        get => state;
        set 
        {
            switch (value)
            {
                case playerState.»ıÁ¸:
                    state = playerState.»ıÁ¸;
                    break;

                case playerState.Á×À½:
                    state = playerState.Á×À½;
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
