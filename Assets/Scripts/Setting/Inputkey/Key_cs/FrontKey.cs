using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FrontKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Front_Key.ToString();
    }
    protected override void SetChangeKey() //키 저장
    { 
        base.SetChangeKey();
        
        //키 저장
        PlayerPrefs.SetString("FrontKey", keyText.text);

        KeyManager.Front_Key = currentKey;
    }
    public override void ResetKey()
    {
        base.ResetKey();
        PlayerPrefs.DeleteKey("FrontKey");
        key.LoadKey();
        keyText.text = KeyManager.Front_Key.ToString();
    }
}
