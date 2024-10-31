using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Photon.Pun;

public class ItemSlot
{
    public itemData item; // ������ ������
    public int quantity; // ����
}

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] ui_itemSlot; // UI ���� ������ ����
    public ItemSlot[] slots; // ���� ������ ������ ����Ǵ� �迭

    public Transform dropPos; // ������ ��� ��ġ

    [Header("Selected Item")]
    [SerializeField]
    ItemSlot selectedItem;
    public int selectedItemIndex;

    public static Inventory instance;

    public PhotonView pv;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.GetComponent<Inventory>());
        }

        pv = GetComponent<PhotonView>();

        ConnectUi_itemSlot();
    }

    private void Start()
    {
        slots = new ItemSlot[ui_itemSlot.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            // UI ���� �ʱ�ȭ
            slots[i] = new ItemSlot();
            ui_itemSlot[i].Clear();
        }

        dropPos = GameObject.Find("ItemDropPos").gameObject.GetComponent<Transform>();

        ClearSelectItemWindows();
    }

    void ConnectUi_itemSlot()
    {
        // UI ���� ���� (�������� ���� ��)
        ui_itemSlot[0] = GameObject.Find("ItemSlot").gameObject.GetComponent<ItemSlotUI>();
        ui_itemSlot[1] = GameObject.Find("ItemSlot (1)").gameObject.GetComponent<ItemSlotUI>();
        ui_itemSlot[2] = GameObject.Find("ItemSlot (2)").gameObject.GetComponent<ItemSlotUI>();
        ui_itemSlot[3] = GameObject.Find("ItemSlot (3)").gameObject.GetComponent<ItemSlotUI>();
        ui_itemSlot[4] = GameObject.Find("ItemSlot (4)").gameObject.GetComponent<ItemSlotUI>();
        ui_itemSlot[5] = GameObject.Find("ItemSlot (5)").gameObject.GetComponent<ItemSlotUI>();
    }

    public void Additem(itemData item)
    {
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }
        // �κ��丮�� ��ĭ�� ���� ��� �� ����
    }

    public bool HasKey(string keyID)
    {
        foreach (var slot in slots)
        {
            if (slot.item != null && slot.item.itemID == keyID)  // ������ itemID�� keyID�� ������ Ȯ��
            {
                return true;
            }
        }
        return false;
    }


    ItemSlot GetEmptySlot()
    {
        // �� ���� ã��
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }
        return null;
    }

    void UpdateUI()
    {
        // UI�� ���� �ֽ�ȭ
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                ui_itemSlot[i].Set(slots[i]);
            else
                ui_itemSlot[i].Clear();
        }
    }

    void UnEquip(int index)
    {
        // ������ ���� ����
        if (GetComponent<Player_Equip>().Item != null)
            Destroy(GetComponent<Player_Equip>().Item);
    }

    void RemoveSelectedItem()
    {
        if (slots[selectedItemIndex].item != null)
        {
            // ���� ���� �������� ��� ����
            if (ui_itemSlot[selectedItemIndex].equipped)
            {
                UnEquip(selectedItemIndex);
                ThrowItem(slots[selectedItemIndex].item);
            }

            // ������ ���� �� UI������ ���� �����
            slots[selectedItemIndex].item = null;
            ClearSelectItemWindows();
        }
        UpdateUI();
    }

    void ClearSelectItemWindows()
    {
        // ���õ� ������ �ʱ�ȭ
        selectedItem = null;
    }

    void ThrowItem(itemData item)
    {
        // ������ ������
        Instantiate(item.dropPerfab, dropPos.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            RemoveSelectedItem();
        }
    }
}
