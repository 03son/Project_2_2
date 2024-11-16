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

    //int selectIndex = 0;

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
        //Esc 창이 닫혀있을 때만
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu)
        {
            numberKey();
            mouseWheelScroll();
            EquipFunction();
        }
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
        }
    }
    public void numderKeySelectSlot(int index)
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.slots[i].item == null)
            {
               // invenUtil(index); 먹은 아이템이 바로 선택되게 하기 위해 주석처리함
                return;
            }
        }
    }
    public void setEquipItem(string item)
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

    public void invenUtil(int index)
    {
        //Outline 다 끄고 선택한 번호의 슬롯만 켜기
        for (int i = 0; i < invenSlot.Length; i++)
            invenSlot[i].GetComponent<ItemSlotUI>().equipped = false;

        if (!invenSlot[index - 1].GetComponent<ItemSlotUI>().equipped)
        {
            invenSlot[index - 1].GetComponent<ItemSlotUI>().equipped = true;
            inventory.selectedItemIndex = index - 1;

            //저장 된 아이템 있으면 손에 아이템 들기
            if (inventory.slots[index - 1].item != null)
                setEquipItem(inventory.slots[index - 1].item.name);
            else if (Item != null)
                Destroy(Item);
        }
    }

    void EquipFunction()//아이템 기능 작동
    {
        if (Input.GetMouseButtonDown(0) && Item != null)
        {
            Item.GetComponent<IItemFunction>().Function();
        }
    }

    void mouseWheelScroll()
    {
        //마우스 휠 스크롤 해서 아이템 선택 바꾸기
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        int selectedItemIndex = GetComponent<Inventory>().selectedItemIndex;
        if (wheelInput < 0)
        {  
            if (selectedItemIndex < 5)
            {
                selectedItemIndex += 1;
                GetComponent<Inventory>().selectedItemIndex = Mathf.Clamp(selectedItemIndex, 0, 6);
                invenUtil(selectedItemIndex + 1);
                return;
            }
        }
        else if (wheelInput > 0)
        {
            if (selectedItemIndex > 0)
            {
                selectedItemIndex -= 1;
                GetComponent<Inventory>().selectedItemIndex = Mathf.Clamp(selectedItemIndex, 0, 6);
                invenUtil(selectedItemIndex + 1);
                return;
            }
        }
    }
}
