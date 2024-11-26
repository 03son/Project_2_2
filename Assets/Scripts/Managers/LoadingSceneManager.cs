using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoadingSceneManager : MonoBehaviour
{
    public static LoadingSceneManager loa;

    static bool hasRunOnce = false;

    [SerializeField] Slider LoadingBar;
    [SerializeField] Image background_LoadingImage;

    [SerializeField] Sprite[] loadingImages;
    [SerializeField] GameObject loadingBarObj;

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
    }

    static IEnumerator acc_LoadScene(string MapName)
    {

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

    static IEnumerator MultiGameLoadScene(string MapName)//��Ƽ
    {
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

        PhotonNetwork.LoadLevel(MapName);
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
    void loadingImage(int MapNum) // �̹���
    {
        background_LoadingImage.GetComponent<Image>().sprite = loadingImages[MapNum];
    }

    //���α׷��� ����Ǹ� �ڵ����� ����, PlayerPrefs���� ����
    void OnApplicationQuit()
    {
        Debug.Log("���� ����");
        PlayerPrefs.SetString("PlayerNickName", null);
    }
}
