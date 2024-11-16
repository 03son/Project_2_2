using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionKey : InputKeySetting
{
    void OnEnable()
    {
        keyText.text = KeyManager.Interaction_Key.ToString();
    }
    protected override void SetChangeKey() //키 저장
    {
        base.SetChangeKey();

        //키 저장
        PlayerPrefs.SetString("Interaction_Key", keyText.text);

        KeyManager.Interaction_Key = currentKey;
    }

    public override void ResetKey()
    {
        base.ResetKey();
        PlayerPrefs.DeleteKey("Interaction_Key");
        key.LoadKey();
        keyText.text = KeyManager.Interaction_Key.ToString();
    }
}
