using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ItemSlot
{
    public itemData item; //아이템 데이터
    public int quantity; //개수
}

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] ui_itemSlot; // UI 상의 아이템 슬롯
    public ItemSlot[] slots; //실제 아이템 슬롯이 저장되는 배열

    public Transform dropPos; //아이템 드랍 위치

    [Header("Selected Item")]
    ItemSlot selectedItem;
    int selectedItemIndex;
    //public TextMeshProUGUI selectedItemName;

    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        slots = new ItemSlot[ui_itemSlot.Length];

        for (int i = 0; i< slots.Length; i++)
        {
            //UI 슬롯 초기화 하기
            slots[i] = new ItemSlot();
            //ui_itemSlot[i].index = i;
            ui_itemSlot[i].Clear();
        }

        ClearSelectItemWindows();
    }
    public void Additem(itemData item)
    {
        ItemSlot emptyslot = GetEmptySlot();

        if (emptyslot != null)
        {
            emptyslot.item = item;
            emptyslot.quantity = 1;
            UpdateUI();
            return;
        }
        //인벤토리에 빈칸이 없을 경우 못 먹음
        return;
    }
    ItemSlot GetEmptySlot()
    {
        //빈 슬롯 찾기
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }
        return null;
    }

    void UpdateUI()
    {
        //UI의 슬롯 최신화하기
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                ui_itemSlot[i].Set(slots[i]);
            else
                ui_itemSlot[i].Clear();
        }
    }
    void ClearSelectItemWindows()
    { 
        //아이템 초기화
        selectedItem = null;
       // selectedItemName.text = string.Empty;
    }
    public void ThrowItem(itemData item)
    {
        //아이템 버리기
        //버려질 때, 랜덤한 회전값을 가진 채 버리기
        Instantiate(item.dropPerfab, dropPos.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }
}
