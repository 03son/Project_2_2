using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Decibel_Bar : MonoBehaviour
{
    public static Decibel_Bar instance;

    Slider decibelBar;
    public float targetValue = 0f;  // 목표 값
    public float smoothTime = 0.3f;  // 부드러워지는 시간
    private float velocity = 0f;  // 현재 속도 (SmoothDamp에서 필요)
    bool inputDecibel = false;
    void Awake()
    {
        instance = this;
        decibelBar = GetComponent<Slider>();
        decibelBar.value = 0;
    }
    void Update()
    {
        //decibelBar.value -= 0.06f;
        decibelBar.value = Mathf.SmoothDamp(decibelBar.value, targetValue, ref velocity, smoothTime);
    }

    public void Decibel_Value(float decibel, bool micDecibel)
    {
        if (!micDecibel && decibelBar.value <= decibel)
        {
            decibelBar.value = decibel;
            return;
        }
        else if(decibelBar.value <= decibel)
        {
            decibelBar.value = decibel;
            return;
        }
    }
}
