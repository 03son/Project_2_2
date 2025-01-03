using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public virtual void Init()
    {
        UIManger.Instance.SetCanvas(gameObject, true);
    }
    public virtual void ClosePopupUI()
    { 
        UIManger.Instance.ClosePopupUI(this);
    }
}
