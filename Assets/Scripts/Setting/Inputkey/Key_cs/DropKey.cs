using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Drop_Key.ToString();
    }
    protected override void SetChangeKey() //Ű ����
    {
        base.SetChangeKey();

        //Ű ����
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
