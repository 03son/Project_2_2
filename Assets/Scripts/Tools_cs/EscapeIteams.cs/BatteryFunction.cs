using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryFunction : ItemFunction, IItemFunction
{
    public void Function()
    {
        Debug.Log("배터리 작동");
    }
}
