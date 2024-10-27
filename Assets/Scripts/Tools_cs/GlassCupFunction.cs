using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCupFunction : ItemFunction, IItemFunction
{
    public Rigidbody glassCupRigidbody;  // �����ſ� �پ� �ִ� Rigidbody
    public Collider glassCupCollider;  // �����ſ� �پ� �ִ� Collider
    public AudioClip breakSound;  // �������� ���� �� ���� �Ҹ�
    public float throwForce = 10f;  // ���� �� ���� ���� ũ��
    public float upwardForce = 5f;  // ���� ���� ���� ũ��
    public Transform playerCamera;  // �÷��̾� ī�޶� ����

    private bool hasBeenThrown = false;  // �������� �̹� ���������� Ȯ��
    public static event System.Action<Vector3> OnGlassBreak;  // �������� ���� �� �̺�Ʈ �߻�

    void Start()
    {
        if (glassCupRigidbody == null)
        {
            glassCupRigidbody = GetComponent<Rigidbody>();
        }

        if (glassCupCollider == null)
        {
            glassCupCollider = GetComponent<Collider>();  // Collider �ڵ� �Ҵ�
        }

        if (glassCupRigidbody == null || glassCupCollider == null)
        {
            Debug.LogError("Rigidbody �Ǵ� Collider�� �Ҵ���� �ʾҽ��ϴ�!");
        }

        // �÷��̾� ī�޶� ������ ������ �ڵ����� ã�� ����
        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;  // �⺻������ ���� ī�޶� ���
        }
    }

    // �θ� ������Ʈ�� ����� �� ȣ��Ǵ� Unity �̺�Ʈ
    void OnTransformParentChanged()
    {
        EnsureRigidbodyAndCollider();

        if (transform.parent != null && transform.parent.CompareTag("EquipItem"))
        {
            SetRigidbodyForEquipItem();  // EquipItem�� ���� �� ����
        }
        else
        {
            SetRigidbodyForField();  // �ʵ忡 ���� �� ����
        }
    }

    // Rigidbody�� Collider�� ���� �� �ڵ����� �Ҵ�
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

    // EquipItem ������ ���� Rigidbody ����
    void SetRigidbodyForEquipItem()
    {
        glassCupRigidbody.isKinematic = true;
        glassCupRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        glassCupRigidbody.interpolation = RigidbodyInterpolation.None;
        glassCupCollider.isTrigger = false;  // �浹 ����
    }

    // �ʵ� ������ ���� Rigidbody ����
    void SetRigidbodyForField()
    {
        glassCupRigidbody.isKinematic = false;
        glassCupRigidbody.useGravity = true;
        glassCupRigidbody.mass = 0.1f;
        glassCupRigidbody.drag = 0.02f;
        glassCupRigidbody.angularDrag = 0.05f;
        glassCupRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        glassCupRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        glassCupCollider.isTrigger = false;  // �浹 ����
    }

    public void Function()
    {
        if (!hasBeenThrown && glassCupRigidbody != null)
        {
            Debug.Log("������ ������");

            // ������ ���� Interpolate �� Continuous ����
            glassCupRigidbody.isKinematic = false;
            glassCupRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            glassCupRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            glassCupCollider.isTrigger = false;  // ���� �� �浹�� ���� Is Trigger ���α�

            // ī�޶��� ������ �������� �������� ���������� ������ (ī�޶� ���� + ������ ��)
            Vector3 throwDirection = playerCamera.forward * throwForce + playerCamera.up * upwardForce;
            glassCupRigidbody.AddForce(throwDirection, ForceMode.VelocityChange);

            hasBeenThrown = true;

            // �浹 ������ ���� Collider ���� (���� �� �浹�� ������)
            glassCupRigidbody.gameObject.AddComponent<ThrownGlassCup>();
        }
    }

    // �浹 �� �������� �����鼭 �Ҹ��� �̺�Ʈ �߻�
    void OnCollisionEnter(Collision collision)
    {
        // �������� ���� ���� ����� �̺�Ʈ �߻�
        if (!hasBeenThrown) return;

        // �������� ���� ��츸 �̺�Ʈ �߻�
        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position);
        }

        // �������� ���� ��ġ�� �̺�Ʈ�� ����
        if (OnGlassBreak != null)
        {
            OnGlassBreak(transform.position);  // ���� ��ġ�� ����
        }

        // ������Ʈ ��� �ı� (�Ҹ� ��� �Ŀ��� �������� ������ ����)
        Destroy(gameObject);  // ��� ������Ʈ �ı�
    }

}

