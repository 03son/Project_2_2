using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Run_Key.ToString();
    }
    protected override void SetChangeKey() //Ű ����
    {
        base.SetChangeKey();

        //Ű ����
        PlayerPrefs.SetString("Run_Key", keyText.text);

        KeyManager.Run_Key = currentKey;
    }
    public override void ResetKey()
    {
        base.ResetKey();
        PlayerPrefs.DeleteKey("Run_Key");
        key.LoadKey();
        keyText.text = KeyManager.Run_Key.ToString();
    }
}
