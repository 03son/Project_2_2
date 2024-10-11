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
        return string.Format("�ݱ� {0}", item.ItemName);
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
        //�κ��丮�� �� ������ �ִ��� Ȯ��
        for (int i = 0; i < Inventory.instance.slots.Length; i++)
        {
            if (Inventory.instance.slots[i].item == null)
                return true;
        }
        return false;
    }
}