using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair_Image : MonoBehaviour
{
   Image crosshairImage;
   public Sprite[] crosshair = new Sprite[2];// 0 = СЁ, 1 = Ме

    void Start()
    {
        crosshairImage = GetComponent<Image>();
        Not_Interaction();
    }
    public void Interaction()
    {
        crosshairImage.sprite = crosshair[1];
    }
    public void Not_Interaction()
    {
        crosshairImage.sprite = crosshair[0];
    }
}
