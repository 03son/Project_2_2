using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen = false;
    private bool openDoor = false;
    private Player_Equip playerEquip; // Player_Equip 스크립트 참조

    [SerializeField] private AudioSource audioSource; // AudioSource 컴포넌트
    [SerializeField] private AudioClip lockedSound; // 잠겨있는 소리 클립
    [SerializeField] private AudioClip openSound; // 열리는 소리 클립

    void Start()
    {
        animator = GetComponent<Animator>();
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip 컴포넌트를 가진 오브젝트 찾기

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource 컴포넌트가 없으면 추가
        }
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
            openDoor = true;

            // Player_Equip의 RemoveEquippedItem 호출
            Inventory.instance.RemoveItem("Key");
            playerEquip.setEquipItem("Key");

            return;
        }
        else if (!isOpen)
        {
            Debug.Log("잠긴 문입니다. 열쇠가 필요합니다.");
            if (audioSource != null && lockedSound != null)
            {
                audioSource.PlayOneShot(lockedSound); // 잠겨있는 소리 재생
            }
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
            transform.position += -transform.forward * 1.5f; // 문을 오른쪽으로 약간 이동
        }

        // 열리는 소리 재생
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
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
            transform.position -= -transform.forward * 1.5f; // 문을 왼쪽으로 약간 이동
        }
    }
}
