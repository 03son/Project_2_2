using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class WoodenPlank : MonoBehaviour, IInteractable
{
    private Player_Equip playerEquip; // Player_Equip 스크립트 참조
    private bool isRemoving = false; // 제거 진행 여부
    private float holdTime = 2.5f; // 홀딩 시간
    private float holdCounter = 0f; // 현재 누르고 있는 시간

    [SerializeField] private AudioSource audioSource; // AudioSource 컴포넌트
    [SerializeField] private AudioClip holdSound; // 키를 누르고 있을 때 효과음
    [SerializeField] private Image progressBar; // 원형 프로그레스 바 UI

    void Start()
    {
        playerEquip = FindObjectOfType<Player_Equip>(); // Player_Equip 스크립트 찾기
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false); // 초기 비활성화
        }
    }

    void Update()
    {
        // F 키를 누르고 있을 때만 타이머 진행
        if (isRemoving && Input.GetKey(KeyCode.F))
        {
            // 타이머 진행
            holdCounter += Time.deltaTime;

            // UI 업데이트
            if (progressBar != null)
            {
                progressBar.fillAmount = holdCounter / holdTime;
            }

            // 효과음 재생
            if (!audioSource.isPlaying && holdSound != null)
            {
                audioSource.loop = true; // 반복 재생
                audioSource.clip = holdSound;
                audioSource.Play();
            }

            // 타이머 완료 시 판자 제거
            if (holdCounter >= holdTime)
            {
                RemovePlank();
            }
        }
        else if (isRemoving && Input.GetKeyUp(KeyCode.F))
        {
            // 키를 떼면 제거 중단
            StopRemoving();
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

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true); // 타이머 UI 활성화
            progressBar.fillAmount = 0f; // 초기화
        }
    }

    private void StopRemoving()
    {
        isRemoving = false;

        // 효과음 멈춤
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // UI 비활성화
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
        }

        // 타이머 초기화
        holdCounter = 0f;
    }

    private void RemovePlank()
    {
        StopRemoving();
        Debug.Log("판자가 제거되었습니다.");
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                PhotonItem photonItem = player.GetComponent<PhotonItem>();
                if (photonItem != null)
                {
                    photonItem.RemoveEquippedItem("쇠지렛대");
                    Debug.Log($"{"쇠지렛대"} 아이템이 제거되었습니다.");
                }
            }
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
