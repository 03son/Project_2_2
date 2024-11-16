using System.Collections;
using UnityEngine;

public class GlassCupThrower : MonoBehaviour
{
    [Header("Glass Cup Prefab")]
    [SerializeField] private GameObject glassCupPrefab;  // 유리컵 프리팹 참조

    [Header("Throw Settings")]
    [SerializeField] private KeyCode throwKey = KeyCode.Mouse0;  // 던지기 키
    [SerializeField] private float throwForce = 10f;  // 기본 던지는 힘
    [SerializeField] private float maxForce = 20f;  // 최대 던지는 힘

    [Header("Trajectory Settings")]
    [SerializeField] private LineRenderer trajectoryLine;  // 궤적 표시 LineRenderer

    private Camera mainCamera;
    private bool isCharging = false;
    private float chargeTime = 0f;

    private bool hasGlassCup = false;  // 유리컵을 소지하고 있는지 여부
    private GameObject currentGlassCup;  // 현재 소지한 유리컵 오브젝트

    void Start()
    {
        mainCamera = Camera.main;
        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = false;  // 궤적 표시 초기화
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
        Debug.Log("유리컵 획득 - 던질 준비 완료");
        hasGlassCup = true;  // 유리컵 획득 상태로 변경
        currentGlassCup = glassCup;

        // 유리컵을 장착 위치로 이동시킴
        Transform equipItemTransform = transform.Find("EquipCamera/EquipItem");
        if (equipItemTransform != null)
        {
            Debug.Log("EquipItem 트랜스폼을 찾았습니다.");

            currentGlassCup.transform.SetParent(equipItemTransform); // SetParent로 변경하여 부모 설정
            currentGlassCup.transform.localPosition = Vector3.zero;
            currentGlassCup.transform.localRotation = Quaternion.identity;
            currentGlassCup.layer = LayerMask.NameToLayer("Equip");

            // Rigidbody 설정
            Rigidbody rb = currentGlassCup.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;   // 획득 시에는 움직이지 않도록 설정
                rb.useGravity = false;   // 중력 비활성화
                Debug.Log("유리컵의 Rigidbody 설정 완료 - isKinematic: true, useGravity: false");
            }

            // Collider 설정
            Collider col = currentGlassCup.GetComponent<Collider>();
            if (col != null)
            {
                col.isTrigger = true;   // 플레이어와 충돌하지 않게 하기 위함
                Debug.Log("유리컵의 Collider 설정 완료 - isTrigger: true");
            }

            Debug.Log("유리컵이 장착 위치로 이동되었습니다.");
        }
        else
        {
            Debug.LogWarning("EquipItem 트랜스폼을 찾을 수 없습니다.");
        }
    }

   public void StartThrowing()
    {
        isCharging = true;
        chargeTime = 0f;

        if (trajectoryLine != null)
        {
            trajectoryLine.enabled = true;  // 궤적 표시 활성화
        }
    }

    void ChargeThrow()
    {
        chargeTime += Time.deltaTime;
        float currentForce = Mathf.Min(chargeTime * throwForce, maxForce);

        // 궤적 업데이트
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
                rb.isKinematic = false;   // 던질 때 물리적으로 움직이도록 설정
                rb.useGravity = true;   // 중력 활성화
                Vector3 throwDirection = mainCamera.transform.forward;
                rb.AddForce(throwDirection * finalForce, ForceMode.VelocityChange);
            }

            hasGlassCup = false;  // 던진 후 유리컵 소지 상태 해제
            currentGlassCup = null;  // 유리컵 참조 해제

            if (trajectoryLine != null)
            {
                trajectoryLine.enabled = false;  // 궤적 표시 비활성화
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
