using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Recorder = Photon.Voice.Unity.Recorder;

public class EmptyMic : MonoBehaviour
{
    void Awake()
    {
        if (Global_Microphone.UseMic == null)
        {
            GetComponent<Recorder>().enabled = false;
        }
    }
}
