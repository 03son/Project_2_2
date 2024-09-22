using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GatOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    { 
        return Util.GetOrAddComponent<T>(go);
    }

    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    { 
        UI_Base.AddUIEvent(go, action, type);
    }
}
