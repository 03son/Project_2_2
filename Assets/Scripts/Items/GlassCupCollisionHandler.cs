using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCupCollisionHandler : MonoBehaviour
{
    public Rigidbody glassCupRigidbody;  // �����ſ� �پ� �ִ� Rigidbody
    public Collider glassCupCollider;  // �����ſ� �پ� �ִ� Collider

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
            Debug.LogError("Rigidbody �Ǵ� Collider�� �Ҵ���� �ʾҽ��ϴ�!");
        }

        // �ʱ� ���¿��� �浹�� �����ϵ��� ����
        glassCupCollider.isTrigger = false;
    }

    // �θ� ������Ʈ�� ����� �� ȣ��Ǵ� Unity �̺�Ʈ
    void OnTransformParentChanged()
    {
        if (transform.parent != null && transform.parent.CompareTag("EquipItem"))
        {
            // �÷��̾ �������� ��� ���� ���� Kinematic ���� ����
            glassCupRigidbody.isKinematic = true;
            glassCupRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            glassCupRigidbody.interpolation = RigidbodyInterpolation.None;
            glassCupCollider.isTrigger = false;  // �浹�� ���� Is Trigger ��Ȱ��ȭ
        }
        else
        {
            // �ʵ忡 ���� ���� ������ ��ȣ�ۿ��� Ȱ��ȭ
            glassCupRigidbody.isKinematic = false;
            glassCupRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            glassCupRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            glassCupCollider.isTrigger = false;  // �浹�� ���� Is Trigger ��Ȱ��ȭ
        }
    }
}
