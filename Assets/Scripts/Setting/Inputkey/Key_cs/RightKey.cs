using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Right_Key.ToString();
    }
    protected override void SetChangeKey() //키 저장
    {
        base.SetChangeKey();

        //키 저장
        PlayerPrefs.SetString("RightKey", keyText.text);

        KeyManager.Right_Key = currentKey;
    }
    public override void ResetKey()
    {
        base.ResetKey();
        PlayerPrefs.DeleteKey("RightKey");
        key.LoadKey();
        keyText.text = KeyManager.Right_Key.ToString();
    }
}
