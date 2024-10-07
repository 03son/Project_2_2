using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Image icon;
    ItemSlot curSlot;
    Outline outline;

    public bool equipped;//���� ���ΰ�?
    void Awake()
    {
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        //������ ����  outline ���ֱ�
        selectItem();

    }

    public void selectItem()
    {
        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    //������ ���� ���� ����
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
