using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class GlobalVolume : MonoBehaviour
{
    public static GlobalVolume globalVolume;

    Volume Volume;
    VolumeProfile volumeProfile;

    ColorAdjustments colorAdjustments;
    void Awake()
    {
        globalVolume = this;

        Volume = GetComponent<Volume>();
        volumeProfile = Volume.profile;
    }
    private void Start()
    {
        SetVolumeEffect();
    }
    public void SetVolumeEffect()
    {
        //밝기 조절
        if (volumeProfile.TryGet(out colorAdjustments))
        {
            //슬라이더 값을 변환함
            colorAdjustments.postExposure.value = Mathf.Lerp(-5f, 2f, PlayerPrefs.GetFloat("brightnessValue"));
        }
    }
}
