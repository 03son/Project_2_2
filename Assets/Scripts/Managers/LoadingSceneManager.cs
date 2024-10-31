using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoadingSceneManager : MonoBehaviour
{
    public static LoadingSceneManager loa;

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
    }
    private void Start()
    {
        //로딩 캔버스 비활성화
        loadingBarObj.SetActive(false);
    }
    public static void InGameLoading(string MapName, int ImageNumber)
    {
        if (PhotonNetwork.IsConnected)
            loa.StartCoroutine(MultiGameLoadScene(MapName));//멀티
        else
            loa.StartCoroutine(acc_LoadScene(MapName));//싱글

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
    }//싱글

    static IEnumerator MultiGameLoadScene(string MapName)//멀티
    {
        if (!loa.loadingBarObj.activeSelf)
            loa.loadingBarObj.SetActive(true);
        
        yield return null;

        PhotonNetwork.LoadLevel(MapName);

        float timer = 0f;
        while (PhotonNetwork.LevelLoadingProgress < 1f)
        {
            yield return new WaitForEndOfFrame();
            if (PhotonNetwork.LevelLoadingProgress < 0.1f)
            {
                loa.LoadingBar.value = (PhotonNetwork.LevelLoadingProgress) * 1/100;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                loa.LoadingBar.value = Mathf.Lerp(0.1f , 1f ,timer);
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

    }
    void loadingImage(int MapNum) // 이미지
    {
        background_LoadingImage.GetComponent<Image>().sprite = loadingImages[MapNum];
    }
}
