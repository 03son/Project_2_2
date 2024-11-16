using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Mic_Key.ToString();
    }
    protected override void SetChangeKey() //Ű ����
    {
        base.SetChangeKey();

        //Ű ����
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
