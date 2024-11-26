using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour
{
    public float baseDecibel = 50f;  // 기본 데시벨 값
    public float range = 20f;        // 소리의 최대 범위
    public AudioSource audioSource; // 오디오 소스

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource.clip == null)
        {
            Debug.LogError("AudioClip이 설정되지 않았습니다.");
        }
    }

    // 특정 위치에서의 데시벨 계산
    public float GetDecibelAtDistance(Vector3 position)
    {
        if (audioSource.isPlaying)
        {
            float distance = Vector3.Distance(transform.position, position);

            // 데시벨 계산 (거리 기반 감소)
            float decibel = baseDecibel - 10 * Mathf.Log10(distance + 1e-6f);  // 거리 기반으로 데시벨 값 조정 (단위: dB)
            Debug.Log(decibel);
            return Mathf.Max(0, decibel); // 음수 방지
        }
        return 0; // 소리가 재생 중이 아니면 데시벨 0
    }
    // 소리 재생
    public void PlaySound()
    {
        audioSource.Play();
    }
    // Gizmo로 소리 범위를 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.5f, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
