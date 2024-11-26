using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_Equip : MonoBehaviour
{
    Dictionary<KeyCode, System.Action> keyCodeDic = new Dictionary<KeyCode, System.Action>();

    public ItemSlotUI[] invenSlot = new ItemSlotUI[6];
    public GameObject equipItem;
    public GameObject Item;
    public TextMeshProUGUI ItemName;
    Inventory inventory;

    ResourceManager resoure = new ResourceManager();
    int selectIndex = 0;
    PhotonView pv;

    [Header("Throw Settings")]
    public KeyCode throwKey = KeyCode.Mouse0;  // 던지기 키
    public float throwForce = 10f;  // 기본 던지는 힘
    public float maxForce = 20f;  // 최대 던지는 힘
    public LineRenderer trajectoryLine;  // 궤적 표시 LineRenderer

    private bool isCharging = false;
    private float chargeTime = 0f;
    private bool hasGlassCup = false;
    private GameObject currentGlassCup;
    private Animator animator;           // Animator 추가
    private CharacterController characterController;
    private bool isFlashlightOn = false; // 손전등 상태를 저장할 변수

    void Start()
    {
        pv = GetComponent<PhotonView>();

        ConnectUi_itemSlot();
        setnumberKey();
        invenSlot[0].GetComponent<ItemSlotUI>().equipped = true;

        inventory = GetComponent<Inventory>();
        equipItem = GameObject.Find("EquipItem").gameObject;
        ItemName = GameObject.Find("ItemName_Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        ItemName.text = "";

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;  // 궤적 표시 초기화
        }

        // Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator 컴포넌트를 찾을 수 없습니다. 플레이어 오브젝트에 Animator가 있어야 합니다.");
        }
    }

    void Update()
    {
        numberKey();
        mouseWheelScroll();
        EquipFunction();

        // 던지는 동작 처리 (유리컵 충전 던지기)
        if (isCharging)
        {
            ChargeThrow();
        }

        // 던지기 종료 처리
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            ReleaseThrow();
        }
        if (isCharging && Input.GetMouseButtonDown(1)) // 마우스 우클릭 시
        {
            CancelThrow();
        }
        // 아이템 장착 상태에 따라 애니메이션 파라미터 설정
        bool isHoldingAnyItem = HasAnyEquippedItem();
        animator.SetBool("isHoldingItem", isHoldingAnyItem);
    }

    void ConnectUi_itemSlot()
    {
        for (int i = 0; i < invenSlot.Length; i++)
        {
            string slotName = i == 0 ? "ItemSlot" : $"ItemSlot ({i})";
            GameObject slotObject = GameObject.Find(slotName);

            if (slotObject != null)
            {
                invenSlot[i] = slotObject.GetComponent<ItemSlotUI>();
            }
            else
            {
                Debug.LogError($"'{slotName}'을(를) 찾을 수 없습니다. 계층 구조를 확인해 주세요.");
            }
        }
    }

    public void selectSlot(int index)
    {
        if (index > 0 && index <= invenSlot.Length)
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

        // 유리컵을 장착한 경우 던질 수 있도록 설정
        if (item == "GlassCup")
        {
            hasGlassCup = true;
            currentGlassCup = Item;
        }
    }

    void numberKey()
    {
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
        const int alphaStart = 48;
        const int alphaEnd = 54;

        int paramValue = 0;
        for (int i = alphaStart; i <= alphaEnd; i++)
        {
            KeyCode tempKeyCode = (KeyCode)i;
            int index = paramValue;
            keyCodeDic.Add(tempKeyCode, () => selectSlot(index));
            paramValue++;
        }
    }

    public void invenUtil(int index)
    {
        for (int i = 0; i < invenSlot.Length; i++)
            invenSlot[i].GetComponent<ItemSlotUI>().equipped = false;

        if (!invenSlot[index - 1].GetComponent<ItemSlotUI>().equipped)
        {
            invenSlot[index - 1].GetComponent<ItemSlotUI>().equipped = true;
            inventory.selectedItemIndex = index - 1;

            if (inventory.slots[index - 1].item != null)
            {
                setEquipItem(inventory.slots[index - 1].item.name);
                inventory.EquipItem(Item); // 장착된 아이템을 Inventory에도 반영

                //아이템 이름 출력
                ItemName.text = inventory.slots[index - 1].item.ItemName;
            }
            else if (Item != null)
            {
                Destroy(Item);
                ItemName.text = "";
            }
        }
    }

    void EquipFunction()
    {
        // 마우스 좌클릭으로 모든 아이템 작동
        if (Input.GetMouseButtonDown(0) && Item != null)
        {
            // 손전등 아이템 처리
            if (Item.name == "Flashlight")
            {
                Flashlight1 flashlightScript = Item.GetComponent<Flashlight1>();
                if (flashlightScript != null)
                {
                    Debug.Log("손전등 획득 및 사용");
                    flashlightScript.AcquireFlashlight();

                    // 손전등 켜기/끄기 처리
                    isFlashlightOn = !isFlashlightOn;
                    flashlightScript.ToggleFlashlight(isFlashlightOn);

                    // 애니메이션 파라미터 설정
                    animator.SetBool("isFlashlightOn", isFlashlightOn);
                }
            }
            // 유리컵 아이템 처리
            else if (hasGlassCup && currentGlassCup != null && currentGlassCup.name == "GlassCup")
            {
                StartThrowing();
            }
            // 다른 아이템 처리
            else
            {
                // IItemFunction 인터페이스를 가진 아이템 처리
                IItemFunction itemFunction = Item.GetComponent<IItemFunction>();
                if (itemFunction != null)
                {
                    Debug.Log($"{Item.name} 아이템 기능 실행");
                    itemFunction.Function();
                }
            }
        }
    }





    void StartThrowing()
    {
        isCharging = true;
        chargeTime = 0f;

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = true;  // 궤적 표시 활성화
        }
    }

    void ChargeThrow()
    {
        isCharging = true;
        chargeTime += Time.deltaTime;
        float currentForce = Mathf.Min(chargeTime * throwForce, maxForce);

        // 애니메이터 파라미터 설정: 충전 중인 상태
        animator.SetBool("isChargingThrow", true);

        // 궤적 업데이트
        Vector3 throwDirection = Camera.main.transform.forward;
        Vector3 grenadeVelocity = throwDirection * currentForce;
        ShowTrajectory(Camera.main.transform.position + Camera.main.transform.forward, grenadeVelocity);
    }

    void ReleaseThrow()
    {
        if (currentGlassCup != null)
        {
            // 부모-자식 관계 해제
            currentGlassCup.transform.parent = null;

            float finalForce = Mathf.Min(chargeTime * throwForce, maxForce);
            Rigidbody rb = currentGlassCup.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;   // 던질 때 물리적으로 움직이도록 설정
                rb.useGravity = true;   // 중력 활성화
                Vector3 throwDirection = Camera.main.transform.forward;
                rb.AddForce(throwDirection * finalForce, ForceMode.VelocityChange);
            }

            // 현재 유리컵의 itemData 가져오기
            itemData cupItemData = currentGlassCup.GetComponent<ItemObject>().item;

            // 인벤토리에서 아이템 제거
            if (cupItemData != null)
            {
                Inventory.instance.RemoveItem(cupItemData.ItemName);
            }

            hasGlassCup = false;  // 던진 후 유리컵 소지 상태 해제
            currentGlassCup = null;  // 유리컵 참조 해제

            if (trajectoryLine != null)
            {
                trajectoryLine.enabled = false;  // 궤적 표시 비활성화
            }
        }

        // 충전 중 상태 해제
        animator.SetBool("isChargingThrow", false);

        // 던지기 애니메이션 트리거 설정
        animator.SetTrigger("isThrowing");

        isCharging = false;
    }


    void CancelThrow()
    {
        isCharging = false;
        chargeTime = 0f;

        // 충전 상태 해제
        animator.SetBool("isChargingThrow", false);

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;  // 궤적 표시 비활성화
        }

        Debug.Log("던지기 취소됨");
    }





    void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        if (trajectoryLine == null) return;

        int numPoints = 100;
        Vector3[] points = new Vector3[numPoints];
        trajectoryLine.positionCount = numPoints;

        for (int i = 0; i < numPoints; i++)
        {
            float time = i * 0.1f;
            points[i] = origin + speed * time + 0.5f * Physics.gravity * time * time;
        }

        trajectoryLine.SetPositions(points);
    }
    void mouseWheelScroll()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (wheelInput < 0)
        {
            if (selectIndex < 5)
            {
                selectIndex += 1;
                selectIndex = Mathf.Clamp(selectIndex, 0, 5);
                inventory.selectedItemIndex = selectIndex;
                selectSlot(selectIndex + 1);
            }
            return;
        }
        else if (wheelInput > 0)
        {
            if (selectIndex > 0)
            {
                selectIndex -= 1;
                selectIndex = Mathf.Clamp(selectIndex, 0, 5);
                inventory.selectedItemIndex = selectIndex;
                selectSlot(selectIndex + 1);
            }
            return;
        }
    }

    public bool HasEquippedKey()
    {
        Transform keyObject = equipItem.transform.Find("Key");
        if (keyObject != null)
        {
            ItemObject equippedItem = keyObject.GetComponent<ItemObject>();
            if (equippedItem != null)
            {
                return true;
            }
        }
        return false;
    }
    public void RemoveEquippedItem(string itemName)
    {
        Debug.Log($"RemoveEquippedItem 호출됨. 제거하려는 아이템: {itemName}");

        if (equipItem != null)
        {
            Debug.Log("equipItem이 null이 아닙니다. 모든 자식에서 아이템을 검색합니다.");

            // equipItem 하위 모든 자식에서 ItemObject를 검색
            ItemObject[] itemObjects = equipItem.GetComponentsInChildren<ItemObject>();
            Debug.Log($"검색된 ItemObject 개수: {itemObjects.Length}");

            foreach (ItemObject itemObject in itemObjects)
            {
                Debug.Log($"탐색된 아이템: {itemObject.item.ItemName}");
                Debug.Log($"비교 중: {itemObject.item.ItemName} == {itemName}");

                if (itemObject.item.ItemName == itemName)
                {
                    Debug.Log($"장착된 아이템 이름 {itemObject.item.ItemName}이(가) 제거하려는 아이템 이름과 일치합니다.");

                    // 인벤토리에서 제거
                    Inventory.instance.RemoveItem(itemName);

                    // 장착된 아이템 제거
                    Destroy(itemObject.gameObject);

                    Debug.Log($"장착된 아이템 {itemName}이(가) 제거되었습니다.");
                    return; // 아이템 제거 후 메서드 종료
                }
                else
                {
                    Debug.LogWarning($"이름이 일치하지 않음: {itemObject.item.ItemName} != {itemName}");
                }
            }

            Debug.LogWarning($"equipItem의 모든 자식에서 {itemName} 이름을 가진 아이템을 찾을 수 없습니다.");
        }
        else
        {
            Debug.LogWarning("equipItem이 null입니다. 장착된 아이템이 없습니다.");
        }
    }







    public bool HasEquippedCardKey()
    {
        // equipItem 하위에 "CardKey"라는 이름의 아이템을 찾음
        Transform cardKeyObject = equipItem.transform.Find("CardKey");
        if (cardKeyObject != null)
        {
            ItemObject equippedItem = cardKeyObject.GetComponent<ItemObject>();
            if (equippedItem != null)
            {
                return true; // CardKey가 장착되어 있으면 true 반환
            }
        }
        return false; // CardKey가 없으면 false 반환
    }

    // Player_Equip에서 특정 아이템이 장착되었는지 확인하는 메서드 추가
    public bool HasEquippedItem(string itemName)
    {
        if (equipItem != null)
        {
            ItemObject itemObject = equipItem.GetComponentInChildren<ItemObject>();
            if (itemObject != null && itemObject.item.ItemName == itemName)
            {
                return true; // 장착된 아이템이 있다면 true 반환
            }
        }
        return false; // 장착된 아이템이 없으면 false 반환
    }

    public bool HasAnyEquippedItem()
    {
        // equipItem의 모든 자식에서 ItemObject 컴포넌트를 검색하여 아이템이 장착되어 있는지 확인
        ItemObject[] equippedItems = equipItem.GetComponentsInChildren<ItemObject>();
        return equippedItems.Length > 0; // 하나라도 장착되어 있으면 true 반환
    }


}



