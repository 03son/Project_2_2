using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementKeyFunction : ItemFunction, IItemFunction
{
    public void Function()
    {
        Debug.Log("지하실 열쇠 작동");
    }
}
