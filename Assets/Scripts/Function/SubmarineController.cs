using Photon.Pun;
using UnityEngine;

public class SubmarineController : MonoBehaviourPun
{
    private bool isPropellerAttached = false;
    private bool isBatteryAttached = false;
    private bool isKeyAttached = false;
    private bool isStarted = false;

    public AudioSource audioSource; // ����� �õ� �Ҹ��� ���� AudioSource
    public AudioClip startSound; // ����� �õ� �Ҹ� Ŭ��
    [Range(0f, 1f)]
    public float volume = 1.0f; // ���� ���� ����

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource�� ������ �߰�
        }
        audioSource.volume = volume; // �ʱ� ���� ����
    }

    public void AttachedItem(string itemName)
    {
        if (itemName == "Propeller")
        {
            isPropellerAttached = true;
            Debug.Log("Propeller ������");
        }
        else if (itemName == "Battery")
        {
            isBatteryAttached = true;
            Debug.Log("Battery ������");
        }
        else if (itemName == "Submarine Key")
        {
            isKeyAttached = true;
            Debug.Log("Submarine Key ������");
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
            Debug.Log("������� �õ��Ǿ����ϴ�. Ż�� �������� ���۵˴ϴ�!");

            // �õ� �Ҹ� ���
            if (audioSource != null && startSound != null)
            {
                audioSource.clip = startSound;
                audioSource.volume = volume; // ������ ���� ���
                audioSource.Play();
            }

            // 3�� �� Ż�� ������ ����
            Invoke("EscapeSequence", 3.0f);
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
