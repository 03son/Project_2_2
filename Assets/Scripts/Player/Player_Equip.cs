using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Throw Settings")]
    public KeyCode throwKey = KeyCode.Mouse0;  // 던지기 키
    public float throwForce = 10f;  // 기본 던지는 힘
    public float maxForce = 20f;  // 최대 던지는 힘
    public LineRenderer trajectoryLine;  // 궤적 표시 LineRenderer

    private bool isCharging = false;
    private float chargeTime = 0f;
    private bool hasGlassCup = false;
    private GameObject currentGlassCup;

    void Start()
    {
        pv = GetComponent<PhotonView>();

        ConnectUi_itemSlot();
        setnumberKey();
        invenSlot[0].GetComponent<ItemSlotUI>().equipped = true;

        inventory = GetComponent<Inventory>();
        equipItem = GameObject.Find("EquipItem").gameObject;

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;  // 궤적 표시 초기화
        }
    }

    void Update()
    {
        numberKey();
        mouseWheelScroll();
        EquipFunction();
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
            }
            else if (Item != null)
            {
                Destroy(Item);
            }
        }
    }

    void EquipFunction()
    {
        if (Input.GetMouseButtonDown(0) && Item != null)
        {
            // 아이템이 유리컵인 경우
            if (hasGlassCup && currentGlassCup != null && currentGlassCup.name == "GlassCup")
            {
                StartThrowing();
            }
            else if (Item != null)
            {
                // IItemFunction 인터페이스를 가진 다른 아이템 사용 처리
                IItemFunction itemFunction = Item.GetComponent<IItemFunction>();
                if (itemFunction != null)
                {
                    Debug.Log($"{Item.name} 아이템 기능 실행");
                    itemFunction.Function();
                }

                // 손전등 아이템 처리
                if (Item.name == "Flashlight")
                {
                    Flashlight1 flashlightScript = Item.GetComponent<Flashlight1>();
                    if (flashlightScript != null)
                    {
                        Debug.Log("손전등 획득 및 사용");
                        flashlightScript.AcquireFlashlight();
                    }
                }
            }
        }

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
        chargeTime += Time.deltaTime;
        float currentForce = Mathf.Min(chargeTime * throwForce, maxForce);

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

            hasGlassCup = false;  // 던진 후 유리컵 소지 상태 해제
            currentGlassCup = null;  // 유리컵 참조 해제

            if (trajectoryLine != null)
            {
                trajectoryLine.enabled = false;  // 궤적 표시 비활성화
            }
        }

        isCharging = false;
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
}
