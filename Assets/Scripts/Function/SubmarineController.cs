using Photon.Pun;
using UnityEngine;

public class SubmarineController : MonoBehaviourPun
{
    private bool isPropellerAttached = false;
    private bool isBatteryAttached = false;
    private bool isKeyAttached = false;
    private bool isStarted = false;

    public void AttachedItem(string itemName)
    {
        if (itemName == "Propeller")
        {
            isPropellerAttached = true;
            Debug.Log("Propeller 부착됨");  // Propeller 부착 상태 확인
        }
        else if (itemName == "Battery")
        {
            isBatteryAttached = true;
            Debug.Log("Battery 부착됨");  // Battery 부착 상태 확인
        }
        else if (itemName == "Submarine Key")
        {
            isKeyAttached = true;
            Debug.Log("Submarine Key 부착됨");  // Submarine Key 부착 상태 확인
        }

        // 모든 부품 상태를 한번에 확인하는 로그
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

            // 여기에 애니메이션 호출 또는 씬 전환 등 탈출 시퀀스 코드 추가 가능
            Invoke("EscapeSequence", 3.0f); // 3초 후 탈출 시퀀스 실행
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
