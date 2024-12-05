using UnityEngine;

public class CanHearVoiceSource : MonoBehaviour
{
    private float checkInterval = 1f; // 1초 간격
    private float timer = 0f;
    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;

        if (timer < checkInterval)
            return;
        if (!other.CompareTag("Player"))
            return; // 플레이어가 아니면 무시
        Mic mic = other.GetComponentInChildren<Mic>();
        if (mic != null)
        {
            float decibel = mic.GetDecibelAtDistance(transform.position);
            //Debug.Log($"목소리 감지: {other.gameObject.name}, 데시벨: {decibel}");
            if (decibel >= MonsterAI.Instance.minDecibelToDetect)
            {
                MonsterAI.Instance.HandlePlayerSound(decibel, other.transform.position);
            }

        }
        timer = 0f; // 타이머 초기화
    }
}
