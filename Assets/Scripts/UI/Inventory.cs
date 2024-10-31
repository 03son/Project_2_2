using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Photon.Pun;

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
    [SerializeField]
    ItemSlot selectedItem;
    public int selectedItemIndex;
    //public TextMeshProUGUI selectedItemName;

    public static Inventory instance;

    public PhotonView pv;
    private void Awake()
    {
        instance = this;

        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            slots = new ItemSlot[ui_itemSlot.Length];

            for (int i = 0; i < slots.Length; i++)
            {
                //UI ���� �ʱ�ȭ �ϱ�
                slots[i] = new ItemSlot();
                //ui_itemSlot[i].index = i;
                ui_itemSlot[i].Clear();
            }

            ClearSelectItemWindows();
        }
    }
    public void Additem(itemData item)
    {
        if (pv.IsMine)
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
    }
    ItemSlot GetEmptySlot()
    {
        if (!pv.IsMine)
            return null;

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
        if (!pv.IsMine)
            return;

        //UI�� ���� �ֽ�ȭ�ϱ�
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
        if (!pv.IsMine)
            return;

        //������ ��������
        if (GetComponent<Player_Equip>().Item != null)
             Destroy(GetComponent<Player_Equip>().Item);
    }

    void RemoveSelectedItem()
    {
        if (!pv.IsMine)
            return;

        if (slots[selectedItemIndex].item != null)
        {
            //���� ���� �������� ���� ���� �������� ��� ����
            if (ui_itemSlot[selectedItemIndex].equipped)
            {
                UnEquip(selectedItemIndex);
                ThrowItem(slots[selectedItemIndex].item);
            }

            //������ ���� �� UI������ ������ ���� �����
            slots[selectedItemIndex].item = null;
            ClearSelectItemWindows();
        }
        UpdateUI();
    }
    void ClearSelectItemWindows()
    {
        if (!pv.IsMine)
            return;

        //������ �ʱ�ȭ
        selectedItem = null;
       // selectedItemName.text = string.Empty;
    }
    void ThrowItem(itemData item)
    {
        if (!pv.IsMine)
            return;

        //������ ������
        //������ ��, ������ ȸ������ ���� ä ������
        Instantiate(item.dropPerfab, dropPos.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                RemoveSelectedItem();
            }
        }
    }
}
