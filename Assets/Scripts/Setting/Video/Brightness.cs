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

    int brightnessValue;//��� ��

    public Image brightnessImage;

    Scene sceneName;
    private void Start()
    {
        sceneName = SceneManager.GetActiveScene();
    }

    private void OnEnable()
    {
        //����� ���� ������ ǥ�� ������ 80���� ǥ��
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

    void UpdateValueText()// ��ġ ǥ��
    {
        brightnessValue = (int)Mathf.Floor(brightness.value * 100);
        brightnessValueText.text =  $"{brightnessValue}";

        //�ΰ� �̹���
        brightnessImage.color = new Color(brightnessValue, brightnessValue, brightnessValue, Mathf.Lerp(0f, 1f, brightness.value));

        PlayerPrefs.SetFloat("brightnessValue", brightness.value);

        if (sceneName.name == "UI") //�ΰ����̸� ����� ���� �ٷ� ����
        {
            GameObject.Find("Global Volume").GetComponent<GlobalVolume>().SetVolumeEffect();
        }
    }
}
