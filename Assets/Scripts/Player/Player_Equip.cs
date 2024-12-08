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
    public KeyCode throwKey = KeyCode.Mouse0;  // ������ Ű
    public float throwForce = 10f;  // �⺻ ������ ��
    public float maxForce = 20f;  // �ִ� ������ ��
    public LineRenderer trajectoryLine;  // ���� ǥ�� LineRenderer

    private bool isCharging = false;
    private float chargeTime = 0f;
    private bool hasGlassCup = false;
    private GameObject currentGlassCup;
    private Animator animator;           // Animator �߰�
    private CharacterController characterController;
    private bool isFlashlightOn = false; // ������ ���¸� ������ ����

    [Header("3��Ī ������ ��ġ")]
    public Transform thirdPersonHand; // 3��Ī ������ ��ġ
    public GameObject itemForOthers; // 3��Ī ĳ���Ͱ� ��� �ִ� �������� �����ϴ� ���� �߰�

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        pv = GetComponent<PhotonView>();

        // PhotonView�� null���� üũ
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
        ItemName = GameObject.Find("ItemName_Text").gameObject.GetComponent<TextMeshProUGUI>();
        ItemName.text = "";

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;  // ���� ǥ�� �ʱ�ȭ
        }

        // Animator ������Ʈ ��������
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator ������Ʈ�� ã�� �� �����ϴ�. �÷��̾� ������Ʈ�� Animator�� �־�� �մϴ�.");
        }
    }

    void Update()
    {
        playerState.GetState(out state);
        if (CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu || state == PlayerState.playerState.Die) return;

        numberKey();
        mouseWheelScroll();
       EquipFunction();

        // ������ ���� ó�� (������ ���� ������)
        if (isCharging)
        {
            ChargeThrow();
        }

        // ������ ���� ó��
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            ReleaseThrow();
        }
        if (isCharging && Input.GetMouseButtonDown(1)) // ���콺 ��Ŭ�� ��
        {
            CancelThrow();
        }
        // ������ ���� ���¿� ���� �ִϸ��̼� �Ķ���� ����
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
                Debug.LogError($"'{slotName}'��(��) ã�� �� �����ϴ�. ���� ������ Ȯ���� �ּ���.");
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

        // 1��Ī �� �𵨸��� ���� (������ ���� ��)
        Item = resoure.Instantiate($"Items/{item}");
        Item.layer = LayerMask.NameToLayer("Equip");
        Item.transform.SetParent(equipItem.transform);
        Item.transform.localPosition = Vector3.zero;
        Item.transform.localRotation = Quaternion.identity;
        Item.GetComponent<Rigidbody>().isKinematic = true;
        SetLayerRecursively(Item, 7);
        // 3��Ī �𵨸��� ���� (�ٸ� �÷��̾ ���� ��)
        if (PhotonNetwork.IsConnected)
        {
            if (itemForOthers != null)
            {
                PhotonNetwork.Destroy(itemForOthers);
                GetComponent<PhotonItem>().setPhotonEquipItem(item);
            }
            else
            {
                Debug.LogWarning("thirdPersonHand�� null�Դϴ�. 3��Ī �������� ������ ��ġ�� ã�� �� �����ϴ�.");
            }
        }

        // �������� ������ ��� ���� �� �ֵ��� ����
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
                inventory.EquipItem(Item); // ������ �������� Inventory���� �ݿ�

                //������ �̸� ���
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




        // 마우스 좌클릭으로 손전등 토글
        if (Input.GetMouseButtonDown(0) && Item != null)
        {
            // 손전등 처리
            if (Item.name == "Flashlight")
            {
                Flashlight1 flashlightScript = Item.GetComponent<Flashlight1>();
                if (flashlightScript != null)
                {
                    Debug.Log("손전등 토글 호출");
                   flashlightScript.ToggleFlashlight(); // ToggleFlashlight 메서드 호출 (RPC 포함)
                   flashlightScript.AcquireFlashlight();
                }
            }
            // 유리잔 처리
            else if (hasGlassCup && currentGlassCup != null && currentGlassCup.name == "GlassCup")
            {
                StartThrowing();
            }
            // 기타 아이템 처리
            else
            {
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
        // ���� ���� ���� �������� ���������� Ȯ��
        if (Item != null && Item.name == "Flashlight")
        {
            // �������� ���� �ִ� ���¿��� �������� �����ϰų� ������ ���, �ִϸ��̼� �Ķ���� �ʱ�ȭ
            if (isFlashlightOn)
            {
                isFlashlightOn = false;
              //  animator.SetBool("isFlashlightOn", false);
            }
        }

        // ���� �������� �����ϱ� ���� �׻� �ִϸ��̼� ���¸� �ʱ�ȭ
        if (animator != null)
        {
            // �������� ���� �ִ� �ִϸ��̼��� �����Ͽ� ��� ������ ���� ���¸� �ʱ�ȭ
         //   animator.SetBool("isFlashlightOn", false);
            animator.SetBool("isHoldingItem", false);
        }

        // ���� �������� ����
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
            trajectoryLine.enabled = true;  // ���� ǥ�� Ȱ��ȭ
        }
    }

    void ChargeThrow()
    {
        isCharging = true;
        chargeTime += Time.deltaTime;
        float currentForce = Mathf.Min(chargeTime * throwForce, maxForce);

        // �ִϸ����� �Ķ���� ����: ���� ���� ����
        animator.SetBool("isChargingThrow", true);

        // ���� ������Ʈ
        Vector3 throwDirection = Camera.main.transform.forward;
        Vector3 grenadeVelocity = throwDirection * currentForce;
        ShowTrajectory(Camera.main.transform.position + Camera.main.transform.forward, grenadeVelocity);
    }

    void ReleaseThrow()
    {
        if (currentGlassCup != null)
        {
            // �θ�-�ڽ� ���� ����
            currentGlassCup.transform.parent = null;

            float finalForce = Mathf.Min(chargeTime * throwForce, maxForce);
            Rigidbody rb = currentGlassCup.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;   // ���� �� ���������� �����̵��� ����
                rb.useGravity = true;   // �߷� Ȱ��ȭ
                Vector3 throwDirection = Camera.main.transform.forward;
                rb.AddForce(throwDirection * finalForce, ForceMode.VelocityChange);
            }

            // ���� �������� itemData ��������
            itemData cupItemData = currentGlassCup.GetComponent<ItemObject>().item;

            // �κ��丮���� ������ ����
            if (cupItemData != null)
            {
                Inventory.instance.RemoveItem(cupItemData.ItemName);
            }

            hasGlassCup = false;  // ���� �� ������ ���� ���� ����
            currentGlassCup = null;  // ������ ���� ����

            if (trajectoryLine != null)
            {
                trajectoryLine.enabled = false;  // ���� ǥ�� ��Ȱ��ȭ
            }
        }

        // ���� �� ���� ����
        animator.SetBool("isChargingThrow", false);

        // ������ �ִϸ��̼� Ʈ���� ����
        animator.SetTrigger("isThrowing");

        isCharging = false;
    }


    void CancelThrow()
    {
        isCharging = false;
        chargeTime = 0f;

        // ���� ���� ����
        animator.SetBool("isChargingThrow", false);

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;  // ���� ǥ�� ��Ȱ��ȭ
        }

        Debug.Log("������ ��ҵ�");
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
        // CrowBar �������� �����Ǿ� �ִ��� Ȯ��
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

    public bool HasEquippedCardKey()
    {
        // equipItem ������ "CardKey"��� �̸��� �������� ã��
        Transform cardKeyObject = equipItem.transform.Find("CardKey");
        if (cardKeyObject != null)
        {
            ItemObject equippedItem = cardKeyObject.GetComponent<ItemObject>();
            if (equippedItem != null)
            {
                return true; // CardKey�� �����Ǿ� ������ true ��ȯ
            }
        }
        return false; // CardKey�� ������ false ��ȯ
    }

    // Player_Equip���� Ư�� �������� �����Ǿ����� Ȯ���ϴ� �޼��� �߰�
    public bool HasEquippedItem(string itemName)
    {
        if (equipItem != null)
        {
            ItemObject itemObject = equipItem.GetComponentInChildren<ItemObject>();
            if (itemObject != null && itemObject.item.ItemName == itemName)
            {
                return true; // ������ �������� �ִٸ� true ��ȯ
            }
        }
        return false; // ������ �������� ������ false ��ȯ
    }

    public bool HasAnyEquippedItem()
    {
        // equipItem�� ��� �ڽĿ��� ItemObject ������Ʈ�� �˻��Ͽ� �������� �����Ǿ� �ִ��� Ȯ��
        ItemObject[] equippedItems = equipItem.GetComponentsInChildren<ItemObject>();
        return equippedItems.Length > 0; // �ϳ��� �����Ǿ� ������ true ��ȯ
    }

    // ������Ʈ�� ��� �ڽ� ������Ʈ�� ���̾ ����
    void SetLayerRecursively(GameObject obj, int layer)
    {
        // ���� ������Ʈ�� ���̾� ����
        obj.layer = layer;

        // �ڽ� ������Ʈ ��ȸ�ϸ� ���̾� ����
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}



