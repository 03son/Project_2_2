using UnityEngine;

public class HelicopterStart : MonoBehaviour, IInteractable
{
    public HelicopterController helicopterController; // 헬리콥터 컨트롤러
    public Transform equipItem; // 플레이어의 EquipItem 객체
    private bool isPlayerNearby = false; // 플레이어가 근처에 있는지 여부
    public string requiredItemName = "HelicopterKey"; // 필요한 아이템 이름
    private Player_Equip playerEquip; // 플레이어의 Player_Equip 스크립트 참조

    // 인터페이스 메서드: 상호작용 메시지 반환

    void Start()
    {
        // playerEquip을 동적으로 할당
        if (playerEquip == null)
        {
            playerEquip = FindObjectOfType<Player_Equip>();
            if (playerEquip == null)
            {
                Debug.LogError("Player_Equip 스크립트를 찾을 수 없습니다.");
            }
        }
    }
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

        // Player_Equip에서 현재 장착된 아이템이 필요한 아이템인지 확인
        if (playerEquip != null && playerEquip.HasEquippedItem(requiredItemName))
        {
            // 필요한 아이템이 장착되어 있으면 시동 시도
            Debug.Log($"{requiredItemName}를 사용하여 헬리콥터 시동을 겁니다.");

            bool startSuccess = helicopterController.StartHelicopter(); // 시동 시도 후 성공 여부 반환

            if (startSuccess)
            {
                // 시동이 성공하면 아이템 제거
                playerEquip.RemoveEquippedItem(requiredItemName);
                Debug.Log($"{requiredItemName}가 사용되었습니다.");
            }
            else
            {
                // 시동이 실패하면 아이템 유지
                Debug.Log("헬리콥터 시동 실패. 아이템을 유지합니다.");
            }
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
