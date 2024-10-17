using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGunFunction : ItemFunction, IItemFunction
{
    public KeyCode fireKey = KeyCode.Mouse0; // 마우스 좌클릭으로 발사
    public float range = 10f; // 스턴건의 사거리
    public float stunDuration = 5f; // 적을 5초 동안 멈추게 함
    public LayerMask enemyLayer; // 적 레이어 설정

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy"); // 레이어 초기화
    }

    public void Function()
    {
        Debug.Log("스턴건 작동");
        FireStunGun();
    }

    void FireStunGun()
    {
        Vector3 rayOrigin = playerCamera.transform.position;
        Ray ray = new Ray(rayOrigin, playerCamera.transform.forward);
        RaycastHit hit;

        // 레이캐스트 시각화
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 2f); // 2초 동안 빨간색으로 라인 그리기

        if (Physics.Raycast(ray, out hit, range, enemyLayer))
        {
            Debug.Log("레이캐스트 충돌 오브젝트: " + hit.collider.name + ", 레이어: " + hit.collider.gameObject.layer);

            Stunnable stunnableEnemy = hit.collider.GetComponent<Stunnable>();
            if (stunnableEnemy != null)
            {
                stunnableEnemy.Stun(stunDuration);
                Debug.Log("적 감지됨: " + hit.collider.name);
            }
            else
            {
                Debug.Log("적을 감지했지만 Stunnable 컴포넌트가 없음.");
            }
        }
        else
        {
            Debug.Log("적 감지 실패");
        }
    }
}
