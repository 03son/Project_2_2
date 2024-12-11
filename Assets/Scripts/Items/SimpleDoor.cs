using UnityEngine;
using Photon.Pun; // Photon 네트워킹 관련 네임스페이스 추가

public class SimpleDoor : MonoBehaviour, IInteractable //IPunObservable
{
    private Animator animator;
    private bool isOpen = false; // 문이 열려있는 상태

    [SerializeField] private AudioSource audioSource; // AudioSource 컴포넌트
    [SerializeField] private AudioClip openSound; // 열리는 소리 클립
    [SerializeField] private AudioClip closeSound; // 닫히는 소리 클립

    private PhotonView photonView; // PhotonView 참조

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<Animator>();
        photonView = GetComponentInParent<PhotonView>(); // PhotonView 컴포넌트 가져오기
    }

    public void OnInteract()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("ToggleDoor", RpcTarget.All, !isOpen);
        }
        else
        {
            // 로컬 동작만 수행 (싱글플레이 상태)
            ToggleDoor(!isOpen);
        }
    }

    public string GetInteractPrompt()
    {
        return isOpen ? "문 닫기" : "문 열기"; // 프롬프트 텍스트 반환
    }

    [PunRPC]
    public void ToggleDoor(bool open)
    {
        animator = GetComponentInParent<Animator>();
        if (open) //T
        {
            OpenDoor(); // 문 열기
        }
        else // F
        {
            CloseDoor(); // 문 닫기
        }
        isOpen = open;
    }

    void OpenDoor()
    {
        if (!isOpen) // 중복 실행 방지
        {
            if (animator != null)
            {
                // animator.SetTrigger("isOpen"); // 문 열기 애니메이션 실행
                animator.SetBool("Open",true);
            }

            // 열리는 소리 재생
            if (audioSource != null && openSound != null)
            {
                audioSource.PlayOneShot(openSound);
                Decibel_Bar.instance.Decibel_Value(audioSource.volume, false);
            }
        }
    }

    void CloseDoor()
    {
        if (isOpen) // 중복 실행 방지
        {
            // 닫히는 소리 재생
            if (audioSource != null && closeSound != null)
            {
                audioSource.PlayOneShot(closeSound);
                Decibel_Bar.instance.Decibel_Value(audioSource.volume,false);
            }
            if (animator != null)
            {
                //animator.SetTrigger("isClose"); // 문 닫기 애니메이션 실행
                animator.SetBool("Open", false);
                return;
            }
        }
    }
  
}
