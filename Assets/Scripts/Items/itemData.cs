using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    // 일회용
    Disposable,

    // 도구형
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

    // 아이템을 구분할 고유 ID
    public string itemID;
}
