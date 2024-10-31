using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Photon.Pun;

public class ItemSlot
{
    public itemData item; // 아이템 데이터
    public int quantity; // 개수
}

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] ui_itemSlot; // UI 상의 아이템 슬롯
    public ItemSlot[] slots; // 실제 아이템 슬롯이 저장되는 배열

    public Transform dropPos; // 아이템 드랍 위치

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
            // UI 슬롯 초기화
            slots[i] = new ItemSlot();
            ui_itemSlot[i].Clear();
        }

        dropPos = GameObject.Find("ItemDropPos").gameObject.GetComponent<Transform>();

        ClearSelectItemWindows();
    }

    void ConnectUi_itemSlot()
    {
        // UI 슬롯 연결 (수동으로 연결 중)
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
        // 인벤토리에 빈칸이 없을 경우 못 먹음
    }

    public bool HasKey(string keyID)
    {
        foreach (var slot in slots)
        {
            if (slot.item != null && slot.item.itemID == keyID)  // 열쇠의 itemID가 keyID와 같은지 확인
            {
                return true;
            }
        }
        return false;
    }


    ItemSlot GetEmptySlot()
    {
        // 빈 슬롯 찾기
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }
        return null;
    }

    void UpdateUI()
    {
        // UI의 슬롯 최신화
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
        // 아이템 장착 해제
        if (GetComponent<Player_Equip>().Item != null)
            Destroy(GetComponent<Player_Equip>().Item);
    }

    void RemoveSelectedItem()
    {
        if (slots[selectedItemIndex].item != null)
        {
            // 장착 중인 아이템일 경우 해제
            if (ui_itemSlot[selectedItemIndex].equipped)
            {
                UnEquip(selectedItemIndex);
                ThrowItem(slots[selectedItemIndex].item);
            }

            // 아이템 제거 및 UI에서도 정보 지우기
            slots[selectedItemIndex].item = null;
            ClearSelectItemWindows();
        }
        UpdateUI();
    }

    void ClearSelectItemWindows()
    {
        // 선택된 아이템 초기화
        selectedItem = null;
    }

    void ThrowItem(itemData item)
    {
        // 아이템 버리기
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
