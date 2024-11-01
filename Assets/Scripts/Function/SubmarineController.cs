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
            Debug.Log("Propeller ������");  // Propeller ���� ���� Ȯ��
        }
        else if (itemName == "Battery")
        {
            isBatteryAttached = true;
            Debug.Log("Battery ������");  // Battery ���� ���� Ȯ��
        }
        else if (itemName == "Submarine Key")
        {
            isKeyAttached = true;
            Debug.Log("Submarine Key ������");  // Submarine Key ���� ���� Ȯ��
        }

        // ��� ��ǰ ���¸� �ѹ��� Ȯ���ϴ� �α�
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
            Debug.Log("������� �õ��Ǿ����ϴ�. Ż�� �������� ���۵˴ϴ�!");

            // ���⿡ �ִϸ��̼� ȣ�� �Ǵ� �� ��ȯ �� Ż�� ������ �ڵ� �߰� ����
            Invoke("EscapeSequence", 3.0f); // 3�� �� Ż�� ������ ����
        }
        else
        {
            Debug.Log("��� ��ǰ�� �������� �ʾҽ��ϴ�. ������� ������ �� �����ϴ�.");
        }
    }

    private void EscapeSequence()
    {
        Debug.Log("Ż�� �������� ���۵Ǿ����ϴ�.");
        // ���⼭ ���� Ż�� �������� ó�� (��: �� ��ȯ, �ִϸ��̼� ��)
    }
}
