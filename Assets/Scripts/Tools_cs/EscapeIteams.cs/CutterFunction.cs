using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterFunction : ItemFunction, IItemFunction
{
    public void Function()
    {
        Debug.Log("절단기 작동");
    }
}
