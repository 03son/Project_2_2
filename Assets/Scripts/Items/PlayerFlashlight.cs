using UnityEngine;
using Photon.Pun;

public class PlayerFlashlight : MonoBehaviourPun
{
    private Light flashlightLight; // PlayerFlashlight�� Light ������Ʈ

    [SerializeField] private Transform headTransform; // Head ������Ʈ ����
    [SerializeField] private float maxDistance = 10f; // ���� �ִ� �Ÿ�
    [SerializeField] private float minSpotAngle = 30f; // �ּ� ����
    [SerializeField] private float maxSpotAngle = 80f; // �ִ� ����
    [SerializeField] private float minIntensity = 1f; // �ּ� ����
    [SerializeField] private float maxIntensity = 3f; // �ִ� ����
    private float smoothTime = 0.1f; // �ε巯�� ��ȭ �ð�

    private float currentSpotAngle; // ���� ���� ����
    private float currentIntensity; // ���� ����

    private void Awake()
    {
        // Light ������Ʈ �ʱ�ȭ
        flashlightLight = GetComponent<Light>();
        if (flashlightLight == null)
        {
            Debug.LogError("PlayerFlashlight�� Light ������Ʈ�� �����ϴ�!");
        }
        else
        {
            flashlightLight.enabled = false; // �⺻ ���� ���·� ����
            currentSpotAngle = maxSpotAngle; // �ʱ� ����
            currentIntensity = maxIntensity; // �ʱ� ����
        }

        if (headTransform == null)
        {
            // Head ������Ʈ�� ã�� �ʱ�ȭ
            Transform head = transform.root.Find("ĳ���͸𵨸�/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/Head");
            if (head != null)
            {
                headTransform = head;
            }
            else
            {
                Debug.LogError("Head ������Ʈ�� ã�� �� �����ϴ�!");
            }
        }
    }
    void Start()
    {
        if (photonView.IsMine) // ���� �÷��̾���
        {
            GetComponent<Light>().enabled = false; // ����Ʈ ����
        }
    }


    private void Update()
    {
        if (flashlightLight != null && flashlightLight.enabled)
        {
            AdjustFlashlight();
        }
    }

    private void AdjustFlashlight()
    {
        if (headTransform == null) return;

        Ray ray = new Ray(headTransform.position, headTransform.forward); // Head ���� ����
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            float distance = hit.distance;
            float t = distance / maxDistance;

            float targetSpotAngle = Mathf.Lerp(minSpotAngle, maxSpotAngle, t);
            float targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            flashlightLight.spotAngle = Mathf.SmoothDamp(flashlightLight.spotAngle, targetSpotAngle, ref currentSpotAngle, smoothTime);
            flashlightLight.intensity = Mathf.SmoothDamp(flashlightLight.intensity, targetIntensity, ref currentIntensity, smoothTime);
        }
        else
        {
            flashlightLight.spotAngle = Mathf.SmoothDamp(flashlightLight.spotAngle, maxSpotAngle, ref currentSpotAngle, smoothTime);
            flashlightLight.intensity = Mathf.SmoothDamp(flashlightLight.intensity, maxIntensity, ref currentIntensity, smoothTime);
        }
    }

    public void SetFlashlightState(bool isActive)
    {
        if (photonView.IsMine)
        {
            // ���ÿ��� ���� ����
            SetLocalFlashlightState(isActive);

            // RPC�� �ٸ� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("SyncFlashlightState", RpcTarget.Others, isActive);
        }
    }

    private void SetLocalFlashlightState(bool isActive)
    {
        if (flashlightLight != null)
        {
            if (!photonView.IsMine) // ���� �÷��̾ �ƴ϶�鸸 ���¸� ����
            {
                flashlightLight.enabled = isActive; // �� Ȱ��ȭ/��Ȱ��ȭ
                Debug.Log("PlayerFlashlight ���� ����: " + (isActive ? "����" : "����"));
            }
            else
            {
                Debug.Log("���� �÷��̾�� PlayerFlashlight�� ������� �ʽ��ϴ�.");
            }
        }
    }


    [PunRPC]
    private void SyncFlashlightState(bool isActive)
    {
        // �ٸ� Ŭ���̾�Ʈ���� ����ȭ�� ���� ����
        SetLocalFlashlightState(isActive);
    }
}
