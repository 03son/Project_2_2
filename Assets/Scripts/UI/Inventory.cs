using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ItemSlot
{
    public itemData item; // ������ ������
    public int quantity; // ����
}

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] ui_itemSlot = new ItemSlotUI[6]; // UI ���� ������ ����
    public ItemSlot[] slots; //���� ������ ������ ����Ǵ� �迭

    public Transform dropPos;

    [Header("Selected Item")]
    [SerializeField]
    ItemSlot selectedItem;
    public int selectedItemIndex;

    public static Inventory instance;

    public PhotonView pv;

    int addItemIndex;

    private GameObject equippedItemObject; // ���� ������ �������� GameObject�� �����ϴ� ����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        pv = GetComponent<PhotonView>();

        ConnectUi_itemSlot();
    }

    private void Start()
    {
        slots = new ItemSlot[ui_itemSlot.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            ui_itemSlot[i].Clear();
        }

        dropPos = GameObject.Find("ItemDropPos").GetComponent<Transform>();

        ClearSelectItemWindows();
    }

    void ConnectUi_itemSlot()
    {
        ui_itemSlot[0] = GameObject.Find("ItemSlot").GetComponent<ItemSlotUI>();
        ui_itemSlot[1] = GameObject.Find("ItemSlot (1)").GetComponent<ItemSlotUI>();
        ui_itemSlot[2] = GameObject.Find("ItemSlot (2)").GetComponent<ItemSlotUI>();
        ui_itemSlot[3] = GameObject.Find("ItemSlot (3)").GetComponent<ItemSlotUI>();
        ui_itemSlot[4] = GameObject.Find("ItemSlot (4)").GetComponent<ItemSlotUI>();
        ui_itemSlot[5] = GameObject.Find("ItemSlot (5)").GetComponent<ItemSlotUI>();
    }

    public void Additem(itemData item)
    {
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();

            // �������� ȹ���ϰ� ���� Player_Equip�� invenUtil�� ȣ���� ������
            GetComponent<Player_Equip>().invenUtil(addItemIndex + 1);

            // ������ �������� �߰��� �� Flashlight1�� AcquireFlashlight �޼��带 ȣ��
            if (item.ItemName == "Flashlight")
            {
                GameObject flashlightObject = GameObject.Find("Flashlight");
                if (flashlightObject != null)
                {
                    Flashlight1 flashlightScript = flashlightObject.GetComponent<Flashlight1>();
                    if (flashlightScript != null)
                    {
                        flashlightScript.AcquireFlashlight();
                    }
                }
            }

            return;
        }
    }

    public bool HasItem(string itemName)
    {
        // �κ��丮 ���Կ��� ������ Ȯ��
        foreach (var slot in slots)
        {
            if (slot.item != null && slot.item.ItemName == itemName)
            {
                Debug.Log($"�κ��丮�� {itemName}��(��) ����");
                return true;
            }
        }

        // ���� ������ �����۵� Ȯ��
        if (equippedItemObject != null)
        {
            itemData equippedItemData = equippedItemObject.GetComponent<itemData>();
            if (equippedItemData != null && equippedItemData.ItemName == itemName)
            {
                Debug.Log($"������ ���¿��� {itemName}��(��) ����");
                return true;
            }
        }

        Debug.Log($"�κ��丮�� {itemName}��(��) ����");
        return false;
    }

    public void RemoveItem(string itemName)
    {
        // �κ��丮 ���Կ��� ������ ����
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null && slots[i].item.ItemName == itemName)
            {
                slots[i].item = null;
                slots[i].quantity = 0;
                ui_itemSlot[i].Clear();
                UpdateUI();
                return;
            }
        }

        // ������ ������ ����
        if (equippedItemObject != null)
        {
            itemData equippedItemData = equippedItemObject.GetComponent<itemData>();
            if (equippedItemData != null && equippedItemData.ItemName == itemName)
            {
                Destroy(equippedItemObject); // ������ ������ ������Ʈ ����
                equippedItemObject = null;
                Debug.Log($"������ {itemName} ���ŵ�");
            }
        }
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                addItemIndex = i;
                return slots[i];
            }
        }
        return null;
    }

    void UpdateUI()
    {
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
        if (GetComponent<Player_Equip>().Item != null)
            Destroy(GetComponent<Player_Equip>().Item);
    }

    void RemoveSelectedItem()
    {
        if (slots[selectedItemIndex].item != null)
        {
            if (ui_itemSlot[selectedItemIndex].equipped)
            {
                UnEquip(selectedItemIndex);
                ThrowItem(slots[selectedItemIndex].item);
            }

            slots[selectedItemIndex].item = null;
            ClearSelectItemWindows();
        }
        UpdateUI();
    }

    void ClearSelectItemWindows()
    {
        selectedItem = null;
    }

    void ThrowItem(itemData item)
    {
        Instantiate(item.dropPerfab, dropPos.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyManager.Drop_Key))
        {
            RemoveSelectedItem();
        }
    }

    public void EquipItem(GameObject itemObject)
    {
        // EquipItem �޼���� ������ �������� ����
        equippedItemObject = itemObject;
    }
}
