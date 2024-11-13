using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public interface IResetKey
{ 
    public void ResetKey();
}
public class ResetKey : MonoBehaviour
{
    public Button resetkeyButton;

    IResetKey[] inputKey;
    private void Start()
    {
        resetkeyButton.gameObject.AddUIEvent(resetKey_);

        inputKey = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IResetKey>().ToArray();
    }

    public void resetKey_(PointerEventData button)
    {
        foreach (IResetKey obj in inputKey)
            obj.ResetKey();
        
    }
}
