using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen = false;
    private bool openDoor = false;
    private Player_Equip playerEquip; // Player_Equip 스크립트 참조

    void Start()
    {
        animator = GetComponent<Animator>();
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip 컴포넌트를 가진 오브젝트 찾기
    }

    public void OnInteract()
    {
        if (openDoor == true)
        {
            OpenDoor();
            Debug.Log("문이 열립니다.");
            openDoor = false;
            return;
        }
        // 문이 열려있지 않고, Player_Equip에서 열쇠가 장착된 상태인지 확인
        if (!isOpen && playerEquip != null && playerEquip.HasEquippedKey())
        {
            isOpen = true;
            Debug.Log("문의 잠금을 해제했습니다.");
            //OpenDoor();
            // 문의 잠금상태를 열림으로 변경
            openDoor = true;
            return;
        }
        else if (!isOpen)
        {
            Debug.Log("잠긴 문입니다. 열쇠가 필요합니다.");
        }
        if (isOpen && openDoor == false)
        {
            Closedoor();
            Debug.Log("문이 닫힙니다.");
            openDoor = true;
            return;
        }
    }

    public string GetInteractPrompt()
    {
        if (isOpen == false)
        {
            return "문 잠금해제";
        }
        else if (openDoor == false)
        {
            return "문 닫기";
        }
        else if (openDoor == true)
        {
            return "문 열기";
        }
        return "???"; // 기본 반환값 추가
    }

    void OpenDoor()
    {
        if (animator != null)
        {
            animator.SetBool("isOpen", true); // 문 열기 애니메이션 실행
        }
        else
        {
            // Animator가 없는 경우 문을 열리는 방향으로 이동시키는 예시 (간단한 이동)
            transform.position += -transform.forward * 1.5f; // 문을 오른쪽으로 약간 이동
        }
    }
    void Closedoor()
    {
        if (animator != null)
        {
            animator.SetBool("isClose", true); // 문 닫기 애니메이션 실행
        }
        else
        {
            // Animator가 없는 경우 문을 열리는 방향으로 이동시키는 예시 (간단한 이동)
            transform.position -= -transform.forward * 1.5f; // 문을 왼쪽으로 약간 이동
        }
    }
}