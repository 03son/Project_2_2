using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Back_Key.ToString();
    }
    protected override void SetChangeKey() //키 저장
    {
        base.SetChangeKey();

        //키 저장
        PlayerPrefs.SetString("BackKey", keyText.text);

        KeyManager.Back_Key = currentKey;
    }
}
