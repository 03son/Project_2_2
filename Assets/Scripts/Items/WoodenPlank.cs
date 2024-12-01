using UnityEngine;

public class WoodenPlank : MonoBehaviour, IInteractable
{
    private Player_Equip playerEquip; // Player_Equip 스크립트 참조
    private bool isRemoving = false; // 제거 진행 여부
    private float holdTime = 2.5f; // 홀딩 시간
    private float holdCounter = 0f; // 현재 누르고 있는 시간

    [SerializeField] private AudioSource audioSource; // AudioSource 컴포넌트
    [SerializeField] private AudioClip holdSound; // 키를 누르고 있을 때 효과음

    void Start()
    {
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip 스크립트 찾기
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isRemoving)
        {
            // 타이머 진행
            holdCounter += Time.deltaTime;

            // 효과음 재생
            if (!audioSource.isPlaying && holdSound != null)
            {
                audioSource.loop = true;
                audioSource.clip = holdSound;
                audioSource.Play();
            }

            // 타이머 완료 시 판자 제거
            if (holdCounter >= holdTime)
            {
                RemovePlank();
            }
        }
    }

    public void OnInteract()
    {
        if (playerEquip != null && playerEquip.HasEquippedCrowBar())
        {
            if (!isRemoving)
            {
                StartRemoving(); // 제거 시작
            }
        }
        else
        {
            Debug.Log("CrowBar가 필요합니다.");
        }
    }

    public string GetInteractPrompt()
    {
        return "F 키를 눌러 나무 판자 제거";
    }

    private void StartRemoving()
    {
        isRemoving = true;
        holdCounter = 0f;
    }

    private void StopRemoving()
    {
        isRemoving = false;

        // 효과음 멈춤
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void RemovePlank()
    {
        StopRemoving();
        Debug.Log("판자가 제거되었습니다.");

        // CrowBar 제거
        if (playerEquip != null)
        {
            playerEquip.RemoveEquippedItem("CrowBar"); // CrowBar 제거 메서드 호출
        }

        Destroy(gameObject); // 판자 제거
    }

    void OnTriggerExit(Collider other)
    {
        // 플레이어가 범위를 벗어나면 제거 중단
        if (other.CompareTag("Player"))
        {
            StopRemoving();
        }
    }
}
