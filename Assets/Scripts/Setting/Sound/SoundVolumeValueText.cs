using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeValueText : MonoBehaviour
{
    [SerializeField] Slider slider;
    TextMeshProUGUI value;
    void Start()
    {
        value = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        value.text = ((int)(slider.value * 100)).ToString();
    }
}
