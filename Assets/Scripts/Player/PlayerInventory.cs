using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    // 수집한 아이템을 저장하는 리스트
    public List<Item> inventory = new List<Item>();

    // 아이템을 줍는 메서드
    public void PickUpItem(Item item)
    {
        if (item != null)
        {
            inventory.Add(item); // 아이템을 인벤토리 리스트에 추가
            Debug.Log(item.itemName + " 을(를) 주웠습니다.");
            item.gameObject.SetActive(false); // 아이템을 줍고 나서 씬에서 비활성화
        }
    }
}
