using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_Equip : MonoBehaviourPun
{
    public static Player_Equip instance;

    Dictionary<KeyCode, System.Action> keyCodeDic = new Dictionary<KeyCode, System.Action>();

    public ItemSlotUI[] invenSlot = new ItemSlotUI[6];
    public GameObject equipItem;
    public GameObject Item;
    public TextMeshProUGUI ItemName;
    Inventory inventory;

    ResourceManager resoure = new ResourceManager();
    int selectIndex = 0;
    PhotonView pv;
    PlayerState playerState;
    PlayerState.playerState state;

    [Header("Throw Settings")]
    public KeyCode throwKey = KeyCode.Mouse0;  // ?????? ?
    public float throwForce = 10f;  // ?? ?????? ??
    public float maxForce = 20f;  // ??? ?????? ??
    public LineRenderer trajectoryLine;  // ???? ??? LineRenderer

    private bool isCharging = false;
    private float chargeTime = 0f;
    private bool hasGlassCup = false;
    private GameObject currentGlassCup;
    private Animator animator;           // Animator ???
    private CharacterController characterController;
    private bool isFlashlightOn = false; // ?????? ???¸? ?????? ????

    [Header("3??? ?????? ???")]
    public Transform thirdPersonHand; // 3??? ?????? ???
    public GameObject itemForOthers; // 3??? ĳ????? ??? ??? ???????? ??????? ???? ???

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        pv = GetComponent<PhotonView>();

        // PhotonView?? null???? ??
        if (pv == null)
        {
            Debug.LogError("PhotonView component not found.");
            return;
        }
        playerState = GetComponent<PlayerState>();

        ConnectUi_itemSlot();
        setnumberKey();
        invenSlot[0].GetComponent<ItemSlotUI>().equipped = true;

        inventory = GetComponent<Inventory>();
        equipItem = GameObject.Find("handitemattach").gameObject;
        ItemName = GameObject.Find("ItemName_Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        ItemName.text = "";

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;  // ???? ??? ????
        }

        // Animator ??????? ????????
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator ????????? ??? ?? ???????. ?÷???? ????????? Animator?? ???? ????.");
        }
    }

    void Update()
    {
        playerState.GetState(out state);
        if (CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu || state == PlayerState.playerState.Die) return;

        numberKey();
        mouseWheelScroll();
       // EquipFunction();

        // ?????? ???? ??? (?????? ???? ??????)
        if (isCharging)
        {
            ChargeThrow();
        }

        // ?????? ???? ???
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            ReleaseThrow();
        }
        if (isCharging && Input.GetMouseButtonDown(1)) // ???콺 ????? ??
        {
            CancelThrow();
        }
        // ?????? ???? ???¿? ???? ??????? ?????? ????
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
                Debug.LogError($"'{slotName}'??(??) ??? ?? ???????. ???? ?????? ????? ?????.");
            }
        }
    }

    public void selectSlot(int index)
    {
        playerState.GetState(out state);
        if (CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Die) return;

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

        // 1??? ?? ?????? ???? (?????? ???? ??)
        Item = resoure.Instantiate($"Items/{item}");
        Item.layer = LayerMask.NameToLayer("Equip");
        Item.transform.SetParent(equipItem.transform);
        Item.transform.localPosition = Vector3.zero;
        Item.transform.localRotation = Quaternion.identity;
        // 3??? ?????? ???? (??? ?÷???? ???? ??)
        if (PhotonNetwork.IsConnected)
        {
            if (itemForOthers != null)
            {
                PhotonNetwork.Destroy(itemForOthers);
            }
            else
            {
                Debug.LogWarning("thirdPersonHand?? null????. 3??? ???????? ?????? ????? ??? ?? ???????.");
            }
        }

        // ???????? ?????? ??? ???? ?? ????? ????
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
                if (PhotonNetwork.IsConnected)
                {
                    GetComponent<PhotonItem>().setPhotonEquipItem(inventory.slots[index - 1].item.name);
                }
                inventory.EquipItem(Item); // ?????? ???????? Inventory???? ???

                //?????? ??? ???
                ItemName.text = inventory.slots[index - 1].item.ItemName;
            }
            else if (Item != null)
            {
                Destroy(Item);
                ItemName.text = "";
                if (PhotonNetwork.IsConnected)
                {
                    GetComponent<PhotonItem>().RemoveEquippedItem(Item.name);
                }
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
                    Debug.Log("손전등 사용");

                    // 손전등 켜기/끄기 처리
                    flashlightScript.ToggleFlashlight(); // ToggleFlashlight() 메서드를 호출하여 상태 변경
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
    public void ChangeOrRemoveItem()
    {
        // ???? ???? ???? ???????? ?????????? ???
        if (Item != null && Item.name == "Flashlight")
        {
            // ???????? ???? ??? ???¿??? ???????? ???????? ?????? ???, ??????? ?????? ????
            if (isFlashlightOn)
            {
                isFlashlightOn = false;
                animator.SetBool("isFlashlightOn", false);
            }
        }

        // ???? ???????? ??????? ???? ??? ??????? ???¸? ????
        if (animator != null)
        {
            // ???????? ???? ??? ????????? ??????? ??? ?????? ???? ???¸? ????
            animator.SetBool("isFlashlightOn", false);
            animator.SetBool("isHoldingItem", false);
        }

        // ???? ???????? ????
        if (Item != null)
        {
            Destroy(Item);
            Item = null;
            ItemName.text = "";
        }
    }


    void StartThrowing()
    {
        isCharging = true;
        chargeTime = 0f;

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = true;  // ???? ??? ????
        }
    }

    void ChargeThrow()
    {
        isCharging = true;
        chargeTime += Time.deltaTime;
        float currentForce = Mathf.Min(chargeTime * throwForce, maxForce);

        // ???????? ?????? ????: ???? ???? ????
        animator.SetBool("isChargingThrow", true);

        // ???? ???????
        Vector3 throwDirection = Camera.main.transform.forward;
        Vector3 grenadeVelocity = throwDirection * currentForce;
        ShowTrajectory(Camera.main.transform.position + Camera.main.transform.forward, grenadeVelocity);
    }

    void ReleaseThrow()
    {
        if (currentGlassCup != null)
        {
            // ?θ?-??? ???? ????
            currentGlassCup.transform.parent = null;

            float finalForce = Mathf.Min(chargeTime * throwForce, maxForce);
            Rigidbody rb = currentGlassCup.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;   // ???? ?? ?????????? ????????? ????
                rb.useGravity = true;   // ??? ????
                Vector3 throwDirection = Camera.main.transform.forward;
                rb.AddForce(throwDirection * finalForce, ForceMode.VelocityChange);
            }

            // ???? ???????? itemData ????????
            itemData cupItemData = currentGlassCup.GetComponent<ItemObject>().item;

            // ?κ??????? ?????? ????
            if (cupItemData != null)
            {
                Inventory.instance.RemoveItem(cupItemData.ItemName);
            }

            hasGlassCup = false;  // ???? ?? ?????? ???? ???? ????
            currentGlassCup = null;  // ?????? ???? ????

            if (trajectoryLine != null)
            {
                trajectoryLine.enabled = false;  // ???? ??? ??????
            }
        }

        // ???? ?? ???? ????
        animator.SetBool("isChargingThrow", false);

        // ?????? ??????? ????? ????
        animator.SetTrigger("isThrowing");

        isCharging = false;
    }


    void CancelThrow()
    {
        isCharging = false;
        chargeTime = 0f;

        // ???? ???? ????
        animator.SetBool("isChargingThrow", false);

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;  // ???? ??? ??????
        }

        Debug.Log("?????? ????");
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
        playerState.GetState(out state);
        if (CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu || state == PlayerState.playerState.Die) return;

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

    public bool HasEquippedCrowBar()
    {
        // CrowBar ???????? ??????? ????? ???
        Transform crowBarObject = equipItem.transform.Find("CrowBar");
        if (crowBarObject != null)
        {
            ItemObject equippedItem = crowBarObject.GetComponent<ItemObject>();
            if (equippedItem != null)
            {
                return true;
            }
        }
        return false;
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
    [PunRPC]
    public void RemoveEquippedItem(string itemName)
    {
        Debug.Log($"RemoveEquippedItem ????. ????????? ??????: {itemName}");

        if (equipItem != null)
        {
            Debug.Log("equipItem?? null?? ?????. ??? ??Ŀ??? ???????? ???????.");

            // equipItem ???? ??? ??Ŀ??? ItemObject?? ???
            ItemObject[] itemObjects = equipItem.GetComponentsInChildren<ItemObject>();
            Debug.Log($"????? ItemObject ????: {itemObjects.Length}");

            foreach (ItemObject itemObject in itemObjects)
            {
                Debug.Log($"????? ??????: {itemObject.item.ItemName}");
                Debug.Log($"?? ??: {itemObject.item.ItemName} == {itemName}");
                // ?κ??????? ????
                Inventory.instance.RemoveItem(itemName);
                // ?????? ?????? ????
                Destroy(itemObject.gameObject);
            }

            Debug.LogWarning($"equipItem?? ??? ??Ŀ??? {itemName} ????? ???? ???????? ??? ?? ???????.");
        }
        else
        {
            Debug.LogWarning("equipItem?? null????. ?????? ???????? ???????.");
        }

        // 3??? ??????? ?????? ????
        if (itemForOthers != null)
        {
            PhotonNetwork.Destroy(itemForOthers);
            itemForOthers = null;
        }

    }


    public bool HasEquippedCardKey()
    {
        // equipItem ?????? "CardKey"??? ????? ???????? ???
        Transform cardKeyObject = equipItem.transform.Find("CardKey");
        if (cardKeyObject != null)
        {
            ItemObject equippedItem = cardKeyObject.GetComponent<ItemObject>();
            if (equippedItem != null)
            {
                return true; // CardKey?? ??????? ?????? true ???
            }
        }
        return false; // CardKey?? ?????? false ???
    }

    // Player_Equip???? ??? ???????? ??????????? ?????? ????? ???
    public bool HasEquippedItem(string itemName)
    {
        if (equipItem != null)
        {
            ItemObject itemObject = equipItem.GetComponentInChildren<ItemObject>();
            if (itemObject != null && itemObject.item.ItemName == itemName)
            {
                return true; // ?????? ???????? ???? true ???
            }
        }
        return false; // ?????? ???????? ?????? false ???
    }

    public bool HasAnyEquippedItem()
    {
        // equipItem?? ??? ??Ŀ??? ItemObject ????????? ?????? ???????? ??????? ????? ???
        ItemObject[] equippedItems = equipItem.GetComponentsInChildren<ItemObject>();
        return equippedItems.Length > 0; // ????? ??????? ?????? true ???
    }


}



