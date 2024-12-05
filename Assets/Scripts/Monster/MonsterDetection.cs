using Photon.Pun;
using UnityEngine;

public class MonsterDetection : MonoBehaviour
{
    public AudioSource warningSound; // 경고음 오디오 소스
    public float detectionRange = 20f; // 감지 범위
    public float fieldOfView = 120f; // 시야각
    public float cooldownTime = 20f; // 경고음 쿨타임 (초 단위)

    private Transform player; // 로컬 플레이어 Transform
    private bool isWarningPlayed = false;
    private float lastWarningTime = -Mathf.Infinity; // 마지막으로 경고음 재생된 시간

    void Start()
    {
        // 로컬 플레이어 찾기
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj.GetComponent<PhotonView>()?.IsMine == true) // 로컬 플레이어인지 확인
            {
                player = obj.transform;
                break;
            }
        }

        if (player == null)
        {
            Debug.LogError("로컬 플레이어를 찾을 수 없습니다. 'Player' 태그와 PhotonView 설정을 확인하세요.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            DetectPlayer();
        }
    }

    void DetectPlayer()
    {

        if (player == null) return;

        // PlayerState 컴포넌트를 가져옴
        PlayerState playerState = player.GetComponent<PlayerState>();

        // 플레이어가 사망 상태라면 감지 로직 중단
        if (playerState != null && playerState.State == PlayerState.playerState.Die)
        {
            //Debug.Log("플레이어가 사망 상태입니다. 감지 중단.");
            return;
        }

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 시야각 내에 있는지 확인
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (distanceToPlayer <= detectionRange && angleToPlayer <= fieldOfView / 2)
        {
            // 경고음 쿨타임 확인
            if (Time.time >= lastWarningTime + cooldownTime)
            {
                warningSound.Play();
                lastWarningTime = Time.time; // 마지막 경고음 재생 시간 갱신
                isWarningPlayed = true;

                Debug.Log("플레이어가 발각되었습니다!");
            }
        }
        else
        {
            isWarningPlayed = false; // 경고음 상태 초기화
        }
    }
}
