using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGunFunction : ItemFunction, IItemFunction
{
    public KeyCode fireKey = KeyCode.Mouse0; // ���콺 ��Ŭ������ �߻�
    public float range = 10f; // ���ϰ��� ��Ÿ�
    public float stunDuration = 5f; // ���� 5�� ���� ���߰� ��
    public LayerMask enemyLayer; // �� ���̾� ����

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy"); // ���̾� �ʱ�ȭ
    }

    public void Function()
    {
        Debug.Log("���ϰ� �۵�");
        FireStunGun();
    }

    void FireStunGun()
    {
        Vector3 rayOrigin = playerCamera.transform.position;
        Ray ray = new Ray(rayOrigin, playerCamera.transform.forward);
        RaycastHit hit;

        // ����ĳ��Ʈ �ð�ȭ
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 2f); // 2�� ���� ���������� ���� �׸���

        if (Physics.Raycast(ray, out hit, range, enemyLayer))
        {
            Debug.Log("����ĳ��Ʈ �浹 ������Ʈ: " + hit.collider.name + ", ���̾�: " + hit.collider.gameObject.layer);

            Stunnable stunnableEnemy = hit.collider.GetComponent<Stunnable>();
            if (stunnableEnemy != null)
            {
                stunnableEnemy.Stun(stunDuration);
                Debug.Log("�� ������: " + hit.collider.name);
            }
            else
            {
                Debug.Log("���� ���������� Stunnable ������Ʈ�� ����.");
            }
        }
        else
        {
            Debug.Log("�� ���� ����");
        }
    }
}
