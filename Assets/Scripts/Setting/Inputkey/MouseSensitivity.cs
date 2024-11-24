using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour
{
    [SerializeField] Slider mouseSesitivity;
    void OnEnable()
    {
        mouseSesitivity.onValueChanged.AddListener(SetMouseSensitivity);

        float defaultValue = 0.2f;
        float Val;
        SetMouseSensitivity(Val = PlayerPrefs.HasKey("MouseSensitivity") ? PlayerPrefs.GetFloat("MouseSensitivity") : defaultValue);
        mouseSesitivity.value = Val;
    }

    void SetMouseSensitivity(float volume)
    {
        GameInfo.MouseSensitivity = (int)(volume * 10);
        Debug.Log($"마우스 감도 : {(int)(volume *10)}");
        PlayerPrefs.SetFloat("MouseSensitivity" ,volume);
    }
}
