using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCupCollisionHandler : MonoBehaviour
{
    public Rigidbody glassCupRigidbody;  // 유리컵에 붙어 있는 Rigidbody
    public Collider glassCupCollider;  // 유리컵에 붙어 있는 Collider

    void Start()
    {
        if (glassCupRigidbody == null)
        {
            glassCupRigidbody = GetComponent<Rigidbody>();
        }

        if (glassCupCollider == null)
        {
            glassCupCollider = GetComponent<Collider>();
        }

        if (glassCupRigidbody == null || glassCupCollider == null)
        {
            Debug.LogError("Rigidbody 또는 Collider가 할당되지 않았습니다!");
        }

        // 초기 상태에서 충돌이 가능하도록 설정
        glassCupCollider.isTrigger = false;
    }

    // 부모 오브젝트가 변경될 때 호출되는 Unity 이벤트
    void OnTransformParentChanged()
    {
        if (transform.parent != null && transform.parent.CompareTag("EquipItem"))
        {
            // 플레이어가 아이템을 들고 있을 때는 Kinematic 모드로 설정
            glassCupRigidbody.isKinematic = true;
            glassCupRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            glassCupRigidbody.interpolation = RigidbodyInterpolation.None;
            glassCupCollider.isTrigger = false;  // 충돌을 위해 Is Trigger 비활성화
        }
        else
        {
            // 필드에 있을 때는 물리적 상호작용을 활성화
            glassCupRigidbody.isKinematic = false;
            glassCupRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            glassCupRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            glassCupCollider.isTrigger = false;  // 충돌을 위해 Is Trigger 비활성화
        }
    }
}
