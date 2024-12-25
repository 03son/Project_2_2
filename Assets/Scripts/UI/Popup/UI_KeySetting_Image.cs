using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_KeySetting_Image : MonoBehaviour
{
    public Sprite[] KeyButtonImage = new Sprite[2]; // 0 = 기본 이미지 , 1 = 키 변경 이미지

    void OnEnable()
    {
        GetComponent<Image>().sprite = KeyButtonImage[0];
    }

    public void clickButtonImage()
    {
        GetComponent<Image>().sprite = KeyButtonImage[1];
    }
    public void resetButtonImage()
    {
        GetComponent<Image>().sprite = KeyButtonImage[0];
    }

}
