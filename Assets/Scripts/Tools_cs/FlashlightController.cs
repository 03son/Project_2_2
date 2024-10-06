using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight; // ������ ������ �ϴ� Spotlight ������Ʈ
    public KeyCode toggleKey = KeyCode.F; // �������� �Ѱ� ���� Ű (�⺻: F Ű)
    private bool hasFlashlight = false; // �÷��̾ �������� ȹ���ߴ��� ����
    private bool hasLoggedNoFlashlight = false; // �������� ������ �α׷� ����ߴ��� ����

    void Start()
    {
        if (flashlight == null)
        {
            flashlight = GetComponentInChildren<Light>(); // �ڽ� ������Ʈ���� Light ������Ʈ�� ������
        }

        if (flashlight != null)
        {
            flashlight.enabled = false; // ������ �� �������� ���� ���·� ����
            //Debug.Log("������ �ʱ�ȭ: ���� ���� (" + flashlight.name + ")");
        }
        else
        {
           // Debug.Log("������ Light ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    void Update()
    {
        if (hasFlashlight) // �������� ȹ���� ������ ���� �۵�
        {
            if (!hasLoggedNoFlashlight) // �������� ȹ��Ǿ����� �� �̻� ������� �ʵ���
            {
                hasLoggedNoFlashlight = true;
            }

            if (Input.GetKeyDown(toggleKey)) // ������ Ű�� ������ ��
            {
                Debug.Log("F Ű�� ���Ƚ��ϴ�."); // F Ű�� ������ �� �α� ���

                if (flashlight != null)
                {
                    flashlight.enabled = !flashlight.enabled; // �������� Ȱ��ȭ ���¸� ������Ŵ
                    Debug.Log("������ ���� ����: " + (flashlight.enabled ? "����" : "����"));
                }
                else
                {
                    Debug.Log("flashlight ������ �Ҵ���� �ʾҽ��ϴ�.");
                }
            }
        }
        else
        {
            if (!hasLoggedNoFlashlight) // �������� ���ٴ� �α״� �� ���� ���
            {
               // Debug.Log("�������� ���� ȹ����� �ʾҽ��ϴ�."); // �������� ȹ������ ���� �������� �˸�
                hasLoggedNoFlashlight = true;
            }
        }
    }

    // �������� ȹ������ �� ȣ��Ǵ� �޼���
    public void AcquireFlashlight()
    {
        hasFlashlight = true; // ������ ȹ�� ���·� ����
        Debug.Log("�������� ȹ���߽��ϴ�.");

        if (flashlight != null)
        {
            flashlight.enabled = false; // �������� ȹ�� �Ŀ��� ���� ���·� ����
        }
    }
}









