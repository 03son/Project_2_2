using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Image icon;
    ItemSlot curSlot;
    Outline outline;

    public bool equipped;//장착 중인가?
    void Awake()
    {
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        //선택한 슬롯  outline 켜주기
        selectItem();

    }

    public void selectItem()
    {
        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    //아이템 슬롯 정보 전달
    public void Set(ItemSlot slot)
    { 
        curSlot = slot;
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;
    }
    public void Clear()
    {
        curSlot = null;
        icon.gameObject.SetActive(false);
    }


}
