using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndingVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer 컴포넌트
    public string NextScene = "Main_Screen";    // 다음 씬 이름

    bool skip = false;
    void Awake()
    {
        switch (GameInfo.endingNumber)
        { 
            case 0: //전원 탈출
                videoPlayer.clip = Resources.Load<VideoClip>($"EndingVideos/전원탈출엔딩");
                break;

            case 1: //일부 생존
                videoPlayer.clip = Resources.Load<VideoClip>($"EndingVideos/일부생존엔딩");
                break;

            case 2: //전원 사망
                videoPlayer.clip = Resources.Load<VideoClip>($"EndingVideos/전원사망엔딩");
                break;
        }
    }
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.prepareCompleted += OnVideoStart;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !skip)
        {
            skip = true;
            return;
        }
    }
    void OnVideoStart(VideoPlayer vp)
    {
        StartCoroutine(Pause());
    }
    void OnVideoEnd(VideoPlayer vp)
    {
        LoadingSceneManager.InGameLoading("Main_Screen", 1);
    }

    IEnumerator Pause()
    {
        yield return new WaitForSecondsRealtime(5f);
        if (!skip)
        {
            videoPlayer.Pause();
        }
        while (true)
        { 
            yield return new WaitForEndOfFrame();
            if (skip)
            {
                videoPlayer.Play();
                yield break;
            }
        }
    }

}
