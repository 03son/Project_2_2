using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f; // 플레이어가 상호작용할 수 있는 거리
    public LayerMask interactionLayer; // 플레이어가 상호작용할 수 있는 오브젝트의 레이어
    private Camera playerCamera; // 플레이어의 카메라 참조

    void Start()
    {
        playerCamera = Camera.main; // 플레이어의 메인 카메라 가져오기
        interactionLayer = LayerMask.GetMask("Item", "Interactable"); // "Item"과 "Interactable" 레이어 모두 상호작용 가능하도록 설정
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // 플레이어가 'E' 키를 눌렀을 때
        {
            Interact(); // 상호작용 메서드 호출
        }
    }

    // 상호작용을 처리하는 메서드
    void Interact()
    {
        // 카메라 위치에서 앞 방향으로 레이 생성
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // 레이를 쏘아 지정된 거리와 레이어 내에서 오브젝트와 충돌했는지 확인
        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            Debug.Log("레이가 충돌한 오브젝트: " + hit.collider.gameObject.name); // 레이가 충돌한 오브젝트의 이름 출력
            Item item = hit.collider.GetComponent<Item>(); // 충돌한 오브젝트에서 Item 컴포넌트 가져오기
            if (item != null)
            {
                Debug.Log("상호작용 가능한 아이템 발견: " + item.itemName); // 상호작용 가능한 아이템 발견 시 로그 출력
                item.OnInteract(); // 아이템의 상호작용 메서드 호출
            }
        }
        else
        {
            Debug.Log("레이캐스트가 아무것도 감지하지 못했습니다."); // 레이가 아무것도 감지하지 못했을 때 로그 출력
        }
    }
}
