using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen = false;
    private Player_Equip playerEquip; // Player_Equip 스크립트 참조

    void Start()
    {
        animator = GetComponent<Animator>();
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip 컴포넌트를 가진 오브젝트 찾기
    }

    public void OnInteract()
    {
        // 문이 열려있지 않고, Player_Equip에서 열쇠가 장착된 상태인지 확인
        if (!isOpen && playerEquip != null && playerEquip.HasEquippedKey())
        {
            Debug.Log("문이 열렸습니다.");
            OpenDoor();
        }
        else if (!isOpen)
        {
            Debug.Log("잠긴 문입니다. 열쇠가 필요합니다.");
        }
    }

    public string GetInteractPrompt()
    {
        return "문 열기";
    }

    void OpenDoor()
    {
        isOpen = true;
        if (animator != null)
        {
            animator.SetBool("isOpen", true); // 문 열기 애니메이션 실행
        }
        else
        {
            gameObject.SetActive(false); // Animator가 없으면 문을 비활성화
        }
    }
}
