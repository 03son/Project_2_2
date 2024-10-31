using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f; // 상호작용 거리
    public LayerMask interactionLayer; // 상호작용할 레이어
    private Camera playerCamera; // 플레이어의 카메라 참조

    private Player_Equip playerEquip; // Player_Equip 스크립트 참조

    void Start()
    {
        playerCamera = Camera.main;
        interactionLayer = LayerMask.GetMask("Interactable"); // "Interactable" 레이어 설정
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip 스크립트 찾기
    }

    void Update()
    {
        // 좌클릭 시 "Key"라는 이름의 열쇠가 장착된 상태라면 레이캐스트 발사
        if (Input.GetMouseButtonDown(0) && playerEquip.HasEquippedKey())
        {
            InteractWithDoor();
        }
    }

    // 문과 상호작용하는 메서드
    void InteractWithDoor()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // 레이캐스트를 발사하여 문과 충돌했는지 확인
        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            Door door = hit.collider.GetComponent<Door>();
            if (door != null)
            {
                Debug.Log("문과 상호작용 시도");
                door.OnInteract(); // 문을 여는 메서드 호출
            }
            else
            {
                Debug.Log("문이 아닌 대상과 충돌");
            }
        }
        else
        {
            Debug.Log("레이캐스트가 아무것도 감지하지 못했습니다.");
        }
    }
}
