using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : Singleton<LoadingSceneManager>
{
    static LoadingSceneManager loa;

    public Image LoadingBar;
    public Image background_LoadingImage;

    public Sprite[] loadingImages;
    public GameObject loadingBarObj;

    public static void InGameLoading(string MapName, int MapNum)
    {
        loa.StartCoroutine(acc_LoadScene(MapName));
        loa.loadingImage(MapNum - 1);
    }

    static IEnumerator acc_LoadScene(string MapName)
    {
        SceneManager.LoadScene("InGameMapLoading", LoadSceneMode.Single);

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
                loa.LoadingBar.fillAmount = (op.progress) * 1 / 100;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                loa.LoadingBar.fillAmount = Mathf.Lerp(0.1f, 1f, timer);
                if (loa.LoadingBar.fillAmount >= 1f)
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
    }

    void loadingImage(int MapNum) // ¿ÃπÃ¡ˆ
    {
        background_LoadingImage.GetComponent<Image>().sprite = loadingImages[MapNum];
    }
}
