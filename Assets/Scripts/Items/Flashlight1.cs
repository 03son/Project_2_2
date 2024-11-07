using System.Collections;
using UnityEngine;

public class Flashlight1 : MonoBehaviour
{
    [SerializeField] private GameObject flashlightLight; // Spot Light ������Ʈ
    private bool flashlightActive = false;
    private bool isAcquired = false;
    private Transform cameraTransform;
    private Light flashlightComponent;

    [SerializeField] private float intensity = 1f; // �⺻ ���ٽ�Ƽ ��
    [SerializeField] private float range = 10f; // �⺻ ���� ��

    void Start()
    {
        if (flashlightLight == null)
        {
            Debug.LogWarning("Spot Light�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        else
        {
            flashlightComponent = flashlightLight.GetComponent<Light>();
            flashlightComponent.intensity = intensity;
            flashlightComponent.range = range;
            flashlightLight.SetActive(false); // ���� �� ���� ����
        }
        cameraTransform = Camera.main.transform;
    }

    public void AcquireFlashlight()
    {
        isAcquired = true;
        Debug.Log("������ ȹ�� �Ϸ�");
    }

    void Update()
    {
        if (isAcquired && transform.parent == cameraTransform.parent.Find("EquipCamera/EquipItem"))
        {
            // `Main Camera`�� ��ġ�� ȸ���� ����ȭ
            flashlightLight.transform.position = cameraTransform.position;
            flashlightLight.transform.rotation = cameraTransform.rotation;

            // ������ �ѱ�/����
            if (Input.GetMouseButtonDown(0))
                Debug.Log("�۵�");
            {
                flashlightActive = !flashlightActive;
                flashlightLight.SetActive(flashlightActive);
                Debug.Log("������ " + (flashlightActive ? "����" : "����"));
            }

            // ���ٽ�Ƽ �� ���� (Ű����� '+'�� '-' Ű ���)
            if (Input.GetKeyDown(KeyCode.Equals)) // '+' Ű
            {
                intensity += 0.5f;
                flashlightComponent.intensity = intensity;
                Debug.Log("������ ���ٽ�Ƽ ����: " + intensity);
            }
            else if (Input.GetKeyDown(KeyCode.Minus)) // '-' Ű
            {
                intensity = Mathf.Max(0, intensity - 0.5f);
                flashlightComponent.intensity = intensity;
                Debug.Log("������ ���ٽ�Ƽ ����: " + intensity);
            }

            // ���� �� ���� (Ű����� '['�� ']' Ű ���)
            if (Input.GetKeyDown(KeyCode.RightBracket)) // ']' Ű
            {
                range += 1f;
                flashlightComponent.range = range;
                Debug.Log("������ ���� ����: " + range);
            }
            else if (Input.GetKeyDown(KeyCode.LeftBracket)) // '[' Ű
            {
                range = Mathf.Max(1, range - 1f);
                flashlightComponent.range = range;
                Debug.Log("������ ���� ����: " + range);
            }
        }
    }
}
