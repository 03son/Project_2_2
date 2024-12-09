using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using HashTable = ExitGames.Client.Photon.Hashtable;
using static Player_RoomInfo;
using Photon.Realtime;
using TMPro;

public class LoadingSceneManager : MonoBehaviourPunCallbacks
{
    public static LoadingSceneManager loa;

    static bool hasRunOnce = false;

    [SerializeField] Slider LoadingBar;
   // [SerializeField] Image background_LoadingImage;

    [SerializeField] Sprite[] loadingImages;
    [SerializeField] GameObject loadingBarObj;

    [SerializeField] TextMeshProUGUI loadingText;

    HashTable roomCP;
    HashTable playerCP;

    public int loadcompletePlayer = 0;

    public bool startGame = false;

    private void Awake()
    {
        if (loa == null)
        {
            loa = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        if (hasRunOnce) //���� ���� �� ���� 1���� ����
        {
            hasRunOnce = true;
            Debug.Log("���� ����");
            PlayerPrefs.SetString("PlayerNickName", null);
        }    
    }
    private void Start()
    {
        //�ε� ĵ���� ��Ȱ��ȭ
        loadingBarObj.SetActive(false);
    }
    public static void InGameLoading(string MapName, int ImageNumber)
    {
        loa.LoadingBar.value = 0;

        if (PhotonNetwork.IsConnected)
            loa.StartCoroutine(MultiGameLoadScene(MapName));//��Ƽ
        else
            loa.StartCoroutine(acc_LoadScene(MapName));//�̱�

        loa.loadingImage(ImageNumber - 1);

        loa.StartCoroutine(LoadingText());
    }
    #region �̱�
    static IEnumerator acc_LoadScene(string MapName)
    {
        if (PhotonNetwork.IsConnected)
        {
            loa.roomCP = PhotonNetwork.CurrentRoom.CustomProperties;
        }

        if (!loa.loadingBarObj.activeSelf)
        {
            loa.loadingBarObj.SetActive(true);
        }

        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(MapName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.1f)
            {
                loa.LoadingBar.value = (op.progress) * 1 / 100;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                loa.LoadingBar.value = Mathf.Lerp(0.1f, 1f, timer);
                if (loa.LoadingBar.value >= 1f)
                {
                    op.allowSceneActivation = true;

                    yield return null;
                    if (loa.loadingBarObj.activeSelf)
                    {
                        loa.loadingBarObj.SetActive(false);
                    }
                    yield break;
                }
            }
        }
        yield return null;
    }//�̱�
    #endregion
    #region ��Ƽ
    static IEnumerator MultiGameLoadScene(string MapName)//��Ƽ
    {
        loa.PlayerNotReady();

        loa.startGame = false;
        loa.roomCP = new HashTable() { { "startGame", loa.startGame } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(loa.roomCP);

        if (!loa.loadingBarObj.activeSelf)
            loa.loadingBarObj.SetActive(true);

        yield return null;

        float loadingProgress = 0f;
        while (loadingProgress < 1f)
        {
            yield return new WaitForSecondsRealtime(0.01f);

            loadingProgress += 0.01f;
            loa.LoadingBar.value = loadingProgress;
        }
        if (GameInfo.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(MapName);

            loa.startGame = true;
            loa.roomCP = new HashTable() { { "startGame", loa.startGame } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(loa.roomCP);
        }
        while (!loa.startGame)//������ ���� �ε��� ������ ���
        {
            yield return new WaitForSecondsRealtime(0.01f);
            Debug.Log(PhotonNetwork.LevelLoadingProgress);
        }
        while (loa.startGame)
        {
            yield return new WaitForSecondsRealtime(0.001f);
            if (GameInfo.IsMasterClient)
            {
                while (PhotonNetwork.LevelLoadingProgress < 1f)
                {
                    yield return new WaitForSecondsRealtime(0.001f);
                    if (PhotonNetwork.LevelLoadingProgress >= 1f)
                    {
                        loa.LoadingBar.value = 1;
                        yield return null;
                        if (loa.loadingBarObj.activeSelf)
                        {
                            loa.loadingBarObj.SetActive(false);
                        }
                        yield break;
                    }
                }
            }
            else
            {
                loa.LoadingBar.value = 1;
                yield return null;
                loa.loadingBarObj.SetActive(false);
                yield break;
            }
        }
    }
    #endregion
    void loadingImage(int MapNum) // �̹���
    {
       // background_LoadingImage.GetComponent<Image>().sprite = loadingImages[MapNum];
    }

    public static IEnumerator LoadingText()
    {
        for (int i = 0; i<5; i++)
        {
            string repeatedcom = new string('.', i);
            loa.loadingText.text = $"Loading{repeatedcom}";
            yield return new WaitForSecondsRealtime(0.3f);
           
            if (i == 4)
                i = 0;
        }
    }

    public void PlayerNotReady()//�κ񿡼� �غ� �ߴ� �� F�� ����
    {
        playerCP = new HashTable() { { "isReady", false } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);
    }

    public override void OnRoomPropertiesUpdate(HashTable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("startGame"))
        {
           loa.startGame = (bool)propertiesThatChanged["startGame"];
        }
    }

    //���α׷��� ����Ǹ� �ڵ����� ����, PlayerPrefs���� ����
    void OnApplicationQuit()
    {
        Debug.Log("���� ����");
        GameInfo.IsGameFinish = false;
        PlayerPrefs.SetString("PlayerNickName", null);
    }
}
