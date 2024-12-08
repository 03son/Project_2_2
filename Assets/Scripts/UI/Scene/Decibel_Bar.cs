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
    }
    void Update()
    {
        decibelBar.value -= 0.06f;
    }

    public void Decibel_Value(float decibel)
    {
        decibelBar.value = decibel;
    }
}
