using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndingVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer ������Ʈ
    public string NextScene = "Main_Screen";    // ���� �� �̸�

    bool skip = false;
    void Awake()
    {
        switch (GameInfo.endingNumber)
        { 
            case 0: //���� Ż��
                videoPlayer.clip = Resources.Load<VideoClip>($"EndingVideos/����Ż�⿣��");
                break;

            case 1: //�Ϻ� ����
                videoPlayer.clip = Resources.Load<VideoClip>($"EndingVideos/�Ϻλ�������");
                break;

            case 2: //���� ���
                videoPlayer.clip = Resources.Load<VideoClip>($"EndingVideos/�����������");
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
