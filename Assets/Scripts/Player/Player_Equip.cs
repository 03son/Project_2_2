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
        mouseWheelScroll();
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

            for (int i = 0; i < Inventory.instance.slots.Length; i++)
            {
               /* if (Inventory.instance.slots[i].item == null)
                {
                    invenUtil(index);
                }*/
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
        //등록된 키코드가 눌려있는지 확인
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
        //호출할 키와 기능이 일치하도록 키코드 등록
        const int alphaStart = 48;
        const int alphaEnd = 54;

        int paramValue = 0;
        for (int i = alphaStart; i <= alphaEnd; i++)
        {
            KeyCode tempKeyCode = (KeyCode)i;

            //키 등록
            int index = paramValue;
            keyCodeDic.Add(tempKeyCode, () => selectSlot(index));
            paramValue++;
        }
    }

    void invenUtil(int index)
    {
        //Outline 다 끄고 선택한 번호의 슬롯만 켜기
        for (int i = 0; i < invenSlot.Length; i++)
            invenSlot[i].GetComponent<ItemSlotUI>().equipped = false;

        if (!invenSlot[index - 1].GetComponent<ItemSlotUI>().equipped)
        {
            invenSlot[index - 1].GetComponent<ItemSlotUI>().equipped = true;
            Inventory.instance.selectedItemIndex = index-1;

            //저장 된 아이템 있으면 손에 아이템 들기
            if (inventory.slots[index - 1].item != null)
                setEquipItem(inventory.slots[index - 1].item.name);
            else if (Item != null)
                Destroy(Item);
        }
    }

    void mouseWheelScroll()
    {
        //마우스 휠 스크롤 해서 아이템 선택 바꾸기
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (wheelInput < 0)
        {
            selectIndex -= 1;
            selectIndex = Mathf.Clamp(selectIndex, 0, 6);
            Inventory.instance.selectedItemIndex = selectIndex;
            selectSlot(selectIndex);
            Debug.Log(selectIndex);
            return;
        }
        else if (wheelInput > 0)
        {
            selectIndex += 1;
            selectIndex = Mathf.Clamp(selectIndex, 0, 6);
            Inventory.instance.selectedItemIndex = selectIndex;
            selectSlot(selectIndex);
            Debug.Log(selectIndex);
            return ;
        }
    }
}
