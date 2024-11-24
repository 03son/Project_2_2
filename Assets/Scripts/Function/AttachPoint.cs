using Photon.Pun;
using UnityEngine;

public class AttachPoint : MonoBehaviourPun, IInteractable
{
    public string requiredItemName; // 필요한 아이템 이름 (예: "Propeller")
    public GameObject attachedItemPrefab; // 부착할 아이템의 프리팹
    private bool isAttached = false; // 이미 부착되었는지 여부

    // IInteractable 인터페이스의 상호작용 프롬프트 반환 메서드
    public string GetInteractPrompt()
    {
        if (isAttached)
            return ""; // 이미 부착되었으면 빈 문자열 반환
        else
            return $"{requiredItemName} 부착하기"; // 아직 부착되지 않았으면 프롬프트 표시
    }

    public void OnInteract()
    {
        if (isAttached)
        {
            Debug.Log("이미 부착됨");
            return;
        }

        Inventory inventory = Inventory.instance;

        if (inventory.HasItem(requiredItemName))
        {
            Debug.Log($"{requiredItemName} 아이템이 인벤토리에 있음. 부착 시작");

            // 아이템 제거
            inventory.RemoveItem(requiredItemName);

            // 부착 상태 동기화 (RPC 호출)
            photonView.RPC("RPC_AttachItem", RpcTarget.All);
            Debug.Log("RPC_AttachItem 호출됨"); // 호출 확인용 로그
        }
        else
        {
            Debug.Log($"{requiredItemName}이(가) 필요합니다.");
        }
    }

    [PunRPC]
    void RPC_AttachItem()
    {
        Debug.Log($"{requiredItemName} 부착 완료");

        Instantiate(attachedItemPrefab, transform.position, transform.rotation, transform.parent);
        isAttached = true;

        SubmarineController submarine = GetComponentInParent<SubmarineController>();
        if (submarine != null)
        {
            submarine.AttachItem(requiredItemName);
            Debug.Log("잠수함에 부착된 아이템 정보 전달 완료");
        }
        else
        {
            Debug.LogWarning("잠수함 컨트롤러가 없습니다.");
        }
    }
}
