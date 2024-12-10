using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Decibel_Bar : MonoBehaviour
{
    public static Decibel_Bar instance;

    Slider decibelBar;

    bool inputDecibel = false;
    void Awake()
    {
        instance = this;
        decibelBar = GetComponent<Slider>();
        decibelBar.value = 0;
        StartCoroutine(ResetDecibe());
    }
    void Update()
    {
        decibelBar.value -= 0.06f;
    }

    public void Decibel_Value(float decibel, bool micDecibel)
    {
        inputDecibel = true;
        if (!micDecibel && decibelBar.value <= decibel)
        {
            decibelBar.value = decibel;
            inputDecibel = false;
            return;
        }
        else if(decibelBar.value <= decibel)
        {
            decibelBar.value = decibel;
            inputDecibel = false;
            return;

        }
    }

    IEnumerator ResetDecibe()
    {
        while (inputDecibel)
        {
            yield return new WaitForEndOfFrame();
        }
        while (!inputDecibel)
        {
            yield return new WaitForSecondsRealtime(1f);
            decibelBar.value -= 0.05f;
        }
        StartCoroutine(ResetDecibe());
    }
}
