using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;   // 상호작용 가능한 거리
    public LayerMask interactionLayer;    // 상호작용 가능한 레이어 설정
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;       // 플레이어의 메인 카메라 가져오기
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))  // E 키를 눌렀을 때 상호작용 시도
        {
            Interact();
        }
    }

    void Interact()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // interactionLayer에 설정된 오브젝트만 감지하도록 레이캐스트
        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            Debug.Log("레이캐스트 히트: " + hit.collider.gameObject.name);

            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.OnInteract();
            }
        }
        else
        {
            Debug.Log("레이캐스트가 아무것도 감지하지 못했습니다.");
        }
    }
}
