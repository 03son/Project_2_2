using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Drop_Key.ToString();
    }
    protected override void SetChangeKey() //키 저장
    {
        base.SetChangeKey();

        //키 저장
        PlayerPrefs.SetString("Drop_Key", keyText.text);

        KeyManager.Drop_Key = currentKey;
    }
    public override void ResetKey()
    {
        base.ResetKey();
        PlayerPrefs.DeleteKey("Drop_Key");
        key.LoadKey();
        keyText.text = KeyManager.Drop_Key.ToString();
    }
}
