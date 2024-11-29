using System.Collections;
using UnityEngine;

public class Flashlight1 : MonoBehaviour
{
    [SerializeField] private GameObject flashlightLight; // Spot Light ������Ʈ
    private bool flashlightActive = false;
    private bool isAcquired = false;
    private Transform cameraTransform;
    private Light flashlightComponent;
    private Animator animator; // Animator ������Ʈ �߰�

    [SerializeField] private float intensity = 15f; // �⺻ ���ٽ�Ƽ ��
    [SerializeField] private float range = 10f; // �⺻ ���� ��
    [SerializeField] private float minSpotAngle = 30f; // ���� ���� ���� ���� ����
    [SerializeField] private float maxSpotAngle = 80f; // ���� ���� ���� ���� ����
    [SerializeField] private float minIntensity = 1f; // ����� ���� �� ����
    [SerializeField] private float maxIntensity = 3f; // �� ���� �� ����
    [SerializeField] private float maxDistance = 10f; // �ִ� �Ÿ�

    private float currentSpotAngle; // ���� ���� ����
    private float currentIntensity; // ���� ���� ����
    private float smoothTime = 0.1f; // �ε巴�� ��ȭ�ϴ� �ð�

    private void Start()
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
            flashlightLight.SetActive(false);; // ���� �� ���� ����
            currentSpotAngle = maxSpotAngle; // �ʱ� ���� ����
            currentIntensity = maxIntensity; // �ʱ� ���� ����
        }
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������

        if (animator == null)
        {
            Debug.LogError("Animator ������Ʈ�� ã�� �� �����ϴ�. ������ ������Ʈ�� Animator�� �־�� �մϴ�.");
        }
    }

    public void AcquireFlashlight()
    {
        isAcquired = true;
        Debug.Log("������ ȹ�� �Ϸ�");
    }

    // ������ �ѱ�/���� ���� ��ȯ �޼��� �߰�
  /*  public void ToggleFlashlight(bool state)
    {
        flashlightActive = state;
        flashlightLight.SetActive(flashlightActive);
        Debug.Log("������ " + (flashlightActive ? "����" : "����"));
    }  */

    public bool IsFlashlightActive()
    {
        return flashlightActive;
    }

    void Update()
    {
        if (transform.parent != null && transform.parent.name != "handitemattach")
        {
            // �������� �� �̻� ������ ���°� �ƴ� ��� ����
            if (flashlightActive)
            {
                flashlightActive = false;
                flashlightLight.SetActive(false);
                Debug.Log("�������� ���� �����Ǿ� �ڵ����� �������ϴ�.");

                // �ִϸ��̼� ���� �ʱ�ȭ
                if (animator != null)
                {
                    animator.SetBool("isFlashlightOn", false);
                }
            }
            return; // �� �̻� Update���� �ٸ� �۾��� ���� �ʵ��� ��
        }

        // �Ʒ��� �������� ������ ������ ���� �۵�
        if (isAcquired && transform.parent != null && transform.parent.name == "handitemattach")
        {
            // ī�޶� ��ġ�� ȸ���� ����� ����ȭ
            flashlightLight.transform.position = cameraTransform.position;
            flashlightLight.transform.rotation = cameraTransform.rotation;

            // ���콺 ��Ŭ������ ������ �ѱ�/����
            if (Input.GetMouseButtonDown(0))
            {
                flashlightActive = !flashlightActive;
                flashlightLight.SetActive(flashlightActive);
                Debug.Log("������ " + (flashlightActive ? "����" : "����"));

                // ������ �Ѱ� ���� �ִϸ��̼� ����
                if (animator != null)
                {
                    animator.SetBool("isFlashlightOn", flashlightActive);
                }
            }

            if (flashlightActive)
            {
                AdjustFlashlight(); // �������� �� ����
            }
        }
    }


    private void AdjustFlashlight()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        // ����ĳ��Ʈ�� ���� ���̳� ��ü�� �ε������� Ȯ��
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            float distance = hit.distance;

            // �Ÿ��� ���� Spot Angle�� Intensity ����
            float t = distance / maxDistance; // �Ÿ��� ���� ���� ��� (0���� 1 ����)

            float targetSpotAngle = Mathf.Lerp(minSpotAngle, maxSpotAngle, t);
            float targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            // �ε巴�� ��ȭ��Ű��
            flashlightComponent.spotAngle = Mathf.SmoothDamp(flashlightComponent.spotAngle, targetSpotAngle, ref currentSpotAngle, smoothTime);
            flashlightComponent.intensity = Mathf.SmoothDamp(flashlightComponent.intensity, targetIntensity, ref currentIntensity, smoothTime);
        }
        else
        {
            // ���� �ε����� �ʾ��� �� �⺻ �ִ� �� ���
            float targetSpotAngle = maxSpotAngle;
            float targetIntensity = maxIntensity;

            flashlightComponent.spotAngle = Mathf.SmoothDamp(flashlightComponent.spotAngle, targetSpotAngle, ref currentSpotAngle, smoothTime);
            flashlightComponent.intensity = Mathf.SmoothDamp(flashlightComponent.intensity, targetIntensity, ref currentIntensity, smoothTime);
        }
    }
}
