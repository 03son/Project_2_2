using UnityEngine;
using Photon.Pun;
using TMPro;
public class Door : MonoBehaviourPunCallbacks, IInteractable
{
    private Animator animator;
    private bool isOpen = false; // 문이 열려있는 상태
    private bool isUnlocked = false; // 문이 열쇠로 잠겨 있는 상태
    private PhotonItem _PhotonItem; 
    [SerializeField] private AudioSource audioSource; // AudioSource 컴포넌트
    [SerializeField] private AudioClip openSound; // 열리는 소리 클립
    [SerializeField] private AudioClip closeSound; // 닫히는 소리 클립
    [SerializeField] private AudioClip lockedSound; // 잠긴 소리 클립

    private PhotonView photonView;
    private Player_Equip playerEquip; // 플레이어의 장비 스크립트 참조

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<Animator>();
        photonView = GetComponentInParent<PhotonView>();
        playerEquip = FindObjectOfType<Player_Equip>();
       
    }

    public void OnInteract()
    {
        if (!isUnlocked) // 문이 잠긴 상태
        {
            if (playerEquip != null && playerEquip.HasEquippedKey())
            {
                // 문 잠금 해제
                photonView.RPC("UnlockDoor", RpcTarget.All);

                // 플레이어 장비에서 키 제거
                playerEquip.photonView.RPC("RemoveEquippedItem", RpcTarget.All, "Key");

                // UI 갱신
                GameObject.Find("ItemName_Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = "";

                Debug.Log("문의 잠금을 해제했습니다.");
            }
            else
            {
                // 잠긴 문 알림 및 사운드 재생
                Debug.Log("잠긴 문입니다. 열쇠가 필요합니다.");
                if (audioSource != null && lockedSound != null)
                {
                    audioSource.PlayOneShot(lockedSound);
                }
                return;
            }
        }


        // 문이 잠겨 있지 않다면 열거나 닫기
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("ToggleDoor", RpcTarget.All, !isOpen);
        }
        else
        {
            ToggleDoor(!isOpen);
        }
    }

    public string GetInteractPrompt()
    {
        if (!isUnlocked)
        {
            return "문 열기 (잠김)";
        }
        return isOpen ? "문 닫기" : "문 열기";
    }

    [PunRPC]
    public void ToggleDoor(bool open)
    {
        if (open)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
        isOpen = open;
    }

    [PunRPC]
    public void UnlockDoor()
    {
        isUnlocked = true; // 잠금 상태 해제
        Debug.Log("문이 열쇠로 잠금 해제되었습니다.");
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            if (animator != null)
            {
                animator.SetBool("Open", true);
            }

            if (audioSource != null && openSound != null)
            {
                audioSource.PlayOneShot(openSound);
            }
        }
    }

    void CloseDoor()
    {
        if (isOpen)
        {
            if (animator != null)
            {
                animator.SetBool("Open", false);
            }

            if (audioSource != null && closeSound != null)
            {
                audioSource.PlayOneShot(closeSound);
            }
        }
    }
}
