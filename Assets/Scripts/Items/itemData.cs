using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    // ��ȸ��
    Disposable,

    // ������
    Tool
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class itemData : ScriptableObject
{
    [Header("Info")]
    public string ItemName;
    public string Description;
    public int spawnCount;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPerfab;

    // �������� ������ ���� ID
    public string itemID;
}
