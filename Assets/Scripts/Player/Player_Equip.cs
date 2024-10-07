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

    void Start()
    {
        setnumberKey();
        invenSlot[0].GetComponent<ItemSlotUI>().equipped = true;

        inventory = GetComponent<Inventory>();

        equipItem = GameObject.Find("EquipItem");
    }

    void Update()
    {
        numberKey();
    }
    public void selectSlot(int index)
    {
         if (Input.GetKeyDown((KeyCode)(48+index)) && index > 0 &&index < 7)
         {
            invenUtil(index);
            return;
         }

        for (int i = 0; i < Inventory.instance.slots.Length; i++)
        {
            if (Inventory.instance.slots[i].item == null)
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
            //Check if the keycode is pressed
            if (Input.GetKeyDown(entry.Key))
            {
                //Check if the key pressed exist in the dictionary key
                if (keyCodeDic.ContainsKey(entry.Key))
                {
                    //Call the function stored in the Dictionary's value
                    keyCodeDic[entry.Key].Invoke();
                }
            }
        }
    }
    void setnumberKey()
    {
        //ȣ���� Ű�� ����� ��ġ�ϵ��� Ű�ڵ� ���
        const int alphaStart = 48;
        const int alphaEnd = 57;

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

            //���� �� ������ ������ �տ� ������ ���
            if (inventory.slots[index - 1].item != null)
                setEquipItem(inventory.slots[index - 1].item.name);
            else if (Item != null)
                Destroy(Item);
        }
    }
}
