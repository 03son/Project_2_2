using Photon.Pun;
using UnityEngine;

public class SubmarineController : MonoBehaviourPun
{
    private bool isPropellerAttached = false;
    private int attachedBatteries = 0; // 배터리 수
    public int requiredBatteries = 2; // 필요한 배터리 수

    private bool isStarted = false;

    public AudioSource audioSource; // 잠수함 소리
    public AudioClip startSound; // 시동 소리

    PhotonView pv;
    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        pv = GetComponent<PhotonView>();
    }

    public void AttachItem(string itemName)
    {
        if (itemName == "Propeller")
        {
            isPropellerAttached = true;
            Debug.Log("프로펠러가 부착되었습니다.");
        }
        else if (itemName == "Battery")
        {
            attachedBatteries++;
            Debug.Log($"배터리가 부착되었습니다. 현재 배터리 수: {attachedBatteries}/{requiredBatteries}");
        }
    }

    public bool CanStart()
    {
        return isPropellerAttached && attachedBatteries >= requiredBatteries;
    }

    public void Submarine()
    {
        pv.RPC("StartSubmarine", RpcTarget.All);
    }
    [PunRPC]
    public void StartSubmarine()
    {
        if (isStarted)
        {
            Debug.Log("잠수함은 이미 작동 중입니다.");
            return;
        }

        if (CanStart())
        {
            isStarted = true;
            Debug.Log("잠수함 시동 시작!");
            if (audioSource != null && startSound != null)
            {
                audioSource.clip = startSound;
                audioSource.Play();
            }
        }
        else
        {
            Debug.Log("필요한 부품이 모두 부착되지 않았습니다.");
        }
    }
}
