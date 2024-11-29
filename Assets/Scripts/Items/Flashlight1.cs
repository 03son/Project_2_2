using System.Collections;
using UnityEngine;

public class Flashlight1 : MonoBehaviour
{
    [SerializeField] private GameObject flashlightLight; // Spot Light ������Ʈ
    private bool flashlightActive = false;
    private bool isAcquired = false;
    private Transform cameraTransform;
    private Light flashlightComponent;

    [SerializeField] private float intensity = 15f; // �⺻ ���ٽ�Ƽ ��
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
        if (isAcquired)
        {
            Debug.Log("�������� ȹ��� �����Դϴ�.");
        }

        Transform equipCameraTransform = cameraTransform.parent.Find("EquipCamera");
        Transform equipItemTransform = equipCameraTransform != null ? equipCameraTransform.Find("EquipItem") : null;

        if (equipItemTransform != null)
        {
            Debug.Log("ī�޶� �θ��� EquipCamera/EquipItem: " + equipItemTransform.name);
        }
        else
        {
            //Debug.LogWarning("EquipCamera/EquipItem ��θ� ã�� �� �����ϴ�.");
        }

        if (transform.parent != null && transform.parent.name == "EquipItem")
        {
            Debug.Log("�������� �ùٸ� ���� ������ �ֽ��ϴ�.");
        }
        else
        {
            //Debug.Log("�������� �ùٸ� ���� ������ ���� �ʽ��ϴ�.");
        }

        //Debug.Log($"isAcquired ����: {isAcquired}");

        if (isAcquired && transform.parent != null && transform.parent.name == "EquipItem")
        {
            Debug.Log("������ Update ���� ���� ��");

            // `Main Camera`�� ��ġ�� ȸ���� �÷��ö���Ʈ�� ����ȭ
            flashlightLight.transform.position = cameraTransform.position;
            flashlightLight.transform.rotation = cameraTransform.rotation;

            // ���콺 ��Ŭ������ ������ �ѱ�/����
            if (Input.GetMouseButtonDown(0))
            {
                flashlightActive = !flashlightActive;
                flashlightLight.SetActive(flashlightActive);
                Debug.Log("������ " + (flashlightActive ? "����" : "����"));
            }
        }
    }


}
