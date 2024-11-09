using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGunFunction : ItemFunction, IItemFunction
{
    public KeyCode fireKey = KeyCode.Mouse0; // ���콺 ��Ŭ������ �߻�
    public float range = 10f; // ���ϰ��� ��Ÿ�
    public float stunDuration = 5f; // ���� 5�� ���� ���߰� ��
    public LayerMask enemyLayer; // �� ���̾� ����
    public LineRenderer lineRenderer; // ���� ������ ������Ʈ �߰�
    public float laserDuration = 0.1f; // �������� ���̴� �ð�

    public AudioClip fireSound; // �߻� �Ҹ�
    private AudioSource audioSource; // ����� �ҽ��� ������ ����

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();

        // ����� �ҽ� �ʱ�ȭ
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // ����� �ҽ��� ���ٸ� �߰�����
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // �߻� �Ҹ� Ŭ�� ����
        audioSource.clip = fireSound;
        audioSource.playOnAwake = false; // ������ �� �ڵ� ������� �ʵ��� ����
    }

    public void Function()
    {
        FireStunGun();
    }

    void FireStunGun()
    {
        // Ŭ�����ڸ��� �߻� �Ҹ� ���
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        Vector3 rayOrigin = playerCamera.transform.position;
        Ray ray = new Ray(rayOrigin, playerCamera.transform.forward);
        RaycastHit hit;

        StartCoroutine(ShowLaser(ray.origin, ray.direction * range));

        if (Physics.Raycast(ray, out hit, range, enemyLayer))
        {
            Stunnable stunnableEnemy = hit.collider.GetComponent<Stunnable>();
            if (stunnableEnemy != null)
            {
                stunnableEnemy.Stun(stunDuration);
            }
        }
    }

    IEnumerator ShowLaser(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, start + end);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        lineRenderer.enabled = false;
    }
}
