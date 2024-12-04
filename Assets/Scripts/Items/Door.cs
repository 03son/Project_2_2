using Photon.Pun;
using UnityEngine;

public class Door : MonoBehaviourPunCallbacks, IInteractable
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
            photonView.RPC("RPC_OpenDoor", RpcTarget.All); // 문 열기 동기화
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

            playerEquip.photonView.RPC("RemoveEquippedItem", RpcTarget.All, "Key");


            // 문 잠금 해제 상태를 모든 클라이언트에 동기화
            photonView.RPC("RPC_UnlockDoor", RpcTarget.All);

            return;
            isOpen = true;
            Debug.Log("문의 잠금을 해제했습니다.");
            openDoor = true;

            playerEquip.photonView.RPC("RemoveEquippedItem", RpcTarget.All, "Key");


            // 문 잠금 해제 상태를 모든 클라이언트에 동기화
            photonView.RPC("RPC_UnlockDoor", RpcTarget.All);

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
            photonView.RPC("RPC_CloseDoor", RpcTarget.All); // 문 닫기 동기화
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

    [PunRPC]
    void RPC_OpenDoor()
    {
        if (animator != null)
        {
            animator.SetBool("Open", true); // 문 열기 애니메이션 실행
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

    [PunRPC]
    void RPC_CloseDoor()
    {
        if (animator != null)
        {
            animator.SetBool("Open", false); // 문 닫기 애니메이션 실행
        }
        else
        {
            transform.position -= -transform.forward * 1.5f; // 문을 왼쪽으로 약간 이동
        }
    }

    [PunRPC]
    void RPC_UnlockDoor()
    {
        isOpen = true;
    }
}