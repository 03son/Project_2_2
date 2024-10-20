using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCupFunction : ItemFunction, IItemFunction
{
    public Rigidbody glassCupRigidbody;  // 유리컵에 붙어 있는 Rigidbody
    public Collider glassCupCollider;  // 유리컵에 붙어 있는 Collider
    public AudioClip breakSound;  // 유리컵이 깨질 때 나는 소리
    public float throwForce = 10f;  // 던질 때 가할 힘의 크기
    public float upwardForce = 5f;  // 위로 가할 힘의 크기
    public Transform playerCamera;  // 플레이어 카메라 참조

    private bool hasBeenThrown = false;  // 유리컵이 이미 던져졌는지 확인
    public static event System.Action<Vector3> OnGlassBreak;  // 유리병이 깨질 때 이벤트 발생

    void Start()
    {
        if (glassCupRigidbody == null)
        {
            glassCupRigidbody = GetComponent<Rigidbody>();
        }

        if (glassCupCollider == null)
        {
            glassCupCollider = GetComponent<Collider>();  // Collider 자동 할당
        }

        if (glassCupRigidbody == null || glassCupCollider == null)
        {
            Debug.LogError("Rigidbody 또는 Collider가 할당되지 않았습니다!");
        }

        // 플레이어 카메라 참조가 없으면 자동으로 찾아 설정
        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;  // 기본적으로 메인 카메라를 사용
        }
    }

    // 부모 오브젝트가 변경될 때 호출되는 Unity 이벤트
    void OnTransformParentChanged()
    {
        EnsureRigidbodyAndCollider();

        if (transform.parent != null && transform.parent.CompareTag("EquipItem"))
        {
            SetRigidbodyForEquipItem();  // EquipItem에 있을 때 설정
        }
        else
        {
            SetRigidbodyForField();  // 필드에 있을 때 설정
        }
    }

    // Rigidbody와 Collider가 없을 때 자동으로 할당
    void EnsureRigidbodyAndCollider()
    {
        if (glassCupRigidbody == null)
        {
            glassCupRigidbody = gameObject.AddComponent<Rigidbody>();
            glassCupRigidbody.mass = 0.1f;
            glassCupRigidbody.drag = 0.02f;
            glassCupRigidbody.angularDrag = 0.05f;
            glassCupRigidbody.useGravity = true;
            glassCupRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            glassCupRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        if (glassCupCollider == null)
        {
            glassCupCollider = GetComponent<Collider>();
        }
    }

    // EquipItem 상태일 때의 Rigidbody 설정
    void SetRigidbodyForEquipItem()
    {
        glassCupRigidbody.isKinematic = true;
        glassCupRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        glassCupRigidbody.interpolation = RigidbodyInterpolation.None;
        glassCupCollider.isTrigger = false;  // 충돌 감지
    }

    // 필드 상태일 때의 Rigidbody 설정
    void SetRigidbodyForField()
    {
        glassCupRigidbody.isKinematic = false;
        glassCupRigidbody.useGravity = true;
        glassCupRigidbody.mass = 0.1f;
        glassCupRigidbody.drag = 0.02f;
        glassCupRigidbody.angularDrag = 0.05f;
        glassCupRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        glassCupRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        glassCupCollider.isTrigger = false;  // 충돌 감지
    }

    public void Function()
    {
        if (!hasBeenThrown && glassCupRigidbody != null)
        {
            Debug.Log("유리컵 던지기");

            // 던지기 전에 Interpolate 및 Continuous 설정
            glassCupRigidbody.isKinematic = false;
            glassCupRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            glassCupRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            glassCupCollider.isTrigger = false;  // 던질 때 충돌을 위해 Is Trigger 꺼두기

            // 카메라의 방향을 기준으로 유리컵을 포물선으로 던지기 (카메라 방향 + 위로의 힘)
            Vector3 throwDirection = playerCamera.forward * throwForce + playerCamera.up * upwardForce;
            glassCupRigidbody.AddForce(throwDirection, ForceMode.VelocityChange);

            hasBeenThrown = true;

            // 충돌 감지를 위해 Collider 설정 (던진 후 충돌을 감지함)
            glassCupRigidbody.gameObject.AddComponent<ThrownGlassCup>();
        }
    }

    // 충돌 시 유리병이 깨지면서 소리와 이벤트 발생
    void OnCollisionEnter(Collision collision)
    {
        // 유리병이 깨질 때만 사운드와 이벤트 발생
        if (!hasBeenThrown) return;

        // 유리병이 깨진 경우만 이벤트 발생
        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position);
        }

        // 유리병이 깨진 위치를 이벤트로 전달
        if (OnGlassBreak != null)
        {
            OnGlassBreak(transform.position);  // 깨진 위치를 전송
        }

        // 오브젝트 즉시 파괴 (소리 재생 후에도 유리병을 남기지 않음)
        Destroy(gameObject);  // 즉시 오브젝트 파괴
    }

}

