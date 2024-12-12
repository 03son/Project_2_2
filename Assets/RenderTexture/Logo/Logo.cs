using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Logo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer ������Ʈ
    public string NextScene = "Main_Screen";    // ���� �� �̸�

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(NextScene);
    }
}
