using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInfo
{
    public static Camera MainCam;
    public static Camera ObserverCam;

    public static void UseMainCam()
    {
        MainCam.enabled = true;
        MainCam.GetComponent<AudioListener>().enabled = true;
        ObserverCam.enabled = false;
        ObserverCam.GetComponent<AudioListener>().enabled = false;
    }
    public static void UseObserverCam()
    {
        MainCam.enabled = false;
        MainCam.GetComponent<AudioListener>().enabled = false;
        ObserverCam.enabled = true;
        ObserverCam.GetComponent<AudioListener>().enabled = true;
    }
}
