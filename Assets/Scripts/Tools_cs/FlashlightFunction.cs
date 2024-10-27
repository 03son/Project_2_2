using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlighFunction : MonoBehaviour, IItemFunction
{
    public Light flashlight; // ������ ������ �ϴ� Light ������Ʈ
    public KeyCode toggleKey = KeyCode.F;
    private bool hasFlashlight = false; // �������� ȹ���ߴ��� ����

    void Start()
    {
        // �ڽĿ��� Light ������Ʈ�� ã�� �Ҵ� (Inspector���� �Ҵ���� ���� ��쿡��)
        if (flashlight == null)
        {
            flashlight = GetComponentInChildren<Light>();
        }

        // Light ������Ʈ�� �ִٸ� �⺻������ ���� ���·� ����
        if (flashlight != null)
        {
            flashlight.enabled = false;
        }
    }

    void Update()
    {
        // �������� ȹ���� �Ŀ��� �����ϵ��� ��
        if (hasFlashlight && Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight(); // F Ű�� ������ �� ������ �ѱ�/����
        }
    }

    // IItemFunction �������̽��� Function() �޼��� ����
    public void Function()
    {
        ToggleFlashlight(); // ������ �ѱ�/���� ����
    }

    // ������ ȹ�� �� ȣ��Ǵ� �޼���
    public void AcquireFlashlight()
    {
        hasFlashlight = true; // �������� ȹ���� ���·� ����

        // Light ������Ʈ�� ȹ���� �Ŀ��� �ٽ� Ȯ�� �� �Ҵ�
        if (flashlight == null)
        {
            flashlight = GetComponentInChildren<Light>();
        }

        if (flashlight != null)
        {
            flashlight.enabled = false; // ȹ�� �� ���� ���·� ����

            // �������� �θ� ���� ī�޶�� �����Ͽ� ��ġ�� ȸ���� ���󰡰� ��
            flashlight.transform.SetParent(Camera.main.transform);
            flashlight.transform.localPosition = new Vector3(0.3f, -0.3f, 0.5f); // �����տ� �鸰 ��ó�� ��ġ ����
            flashlight.transform.localRotation = Quaternion.Euler(0, 0, 0); // �ʿ��� ȸ�� ����
        }
    }

    // �������� �Ѱ� ���� �޼���
    private void ToggleFlashlight()
    {
        if (flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled; // �� �Ѱ� ����
        }
    }
}
