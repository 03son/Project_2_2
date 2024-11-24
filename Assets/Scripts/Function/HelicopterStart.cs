using UnityEngine;

public class HelicopterStart : MonoBehaviour, IInteractable
{
    public HelicopterController helicopterController; // 헬리콥터 컨트롤러
    public Transform equipItem; // 플레이어의 EquipItem 객체
    private bool isPlayerNearby = false; // 플레이어가 근처에 있는지 여부
    public string requiredItemName = "HelicopterKey"; // 필요한 아이템 이름

    // 인터페이스 메서드: 상호작용 메시지 반환
    public string GetInteractPrompt()
    {
        return $"키를 눌러 헬리콥터 시동 ({requiredItemName} 필요)";
    }

    // 인터페이스 메서드: 상호작용 동작
    public void OnInteract()
    {
        if (!isPlayerNearby)
            return;

        if (helicopterController == null)
        {
            Debug.LogError("HelicopterController가 null입니다. Inspector에서 설정했는지 확인하세요.");
            return;
        }

        // EquipItem의 자식 객체를 확인
        Transform equippedItem = equipItem.Find(requiredItemName);
        if (equippedItem != null)
        {
            // 필요한 아이템이 장착되어 있으면 시동 걸기
            Debug.Log($"{requiredItemName}를 사용하여 헬리콥터 시동을 겁니다.");
            Destroy(equippedItem.gameObject); // 아이템 사용 후 제거
            helicopterController.StartHelicopter();
        }
        else
        {
            // 필요한 아이템이 없으면
            Debug.Log($"{requiredItemName} 아이템이 필요합니다!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("헬리콥터 컨트롤 패널 범위에 진입");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("헬리콥터 컨트롤 패널 범위에서 벗어남");
        }
    }
}
