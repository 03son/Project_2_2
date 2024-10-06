using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public virtual void Init()
    {
        //sorting 안하기 때문에 false
        UIManger.Instance.SetCanvas(gameObject, false);
    }
}
