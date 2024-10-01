using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UI_Button;

public class MainSettingScreen : UI_Popup
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && this.gameObject.activeSelf)
        {
            //메인화면 버튼 4종 활성화
            GameObject.Find("UI_Button").transform.GetChild(1).gameObject.SetActive(true);

            //설정창 닫기
            ClosePopupUI();
        }
    }
}
