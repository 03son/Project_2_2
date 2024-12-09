using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    GameObject MainCam;

    public bool popup_escMenu = false;
    void Start()
    {
        MainCam = GameObject.Find("Main Camera");

        SetUICanvas.HUD = GameObject.Find("HUD_Canvas");
        SetUICanvas.Esc = GameObject.Find("Esc_Canvas");
        SetUICanvas.Observer = GameObject.Find("Observer_Canvas");

        SetUICanvas.OpenUI("HUD");
        SetUICanvas.Esc.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //esc 창이 닫혀있으면 열고 열려있으면 닫기
            SetUICanvas.Esc.SetActive(popup_escMenu ? !true : !false);

            //esc 창이 닫혀있으면 커서 잠금, 열려있으면 커서 활성화
            Cursor.lockState = popup_escMenu ? CursorLockMode.Locked : CursorLockMode.None;

            popup_escMenu = !popup_escMenu;
            MainCam.GetComponent<CameraRot>().popup_escMenu = popup_escMenu;
            return;
        }
    }

    public void CloseEscMenu()
    {
        SetUICanvas.Esc.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        popup_escMenu = false;
        MainCam.GetComponent<CameraRot>().popup_escMenu = popup_escMenu;
    }
}
