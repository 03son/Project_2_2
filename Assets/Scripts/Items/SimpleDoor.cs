using UnityEngine;

public class SimpleDoor : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen = false; // 문이 열려있는 상태
    private bool openDoor = false; // 문이 닫혀있는 상태

    [SerializeField] private AudioSource audioSource; // AudioSource 컴포넌트
    [SerializeField] private AudioClip openSound; // 열리는 소리 클립
    [SerializeField] private AudioClip closeSound; // 닫히는 소리 클립

    void Start()
    {
        animator = GetComponent<Animator>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource 컴포넌트가 없으면 추가
        }
    }

    public void OnInteract()
    {
        if (isOpen)
        {
            CloseDoor(); // 문 닫기
        }
        else
        {
            OpenDoor(); // 문 열기
        }
    }

    public string GetInteractPrompt()
    {
        return isOpen ? "문 닫기" : "문 열기"; // 프롬프트 텍스트 반환
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

        isOpen = true;

        // 열리는 소리 재생
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }

    void CloseDoor()
    {
        if (animator != null)
        {
            animator.SetBool("isOpen", false); // 문 닫기 애니메이션 실행
        }
        else
        {
            transform.position -= -transform.forward * 1.5f; // 문을 왼쪽으로 약간 이동
        }

        isOpen = false;

        // 닫히는 소리 재생
        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }
}
