using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    // 아이템 프리팹에 부착하여 직접 소환 수량를 조절할 예정
    public GameObject itemPrefab; // 소환할 아이템 프리팹
    public int spawnCount;        // 소환할 아이템의 개수
}
