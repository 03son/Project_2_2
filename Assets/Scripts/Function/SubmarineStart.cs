using Photon.Pun;
using UnityEngine;

public class SubmarineStart : MonoBehaviourPun, IInteractable
{
    public SubmarineController submarineController; // Inspector에서 연결할 수 있도록 public으로 설정
    private bool playerInRange = false;

    private void Start()
    {
        if (submarineController == null)
        {
            Debug.LogError("SubmarineController가 할당되지 않았습니다. Inspector에서 연결해 주세요.");
        }
    }

    // IInteractable 인터페이스 구현
    public string GetInteractPrompt()
    {
        return "F키를 눌러 잠수함 시동";
    }

    public void OnInteract()
    {
        if (submarineController != null && submarineController.CanStart())
        {
            submarineController.StartSubmarine();
            Debug.Log("탈출 시퀀스가 시작됩니다! (애니메이션 대체 디버그)");
        }
        else
        {
            Debug.Log("모든 부품이 부착되지 않았습니다. 잠수함을 시작할 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("잠수함 시동 지점에 도달. F 키를 눌러 시동을 걸 수 있습니다.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("시동 지점에서 벗어남.");
        }
    }
}
