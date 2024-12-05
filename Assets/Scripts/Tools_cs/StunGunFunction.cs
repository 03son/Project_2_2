using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class StunGunFunction : ItemFunction, IItemFunction
{
    public KeyCode fireKey = KeyCode.Mouse0; // 마우스 좌클릭으로 발사
    public float range = 10f; // 스턴건의 사거리
    public float stunDuration = 5f; // 적을 5초 동안 멈추게 함
    public LayerMask enemyLayer; // 적 레이어 설정
    public LineRenderer lineRenderer; // 라인 렌더러 컴포넌트 추가
    public float laserDuration = 1f; // 레이저가 보이는 시간 (값을 더 크게 설정)
    private PhotonItem _PhotonItem; // Player_Equip 참조 추가
    public AudioClip fireSound; // 발사 소리
    private AudioSource audioSource; // 오디오 소스를 저장할 변수

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        _PhotonItem = GetComponentInParent<PhotonItem>();

        // 오디오 소스 초기화
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // 오디오 소스가 없다면 추가해줌
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 발사 소리 클립 설정
        audioSource.clip = fireSound;
        audioSource.playOnAwake = false; // 시작할 때 자동 재생되지 않도록 설정
        audioSource.spatialBlend = 0.0f; // 2D 오디오로 설정 (3D 설정이 문제일 수 있으므로 테스트)
    }

    public void Function()
    {
        FireStunGun();
    }

    void FireStunGun()
    {




        // 클릭하자마자 발사 소리 재생
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
            Debug.Log("발사 소리 재생 시도");
        }
        else
        {
            Debug.LogWarning("오디오 소스 또는 발사 소리가 없습니다.");
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

        // 아이템 제거 부분에 딜레이 추가
        StartCoroutine(RemoveItemAfterDelay());
    }

    IEnumerator ShowLaser(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, start + end);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        lineRenderer.enabled = false;
    }

    IEnumerator RemoveItemAfterDelay()
    {
        yield return new WaitForSeconds(laserDuration); // 레이저가 사라진 후 아이템을 파괴하기 위해 딜레이 추가

        if (_PhotonItem != null && _PhotonItem.photonView != null)
        {
            _PhotonItem.RemoveEquippedItem(GetComponent<ItemObject>().item.ItemName);
            Inventory.instance.RemoveItem(GetComponent<ItemObject>().item.ItemName);
            Destroy(GetComponentInParent<Player_Equip>().Item);
            Tesettext();
            // _PhotonItem.photonView.RPC("RemoveEquippedItem", RpcTarget.All, "Key");
        }
    }
}
