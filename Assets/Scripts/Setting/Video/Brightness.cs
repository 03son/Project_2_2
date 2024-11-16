using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Brightness : MonoBehaviour
{
    public Slider brightness;
    public TextMeshProUGUI brightnessValueText;

    int brightnessValue;//밝기 값

    public Image brightnessImage;

    Scene sceneName;
    private void Start()
    {
        sceneName = SceneManager.GetActiveScene();
    }

    private void OnEnable()
    {
        //저장된 값이 있으면 표시 없으면 80으로 표시
        if (PlayerPrefs.HasKey("brightnessValue"))
        {
            brightness.value = PlayerPrefs.GetFloat("brightnessValue");
        }
        else
        {
            brightness.value = 0.8f;
            PlayerPrefs.SetFloat("brightnessValue", brightness.value);
        }
        brightnessImage.color = new Color(255, 255, 255, Mathf.Lerp(0, 1, brightness.value));
    }

    // Update is called once per frame
    void Update()
    {
        UpdateValueText();
    }

    void UpdateValueText()// 수치 표시
    {
        brightnessValue = (int)Mathf.Floor(brightness.value * 100);
        brightnessValueText.text =  $"{brightnessValue}";

        //로고 이미지
        brightnessImage.color = new Color(brightnessValue, brightnessValue, brightnessValue, Mathf.Lerp(0f, 1f, brightness.value));

        PlayerPrefs.SetFloat("brightnessValue", brightness.value);

        if (sceneName.name == "UI") //인게임이면 변경된 값을 바로 적용
        {
            GameObject.Find("Global Volume").GetComponent<GlobalVolume>().SetVolumeEffect();
        }
    }
}
