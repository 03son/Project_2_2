using Photon.Pun;
using UnityEngine;

public class SubmarineController : MonoBehaviourPun
{
    private bool isPropellerAttached = false;
    private bool isBatteryAttached = false;
    private bool isKeyAttached = false;
    private bool isStarted = false;

    public AudioSource audioSource; // 잠수함 시동 소리를 위한 AudioSource
    public AudioClip startSound; // 잠수함 시동 소리 클립
    [Range(0f, 1f)]
    public float volume = 1.0f; // 음량 조절 변수

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource가 없으면 추가
        }
        audioSource.volume = volume; // 초기 음량 설정
    }

    public void AttachedItem(string itemName)
    {
        if (itemName == "Propeller")
        {
            isPropellerAttached = true;
            Debug.Log("Propeller 부착됨");
        }
        else if (itemName == "Battery")
        {
            isBatteryAttached = true;
            Debug.Log("Battery 부착됨");
        }
        else if (itemName == "Submarine Key")
        {
            isKeyAttached = true;
            Debug.Log("Submarine Key 부착됨");
        }

        Debug.Log($"Current Attach Status - Propeller: {isPropellerAttached}, Battery: {isBatteryAttached}, Key: {isKeyAttached}");
    }

    public bool CanStart()
    {
        Debug.Log($"Checking CanStart - Propeller: {isPropellerAttached}, Battery: {isBatteryAttached}, Key: {isKeyAttached}");
        return isPropellerAttached && isBatteryAttached && isKeyAttached;
    }

    public void StartSubmarine()
    {
        if (isStarted) return;

        if (CanStart())
        {
            isStarted = true;
            Debug.Log("잠수함이 시동되었습니다. 탈출 시퀀스가 시작됩니다!");

            // 시동 소리 재생
            if (audioSource != null && startSound != null)
            {
                audioSource.clip = startSound;
                audioSource.volume = volume; // 설정된 음량 사용
                audioSource.Play();
            }

            // 3초 후 탈출 시퀀스 실행
            Invoke("EscapeSequence", 3.0f);
        }
        else
        {
            Debug.Log("모든 부품이 부착되지 않았습니다. 잠수함을 시작할 수 없습니다.");
        }
    }

    private void EscapeSequence()
    {
        Debug.Log("탈출 시퀀스가 시작되었습니다.");
        // 여기서 실제 탈출 시퀀스를 처리 (예: 씬 전환, 애니메이션 등)
    }
}
