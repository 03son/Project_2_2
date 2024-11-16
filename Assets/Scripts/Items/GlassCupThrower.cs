using System.Collections;
using UnityEngine;

public class GlassCupThrower : MonoBehaviour
{
    [Header("Glass Cup Prefab")]
    [SerializeField] private GameObject glassCupPrefab;  // ������ ������ ����

    [Header("Throw Settings")]
    [SerializeField] private KeyCode throwKey = KeyCode.Mouse0;  // ������ Ű
    [SerializeField] private float throwForce = 10f;  // �⺻ ������ ��
    [SerializeField] private float maxForce = 20f;  // �ִ� ������ ��

    [Header("Trajectory Settings")]
    [SerializeField] private LineRenderer trajectoryLine;  // ���� ǥ�� LineRenderer

    private Camera mainCamera;
    private bool isCharging = false;
    private float chargeTime = 0f;

    private bool hasGlassCup = false;  // �������� �����ϰ� �ִ��� ����
    private GameObject currentGlassCup;  // ���� ������ ������ ������Ʈ

    void Start()
    {
        mainCamera = Camera.main;
        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;  // ���� ǥ�� �ʱ�ȭ
        }
    }

    void Update()
    {
        if (hasGlassCup && currentGlassCup != null)
        {
            if (Input.GetKeyDown(throwKey))
            {
                StartThrowing();
            }

            if (isCharging)
            {
                ChargeThrow();
            }

            if (Input.GetKeyUp(throwKey))
            {
                ReleaseThrow();
            }
        }
    }

    public void PickUpGlassCup(GameObject glassCup)
    {
        Debug.Log("������ ȹ�� - ���� �غ� �Ϸ�");
        hasGlassCup = true;  // ������ ȹ�� ���·� ����
        currentGlassCup = glassCup;

        // �������� ���� ��ġ�� �̵���Ŵ
        Transform equipItemTransform = transform.Find("EquipCamera/EquipItem");
        if (equipItemTransform != null)
        {
            Debug.Log("EquipItem Ʈ�������� ã�ҽ��ϴ�.");

            currentGlassCup.transform.SetParent(equipItemTransform); // SetParent�� �����Ͽ� �θ� ����
            currentGlassCup.transform.localPosition = Vector3.zero;
            currentGlassCup.transform.localRotation = Quaternion.identity;
            currentGlassCup.layer = LayerMask.NameToLayer("Equip");

            // Rigidbody ����
            Rigidbody rb = currentGlassCup.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;   // ȹ�� �ÿ��� �������� �ʵ��� ����
                rb.useGravity = false;   // �߷� ��Ȱ��ȭ
                Debug.Log("�������� Rigidbody ���� �Ϸ� - isKinematic: true, useGravity: false");
            }

            // Collider ����
            Collider col = currentGlassCup.GetComponent<Collider>();
            if (col != null)
            {
                col.isTrigger = true;   // �÷��̾�� �浹���� �ʰ� �ϱ� ����
                Debug.Log("�������� Collider ���� �Ϸ� - isTrigger: true");
            }

            Debug.Log("�������� ���� ��ġ�� �̵��Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("EquipItem Ʈ�������� ã�� �� �����ϴ�.");
        }
    }

   public void StartThrowing()
    {
        isCharging = true;
        chargeTime = 0f;

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = true;  // ���� ǥ�� Ȱ��ȭ
        }
    }

    void ChargeThrow()
    {
        chargeTime += Time.deltaTime;
        float currentForce = Mathf.Min(chargeTime * throwForce, maxForce);

        // ���� ������Ʈ
        Vector3 throwDirection = mainCamera.transform.forward;
        Vector3 grenadeVelocity = throwDirection * currentForce;
        ShowTrajectory(mainCamera.transform.position + mainCamera.transform.forward, grenadeVelocity);
    }

    void ReleaseThrow()
    {
        if (currentGlassCup != null)
        {
            float finalForce = Mathf.Min(chargeTime * throwForce, maxForce);
            Rigidbody rb = currentGlassCup.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;   // ���� �� ���������� �����̵��� ����
                rb.useGravity = true;   // �߷� Ȱ��ȭ
                Vector3 throwDirection = mainCamera.transform.forward;
                rb.AddForce(throwDirection * finalForce, ForceMode.VelocityChange);
            }

            hasGlassCup = false;  // ���� �� ������ ���� ���� ����
            currentGlassCup = null;  // ������ ���� ����

            if (trajectoryLine != null)
            {
                trajectoryLine.enabled = false;  // ���� ǥ�� ��Ȱ��ȭ
            }
        }
    }

    void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        if (trajectoryLine == null) return;

        int numPoints = 100;
        Vector3[] points = new Vector3[numPoints];
        trajectoryLine.positionCount = numPoints;

        for (int i = 0; i < numPoints; i++)
        {
            float time = i * 0.1f;
            points[i] = origin + speed * time + 0.5f * Physics.gravity * time * time;
        }

        trajectoryLine.SetPositions(points);
    }
}
