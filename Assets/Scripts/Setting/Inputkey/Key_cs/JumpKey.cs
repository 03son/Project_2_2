using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Jump_Key.ToString();
    }
    protected override void SetChangeKey() //Ű ����
    {
        base.SetChangeKey();

        //Ű ����
        PlayerPrefs.SetString("Jump_Key", keyText.text);

        KeyManager.Jump_Key = currentKey;
    }
    public override void ResetKey()
    {
        base.ResetKey();
        PlayerPrefs.DeleteKey("Jump_Key");
        key.LoadKey();
        keyText.text = KeyManager.Jump_Key.ToString();
    }
}
