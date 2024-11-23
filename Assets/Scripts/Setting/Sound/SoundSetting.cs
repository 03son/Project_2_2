using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using TMPro;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class SoundSetting : MonoBehaviour
{
    List<string> options = new List<string>();

    #region ����� �ͼ� ���� ������Ʈ
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private Slider m_MusicMasterSlider;
    [SerializeField] private Slider m_MusicBGMSlider;
    [SerializeField] private Slider m_MusicSFXSlider;
    #endregion

    private void OnEnable()
    {
        AudioMixerController();
    }

    #region ����� �ͼ� ��Ʈ��
    void AudioMixerController()
    {
        m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_MusicBGMSlider.onValueChanged.AddListener(SetBGMVolume);
        m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);

        float defaultValue = 0.2f;
        float Val;
        SetMasterVolume(Val = PlayerPrefs.HasKey("MasterVolume") ? PlayerPrefs.GetFloat("MasterVolume") : defaultValue);
        m_MusicMasterSlider.value = Val;

        SetBGMVolume(Val = PlayerPrefs.HasKey("BGMVolume") ? PlayerPrefs.GetFloat("BGMVolume") : defaultValue);
        m_MusicBGMSlider.value = Val;
        
        SetSFXVolume(Val = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : defaultValue);
        m_MusicSFXSlider.value = Val;
    }
    #endregion
    #region ��ü ����
    void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    #endregion
    #region ��� ����
    void SetBGMVolume(float volume)
    {
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }
    #endregion
    #region ȿ���� ����
    void SetSFXVolume(float volume)
    {
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    #endregion
}
