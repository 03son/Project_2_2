using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFunction : ItemFunction, IItemFunction
{
    public string keyID;  // 열쇠의 고유 ID, 특정 문과 연결할 때 사용

    public void Function()
    {
        Debug.Log("열쇠 작동: " + keyID);
    }
}