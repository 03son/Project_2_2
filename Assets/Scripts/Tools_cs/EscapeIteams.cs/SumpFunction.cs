using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumpFunction : ItemFunction, IItemFunction
{
    public void Function()
    {
        Debug.Log("기름통 작동");
    }
}
