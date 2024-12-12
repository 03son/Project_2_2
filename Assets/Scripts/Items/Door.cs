using UnityEngine;
using Photon.Pun;

public class Door : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen = false; // 문이 열려있는 상태
    private bool isUnlocked = false; // 문이 열쇠로 잠겨 있는 상태

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
        if (!isUnlocked) // 문이 잠겨 있는 상태
        {
            if (playerEquip != null && playerEquip.HasEquippedKey())
            {
                photonView.RPC("UnlockDoor", RpcTarget.All); // 모든 클라이언트에서 잠금 해제
                                                             //   playerEquip.photonView.RPC("RemoveEquippedItem", RpcTarget.All, "Key"); // 열쇠 제거
            }
            else
            {
                Debug.Log("문이 잠겨 있습니다. 열쇠가 필요합니다.");
                if (audioSource != null && lockedSound != null)
                {
                    audioSource.PlayOneShot(lockedSound); // 잠긴 소리 재생
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
