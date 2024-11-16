using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Left_Key.ToString();
    }
    protected override void SetChangeKey() //키 저장
    {
        base.SetChangeKey();

        //키 저장
        PlayerPrefs.SetString("LeftKey", keyText.text);

        KeyManager.Left_Key = currentKey;
    }
}
