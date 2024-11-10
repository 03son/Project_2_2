using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Back_Key.ToString();
    }
    protected override void SetChangeKey() //Ű ����
    {
        base.SetChangeKey();

        //Ű ����
        PlayerPrefs.SetString("BackKey", keyText.text);

        KeyManager.Back_Key = currentKey;
    }
}
