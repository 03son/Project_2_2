using Photon.Pun;
using UnityEngine;

public class SubmarineStart : MonoBehaviourPun, IInteractable
{
    public SubmarineController submarineController; // Inspector���� ������ �� �ֵ��� public���� ����
    private bool playerInRange = false;

    private void Start()
    {
        if (submarineController == null)
        {
            Debug.LogError("SubmarineController�� �Ҵ���� �ʾҽ��ϴ�. Inspector���� ������ �ּ���.");
        }
    }

    // IInteractable �������̽� ����
    public string GetInteractPrompt()
    {
        return "FŰ�� ���� ����� �õ�";
    }

    public void OnInteract()
    {
        if (submarineController != null && submarineController.CanStart())
        {
            submarineController.StartSubmarine();
            Debug.Log("Ż�� �������� ���۵˴ϴ�! (�ִϸ��̼� ��ü �����)");
        }
        else
        {
            Debug.Log("��� ��ǰ�� �������� �ʾҽ��ϴ�. ������� ������ �� �����ϴ�.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("����� �õ� ������ ����. F Ű�� ���� �õ��� �� �� �ֽ��ϴ�.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("�õ� �������� ���.");
        }
    }
}
