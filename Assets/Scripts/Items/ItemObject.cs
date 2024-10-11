using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public itemData item;

    void Start()
    {

    }

    public string GetInteractPrompt()
    {
        return string.Format("줍기 {0}", item.ItemName);
    }

    public void OnInteract()
    {
        if (addSlot())
        {
            Inventory.instance.Additem(item);
            Destroy(gameObject);
        }
    }
    bool addSlot()
    {
        //인벤토리에 빈 슬롯이 있는지 확인
        for (int i = 0; i < Inventory.instance.slots.Length; i++)
        {
            if (Inventory.instance.slots[i].item == null)
                return true;
        }
        return false;
    }
}