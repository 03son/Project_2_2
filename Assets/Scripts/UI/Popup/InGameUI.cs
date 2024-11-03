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
    GameObject Esc_Menu;
    GameObject MainCam;

    public bool popup_escMenu = false;
    void Start()
    {
        Esc_Menu = GameObject.Find("Esc_Canvas");
        MainCam = GameObject.Find("Main Camera");
        Esc_Menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //esc â�� ���������� ���� ���������� �ݱ�
            Esc_Menu.SetActive(popup_escMenu ? !true : !false);

            //esc â�� ���������� Ŀ�� ���, ���������� Ŀ�� Ȱ��ȭ
            Cursor.lockState = popup_escMenu ? CursorLockMode.Locked : CursorLockMode.None;

            popup_escMenu = !popup_escMenu;
            MainCam.GetComponent<CameraRot>().popup_escMenu = popup_escMenu;
            return;
        }
    }
}
