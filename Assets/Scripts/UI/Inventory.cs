using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ItemSlot
{
    public itemData item; // 아이템 데이터
    public int quantity; // 개수
}

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] ui_itemSlot;
    public ItemSlot[] slots;

    public Transform dropPos;

    [Header("Selected Item")]
    [SerializeField]
    ItemSlot selectedItem;
    public int selectedItemIndex;

    public static Inventory instance;

    public PhotonView pv;

    int addItemIndex;

    private GameObject equippedItemObject; // 현재 장착된 아이템의 GameObject를 저장하는 변수

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

            // 아이템을 획득하고 나면 Player_Equip의 invenUtil을 호출해 장착함
            GetComponent<Player_Equip>().invenUtil(addItemIndex + 1);

            // 손전등 아이템을 추가할 때 Flashlight1의 AcquireFlashlight 메서드를 호출
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
        // 인벤토리 슬롯에서 아이템 확인
        foreach (var slot in slots)
        {
            if (slot.item != null && slot.item.ItemName == itemName)
            {
                Debug.Log($"인벤토리에 {itemName}이(가) 있음");
                return true;
            }
        }

        // 현재 장착된 아이템도 확인
        if (equippedItemObject != null)
        {
            itemData equippedItemData = equippedItemObject.GetComponent<itemData>();
            if (equippedItemData != null && equippedItemData.ItemName == itemName)
            {
                Debug.Log($"장착된 상태에서 {itemName}이(가) 있음");
                return true;
            }
        }

        Debug.Log($"인벤토리에 {itemName}이(가) 없음");
        return false;
    }

    public void RemoveItem(string itemName)
    {
        // 인벤토리 슬롯에서 아이템 제거
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

        // 장착된 아이템 제거
        if (equippedItemObject != null)
        {
            itemData equippedItemData = equippedItemObject.GetComponent<itemData>();
            if (equippedItemData != null && equippedItemData.ItemName == itemName)
            {
                Destroy(equippedItemObject); // 장착된 아이템 오브젝트 삭제
                equippedItemObject = null;
                Debug.Log($"장착된 {itemName} 제거됨");
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
        if (Input.GetKeyDown(KeyCode.G))
        {
            RemoveSelectedItem();
        }
    }

    public void EquipItem(GameObject itemObject)
    {
        // EquipItem 메서드로 장착된 아이템을 설정
        equippedItemObject = itemObject;
    }
}
