using UnityEngine;
using Photon.Pun; // Photon 관련 클래스 사용

public class ItemPickup : MonoBehaviourPun
{
    public string itemName; // 이 아이템의 고유 이름

    // 아이템 상호작용 메서드
    public void Interact()
    {
        // 이미 획득된 아이템이면 실행 안 함
        if (ItemManager.Instance.GetItemState(itemName)) return;

        // 아이템 획득 처리
        ItemManager.Instance.SetItemState(itemName, true);

        Debug.Log($"{itemName}을(를) 획득했습니다!");
    }

    [PunRPC]
    public void RPC_HandleItemPickup()
    {
        // 아이템 비활성화
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} 아이템이 비활성화되었습니다.");
    }
}
