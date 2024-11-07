using Photon.Pun;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public itemData item;

    private GlassCupThrower glassCupThrower;

    void Start()
    {
        glassCupThrower = FindObjectOfType<GlassCupThrower>();
        if (glassCupThrower == null)
        {
            Debug.LogError("GlassCupThrower 컴포넌트를 찾을 수 없습니다!");
        }
    }

    public string GetInteractPrompt()
    {
        return string.Format("줍기 {0}", item.ItemName);
    }

    public void OnInteract()
    {
        // 슬롯에 빈 공간이 있는지 확인 후 아이템 추가
        if (addSlot())
        {
            Inventory.instance.Additem(item);
            Debug.Log($"아이템 획득: {item.ItemName}");

            // 유리컵을 획득한 경우 처리
            if (item.ItemName == "GlassCup")
            {
                Debug.Log("유리컵을 획득하려고 합니다...");

                if (glassCupThrower != null)
                {
                    Debug.Log("GlassCupThrower.PickUpGlassCup 호출 준비됨");
                    glassCupThrower.PickUpGlassCup(gameObject);
                    gameObject.SetActive(false); // 비활성화
                }
                else
                {
                    Debug.LogWarning("GlassCupThrower가 존재하지 않습니다. 유리컵을 획득할 수 없습니다.");
                }
            }
        }
        else
        {
            Debug.LogWarning("인벤토리에 빈 슬롯이 없습니다. 아이템을 획득할 수 없습니다.");
        }
    }

    bool addSlot()
    {
        // 인벤토리에 빈 슬롯이 있는지 확인
        for (int i = 0; i < Inventory.instance.slots.Length; i++)
        {
            if (Inventory.instance.slots[i].item == null)
                return true;
        }
        return false;
    }
}
