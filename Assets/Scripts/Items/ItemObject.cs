using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
   public itemData item;

    public string GetInteractPrompt()
    {
        return string.Format("ащ╠Б {0}", item.ItemName);
    }

    public void OnInteract()
    {
        Inventory.instance.Additem(item);
        Destroy(gameObject);
    }
}
