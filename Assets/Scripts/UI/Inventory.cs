using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using static PlayerState;

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

    private Player_Equip playerEquip; // Player_Equip 참조 변수 추가

    public PhotonView pv;

    int addItemIndex;

    private GameObject equippedItemObject; // ���� ������ �������� GameObject�� �����ϴ� ����

    PlayerState playerState;
    PlayerState.playerState state;
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

        // Player_Equip 초기화
        playerEquip = FindObjectOfType<Player_Equip>();
        if (playerEquip == null)
        {
            Debug.LogError("Player_Equip을 찾을 수 없습니다. equipItem 참조가 실패할 수 있습니다.");
        }
    }


    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (pv.IsMine)
            {
                SetInven();
            }
        }
        else
        {
            SetInven();
        }
    }
    void SetInven()
    {
        ConnectUi_itemSlot();

        slots = new ItemSlot[ui_itemSlot.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            ui_itemSlot[i].Clear();
        }

        dropPos = GameObject.Find("ItemDropPos").GetComponent<Transform>();

        playerState = GetComponent<PlayerState>();

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
        // 인벤토리 슬롯에서 아이템 확인
        foreach (var slot in slots)
        {
            if (slot.item != null && slot.item.ItemName == itemName)
            {
                Debug.Log($"인벤토리에 {itemName}이(가) 있습니다.");
                return true;
            }
        }

        // Player_Equip을 참조해서 equipItem에서 아이템 확인
        Player_Equip playerEquip = FindObjectOfType<Player_Equip>();
        if (playerEquip != null)
        {
            Transform equippedObject = playerEquip.equipItem.transform.Find(itemName);
            if (equippedObject != null)
            {
                ItemObject equippedItem = equippedObject.GetComponent<ItemObject>();
                if (equippedItem != null)
                {
                    Debug.Log($"장착된 상태로 {itemName}이(가) 있습니다.");
                    return true;
                }
            }
        }

        Debug.Log($"인벤토리와 장착된 상태 모두에서 {itemName}이(가) 없습니다.");
        return false;
    }

    public bool HasEquippedItem(string itemName)
    {
        if (playerEquip != null && playerEquip.equipItem != null)
        {
            Transform equippedObject = playerEquip.equipItem.transform.Find(itemName);
            if (equippedObject != null)
            {
                ItemObject equippedItem = equippedObject.GetComponent<ItemObject>();
                if (equippedItem != null)
                {
                    Debug.Log($"{itemName}이(가) 장착되어 있습니다.");
                    return true;
                }
            }
        }
        Debug.Log($"{itemName}이(가) 장착되어 있지 않습니다.");
        return false;
    }


    public void RemoveItem(string itemName)
    {
        // 슬롯에서 아이템 제거
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

        // Player_Equip에서 장착된 아이템 제거
        if (playerEquip != null && playerEquip.equipItem != null)
        {
            ItemObject equippedItem = playerEquip.equipItem.GetComponent<ItemObject>();
            if (equippedItem != null && equippedItem.item.ItemName == itemName)
            {
                Destroy(playerEquip.equipItem);
                playerEquip.equipItem = null;
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
        {
            Destroy(GetComponent<Player_Equip>().Item);
            GetComponent<Player_Equip>().Item = null;

            // 자동으로 다른 슬롯을 선택하지 않음
        }
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
            GetComponent<Player_Equip>().ItemName.text = "";
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
        playerState.GetState(out state);
        if (!CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Survival)
        {
            if (Input.GetKeyDown(KeyManager.Drop_Key))
            {
                RemoveSelectedItem();
            }
        }
    }

    public void EquipItem(GameObject itemObject)
    {
        // EquipItem �޼���� ������ �������� ����
        equippedItemObject = itemObject;
    }

   /* void ThrowItem(itemData item)
    {
        if (PhotonNetwork.IsConnected)
        {
            // PhotonNetwork를 사용해 프리팹 생성 (드롭 위치에)
            PhotonNetwork.Instantiate(item.dropPerfab.name, dropPos.position, Quaternion.identity);
        }
        else
        {
            // 싱글플레이에서는 로컬에서만 생성
            Instantiate(item.dropPerfab, dropPos.position, Quaternion.identity);
        }
    } */

}