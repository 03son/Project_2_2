using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Player_Equip : MonoBehaviour
{
    Dictionary<KeyCode, System.Action> keyCodeDic = new Dictionary<KeyCode, System.Action>();

    public ItemSlotUI[] invenSlot = new ItemSlotUI[6];

    public GameObject equipItem;

    public GameObject Item;

    Inventory inventory;

    ResourceManager resoure = new ResourceManager();

    int selectIndex = 0;

    PhotonView pv;
    void Start()
    {
        pv = GetComponent<PhotonView>();

        ConnectUi_itemSlot();

        setnumberKey();
        invenSlot[0].GetComponent<ItemSlotUI>().equipped = true;

        inventory = GetComponent<Inventory>();

        equipItem = GameObject.Find("EquipItem").gameObject;
    }

    void Update()
    {
        numberKey();
        mouseWheelScroll();
        EquipFunction();
    }
    void ConnectUi_itemSlot()
    {
        invenSlot[0] = GameObject.Find("ItemSlot").gameObject.GetComponent<ItemSlotUI>();
        invenSlot[1] = GameObject.Find("ItemSlot (1)").gameObject.GetComponent<ItemSlotUI>();
        invenSlot[2] = GameObject.Find("ItemSlot (2)").gameObject.GetComponent<ItemSlotUI>();
        invenSlot[3] = GameObject.Find("ItemSlot (3)").gameObject.GetComponent<ItemSlotUI>();
        invenSlot[4] = GameObject.Find("ItemSlot (4)").gameObject.GetComponent<ItemSlotUI>();
        invenSlot[5] = GameObject.Find("ItemSlot (5)").gameObject.GetComponent<ItemSlotUI>();
    }
    public void selectSlot(int index)
    {
        if (index != 0 && index <= 6)
        {
            if (Input.GetKeyDown((KeyCode)(48 + index)))
            {
                invenUtil(index);
                return;
            }

            invenUtil(index);
        }
    }
    public void numderKeySelectSlot(int index)
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.slots[i].item == null)
            {
                invenUtil(index);
            }
        }
    }
    void setEquipItem(string item)
    {
        
        if (Item != null)
            Destroy(Item);
        

        Item = resoure.Instantiate($"Items/{item}");
        Item.layer = LayerMask.NameToLayer("Equip");
        Item.transform.SetParent(equipItem.transform);

        Item.transform.localPosition = Vector3.zero;
        Item.transform.localRotation = Quaternion.identity;
    }
    void numberKey()
    {
        //��ϵ� Ű�ڵ尡 �����ִ��� Ȯ��
        foreach (KeyValuePair<KeyCode, System.Action> entry in keyCodeDic)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                if (keyCodeDic.ContainsKey(entry.Key))
                {
                    keyCodeDic[entry.Key].Invoke();
                }
            }
        }
    }
    void setnumberKey()
    {
        //ȣ���� Ű�� ����� ��ġ�ϵ��� Ű�ڵ� ���
        const int alphaStart = 48;
        const int alphaEnd = 54;

        int paramValue = 0;
        for (int i = alphaStart; i <= alphaEnd; i++)
        {
            KeyCode tempKeyCode = (KeyCode)i;

            //Ű ���
            int index = paramValue;
            keyCodeDic.Add(tempKeyCode, () => selectSlot(index));
            paramValue++;
        }
    }

    void invenUtil(int index)
    {
        //Outline �� ���� ������ ��ȣ�� ���Ը� �ѱ�
        for (int i = 0; i < invenSlot.Length; i++)
            invenSlot[i].GetComponent<ItemSlotUI>().equipped = false;

        if (!invenSlot[index - 1].GetComponent<ItemSlotUI>().equipped)
        {
            invenSlot[index - 1].GetComponent<ItemSlotUI>().equipped = true;
            inventory.selectedItemIndex = index - 1;

            //���� �� ������ ������ �տ� ������ ���
            if (inventory.slots[index - 1].item != null)
                setEquipItem(inventory.slots[index - 1].item.name);
            else if (Item != null)
                Destroy(Item);
        }
    }

    void EquipFunction()
    {
        if (Input.GetMouseButtonDown(0) && Item != null)
        {
            Item.GetComponent<IItemFunction>().Function();
        }
    }

    void mouseWheelScroll()
    {
        //���콺 �� ��ũ�� �ؼ� ������ ���� �ٲٱ�
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (wheelInput < 0)
        {
            selectIndex += 1;
            selectIndex = Mathf.Clamp(selectIndex, 0, 6);
            inventory.selectedItemIndex = selectIndex;
            selectSlot(selectIndex);
            return;
        }
        else if (wheelInput > 0)
        {
            selectIndex -= 1;
            selectIndex = Mathf.Clamp(selectIndex, 0, 6);
            inventory.selectedItemIndex = selectIndex;
            selectSlot(selectIndex);
            return;
        }
    }
}
