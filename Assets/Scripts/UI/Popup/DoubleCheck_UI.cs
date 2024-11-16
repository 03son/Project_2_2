using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class DoubleCheck_UI : UI_Popup
{
    public TextMeshProUGUI Ask_again_text;
    public Button Yes_Button;
    public Button No_Button;

    Scene sceneName;
    string Ask_Text;
    void Start()
    {
        Yes_Button.gameObject.AddUIEvent(yes_button);
        No_Button.gameObject.AddUIEvent(no_button);
    }
    void OnEnable()
    {
        sceneName = SceneManager.GetActiveScene();
        DoubleCheck();
    }
    void DoubleCheck()
    {
        if (sceneName.name == "Main_Screen")//메인화면일 경우
            Ask_Text = "게임을 종료하시겠습니까?";
        
        if (sceneName.name == "UI") //인게임일 경우
            Ask_Text = "게임을 나가겠습니까?";
        
            Ask_again_text.text = Ask_Text;
    }

    void yes_button(PointerEventData button)
    {
        //메인씬 일 때
        if (sceneName.name == "Main_Screen")
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
            return;
        }

        //인게임 일 때
        if (sceneName.name == "UI")
        {
            Debug.Log("게임 나가기");
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.InRoom)
                {
                    PhotonNetwork.LeaveRoom();
                }
            }
            else
            {
                SceneManager.LoadScene("Main_Screen");
            }
            return;
        }
    }
    void no_button(PointerEventData button)
    {
        ClosePopupUI();
    }
}
