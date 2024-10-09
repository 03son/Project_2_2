using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerFunction : ItemFunction, IItemFunction
{
    public void Function()
    {
        Debug.Log("프로펠러 작동");
    }
}
