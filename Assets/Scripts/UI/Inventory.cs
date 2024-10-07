using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ItemSlot
{
    public itemData item; //������ ������
    public int quantity; //����
}

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] ui_itemSlot; // UI ���� ������ ����
    public ItemSlot[] slots; //���� ������ ������ ����Ǵ� �迭

    public Transform dropPos; //������ ��� ��ġ

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
            //UI ���� �ʱ�ȭ �ϱ�
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
        //�κ��丮�� ��ĭ�� ���� ��� �� ����
        return;
    }
    ItemSlot GetEmptySlot()
    {
        //�� ���� ã��
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }
        return null;
    }

    void UpdateUI()
    {
        //UI�� ���� �ֽ�ȭ�ϱ�
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
        //������ �ʱ�ȭ
        selectedItem = null;
       // selectedItemName.text = string.Empty;
    }
    public void ThrowItem(itemData item)
    {
        //������ ������
        //������ ��, ������ ȸ������ ���� ä ������
        Instantiate(item.dropPerfab, dropPos.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }
}
