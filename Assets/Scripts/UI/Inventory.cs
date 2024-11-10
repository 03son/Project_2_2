using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Photon.Pun;

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
    [SerializeField]
    ItemSlot selectedItem;
    public int selectedItemIndex;
    //public TextMeshProUGUI selectedItemName;

    public static Inventory instance;

    public PhotonView pv;

    int addItemIndex;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.GetComponent<Inventory>());

        pv = GetComponent<PhotonView>();

        ConnectUi_itemSlot();
    }

    private void Start()
    {
        slots = new ItemSlot[ui_itemSlot.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            //UI 슬롯 초기화 하기
            slots[i] = new ItemSlot();
            //ui_itemSlot[i].index = i;
            ui_itemSlot[i].Clear();
        }

        dropPos = GameObject.Find("ItemDropPos").gameObject.GetComponent<Transform>();

        ClearSelectItemWindows();
    }
    void ConnectUi_itemSlot()
    {
        ui_itemSlot[0] = GameObject.Find("ItemSlot").gameObject.GetComponent<ItemSlotUI>();
        for (int i =1; i<6;i++)
        {
            ui_itemSlot[i] = GameObject.Find($"ItemSlot ({i})").gameObject.GetComponent<ItemSlotUI>();
        }
    }
    public void Additem(itemData item)
    {
        ItemSlot emptyslot = GetEmptySlot();

        if (emptyslot != null)
        {
            emptyslot.item = item;
            emptyslot.quantity = 1;
            UpdateUI();
            GetComponent<Player_Equip>().invenUtil(addItemIndex + 1);
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
            {
                addItemIndex = i;
                return slots[i];
            }
            
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

    void UnEquip(int index)
    {
        //아이템 장착해제
        if (GetComponent<Player_Equip>().Item != null)
        {
            Destroy(GetComponent<Player_Equip>().Item);
        }
    }

    void RemoveSelectedItem()
    {
        if (slots[selectedItemIndex].item != null)
        {
            //만약 버린 아이템이 장착 중인 아이템일 경우 해제
            if (ui_itemSlot[selectedItemIndex].equipped)
            {
                UnEquip(selectedItemIndex);
                ThrowItem(slots[selectedItemIndex].item);
            }

            //아이템 제거 및 UI에서도 아이템 정보 지우기
            slots[selectedItemIndex].item = null;
            ClearSelectItemWindows();
        }
        UpdateUI();
    }
    void ClearSelectItemWindows()
    {
        //아이템 초기화
        selectedItem = null;
       // selectedItemName.text = string.Empty;
    }
    void ThrowItem(itemData item)
    {
        //아이템 버리기
        //버려질 때, 랜덤한 회전값을 가진 채 버리기
        Instantiate(item.dropPerfab, dropPos.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyManager.Drop_Key))
        {
            RemoveSelectedItem();
        }
    }
}
