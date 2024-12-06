using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUICanvas
{
    public static GameObject Esc; //esc창
    public static GameObject Observer; //관전UI
    public static GameObject HUD; //1인칭 UI

    public static void OpenUI(string UIName)
    {
       // Esc.SetActive(false);
        Observer.SetActive(false);
        HUD.SetActive(false);

        switch (UIName)
        {
            case "Esc":
                Esc.SetActive(true);
                break;

            case "Observer":
                Observer.SetActive(true);
                break;

            case "HUD":
                HUD.SetActive(true);
                break;
        }
    }
}
