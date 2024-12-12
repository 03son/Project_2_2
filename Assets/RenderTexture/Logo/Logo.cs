using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Logo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer ÄÄÆ÷³ÍÆ®
    public string NextScene = "Main_Screen";    // ´ÙÀ½ ¾À ÀÌ¸§

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
