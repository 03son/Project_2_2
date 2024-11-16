using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitDownKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.SitDown_Key.ToString();
    }
    protected override void SetChangeKey() //Ű ����
    {
        base.SetChangeKey();

        //Ű ����
        PlayerPrefs.SetString("SitDown_Key", keyText.text);

        KeyManager.SitDown_Key = currentKey;
    }
    public override void ResetKey()
    {
        base.ResetKey();
        PlayerPrefs.DeleteKey("SitDown_Key");
        key.LoadKey();
        keyText.text = KeyManager.SitDown_Key.ToString();
    }
}
