using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Mic_Key.ToString();
    }
    protected override void SetChangeKey() //키 저장
    {
        base.SetChangeKey();

        //키 저장
        PlayerPrefs.SetString("Mic_Key", keyText.text);

        KeyManager.Mic_Key = currentKey;
    }
    public override void ResetKey()
    {
        base.ResetKey();
        PlayerPrefs.DeleteKey("Mic_Key");
        key.LoadKey();
        keyText.text = KeyManager.Mic_Key.ToString();
    }
}
