using UnityEngine;

public class DoorWithCardKey : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen = false;
    private Player_Equip playerEquip; // Player_Equip 스크립트 참조

    [SerializeField] private AudioSource audioSource; // AudioSource 컴포넌트
    [SerializeField] private AudioClip openSound; // 문 열림 소리
    [SerializeField] private AudioClip closeSound; // 문 닫힘 소리
    [SerializeField] private float autoCloseDelay = 5f; // 몇 초 후 자동으로 닫힐지 설정

    private float closeTimer = 0f;
    private bool autoClosing = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip 스크립트 찾기

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // 문이 열려 있고 자동 닫힘이 활성화된 경우 타이머 체크
        if (isOpen && autoClosing)
        {
            closeTimer -= Time.deltaTime;
            if (closeTimer <= 0f)
            {
                CloseDoor();
            }
        }
    }

    public void OnInteract()
    {
        if (!isOpen && playerEquip != null && playerEquip.HasEquippedCardKey())
        {
            OpenDoor();
            return;
        }

        if (isOpen)
        {
            Debug.Log("문이 이미 열려 있습니다.");
        }
        else
        {
            Debug.Log("문을 열 수 없습니다. CardKey가 필요합니다.");
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        autoClosing = true; // 자동 닫힘 활성화
        closeTimer = autoCloseDelay; // 타이머 초기화

        // 문을 오른쪽으로 이동
        Vector3 moveDirection = -transform.forward * 1.5f; // 오른쪽으로 1.5 단위 이동
        transform.position += moveDirection;

        if (animator != null)
        {
            animator.SetBool("isOpen", true); // 애니메이션 실행
        }

        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound); // 열리는 소리 재생
        }

        Debug.Log("문이 열렸습니다.");
    }

    void CloseDoor()
    {
        isOpen = false;
        autoClosing = false; // 자동 닫힘 비활성화

        // 문을 왼쪽으로 이동 (원래 위치 복구)
        Vector3 moveDirection = -transform.forward * -1.5f; // 왼쪽으로 1.5 단위 이동
        transform.position += moveDirection;


        if (animator != null)
        {
            animator.SetBool("isOpen", false); // 닫히는 애니메이션 실행
        }

        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound); // 닫히는 소리 재생
        }

        Debug.Log("문이 닫혔습니다.");
    }

    public string GetInteractPrompt()
    {
        if (!isOpen)
        {
            return "CardKey를 사용하여 문 열기";
        }
        else
        {
            return "문이 열려 있습니다.";
        }
    }
}
