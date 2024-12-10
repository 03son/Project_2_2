using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using Photon.Voice.Unity.UtilityScripts;
using ExitGames.Client.Photon;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class DoubleCheck_UI : UI_Popup
{
    public TextMeshProUGUI Ask_again_text;
    public Button Yes_Button;
    public Button No_Button;

    HashTable playerCP;

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
    private void Update()
    {
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            return;
        }
    }
    void DoubleCheck()
    {
        if (sceneName.name == "Main_Screen")//����ȭ���� ���
            Ask_Text = "������ �����Ͻðڽ��ϱ�?";
        
        if (sceneName.name == GameInfo.InGameScenes) //�ΰ����� ���
            Ask_Text = "������ �����ڽ��ϱ�?";
        
            Ask_again_text.text = Ask_Text;
    }

    void yes_button(PointerEventData button)
    {
        //���ξ� �� ��
        if (sceneName.name == "Main_Screen")
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
            return;
        }

        //�ΰ��� �� ��
        if (sceneName.name == GameInfo.InGameScenes)
        {
            //Debug.Log("���� ������");
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.InRoom)
                {
                    GameInfo.IsGameFinish = false;

                    StartCoroutine(disconnect());

                    Debug.Log("���� ������");
                    PhotonNetwork.Disconnect();
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
    IEnumerator disconnect()
    {
        while (true)
        {
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("Main_Screen");
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
