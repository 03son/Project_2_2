using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Left_Key.ToString();
    }
    protected override void SetChangeKey() //Ű ����
    {
        base.SetChangeKey();

        //Ű ����
        PlayerPrefs.SetString("LeftKey", keyText.text);

        KeyManager.Left_Key = currentKey;
    }
    public override void ResetKey()
    {
        base.ResetKey();
        PlayerPrefs.DeleteKey("LeftKey");
        key.LoadKey();
        keyText.text = KeyManager.Left_Key.ToString();
    }
}
