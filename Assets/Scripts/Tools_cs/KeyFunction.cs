using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFunction : ItemFunction, IItemFunction
{
    public string keyID;  // ������ ���� ID, Ư�� ���� ������ �� ���

    public void Function()
    {
        Debug.Log("���� �۵�: " + keyID);
    }
}