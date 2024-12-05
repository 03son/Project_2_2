using System.Collections.Generic;
using UnityEngine;

public class CanHearSoundSource : MonoBehaviour
{
    public LayerMask detectableLayers; // 여러 레이어를 선택할 수 있는 LayerMask
    private List<Collider> objectsInTrigger = new List<Collider>();

    private float checkInterval = 1f; // 검사 주기
    private float timer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        // 여러 레이어 중 하나에 속하는지 확인
        if ((detectableLayers.value & (1 << other.gameObject.layer)) > 0)
        {
            objectsInTrigger.Add(other);
            //Debug.Log($"{other.gameObject.name}이(가) 트리거에 들어옴 (레이어: {LayerMask.LayerToName(other.gameObject.layer)})");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInTrigger.Contains(other))
        {
            objectsInTrigger.Remove(other);
            //Debug.Log($"{other.gameObject.name}이(가) 트리거에서 나감");
        }
    }

    private void Update()
    {
        // 검사 주기에 따라 실행
        timer += Time.deltaTime;
        if (timer < checkInterval)
            return;

        timer = 0f; // 타이머 초기화

        foreach (var obj in objectsInTrigger)
        {
            if (obj == null) continue; // 이미 삭제된 오브젝트는 무시
            // Player 확인 (레이어 이름 확인)
            string layerName = LayerMask.LayerToName(obj.gameObject.layer);
            if (layerName == "Player")
            {
                PlayerState playerState = obj.GetComponent<PlayerState>();
                //Debug.Log(playerState);
                if (playerState != null && playerState.State == PlayerState.playerState.Die)
                {
                    //Debug.Log("플레이어가 사망 상태입니다. 감지 중단.");
                    return;
                }
            }
            // SoundSource 처리
            SoundSource soundSource = obj.GetComponent<SoundSource>();
            if (soundSource != null)
            {
                float decibel = soundSource.GetDecibelAtDistance(transform.position);
                if (decibel >= MonsterAI.Instance.minDecibelToDetect)
                {
                    //Debug.Log($"사운드 소스 감지: {soundSource.gameObject.name}, 데시벨: {decibel}");
                    MonsterAI.Instance.HandleItemSound(obj.transform.position);
                }
            }
        }
    }
}
