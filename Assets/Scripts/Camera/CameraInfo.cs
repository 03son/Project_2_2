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
        ObserverCam.enabled = false;
    }
    public static void UseObserverCam()
    {
        MainCam.enabled = false;
        ObserverCam.enabled = true;
    }
}
