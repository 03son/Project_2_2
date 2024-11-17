using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour
{
    public AudioSource audioSource;
    public float baseDecibel = 50f;  // 기본 데시벨 값
    public float range = 20f;        // 소리의 최대 범위

    // 특정 위치에서의 데시벨을 계산하는 함수
    public float GetDecibelAtDistance(Vector3 position)
    {
        if (audioSource.isPlaying)
        {
            float distance = Vector3.Distance(transform.position, position);
            float decibel = baseDecibel - 20f * Mathf.Log10(distance);
            return Mathf.Max(0, decibel); // 데시벨은 음수가 되지 않도록 클램핑
        }
        return 0; // 오디오 소스가 재생 중이 아니면 데시벨은 0으로 반환
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 오디오 클립이 설정되지 않았다면 오류 메시지 출력
        if (audioSource.clip == null)
        {
            Debug.LogError("AudioSource에 AudioClip이 할당되지 않았습니다. 오디오 클립을 설정해 주세요.");
        }
    }

    public void PlaySound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    void OnDrawGizmos()
    {
        // AudioSource 가져오기
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // 오디오 소스 위치에 대한 Gizmo 색상 설정
        Gizmos.color = new Color(0, 1, 0, 0.3f); // 반투명한 초록색

        // Min Distance 반경 그리기
        Gizmos.DrawWireSphere(transform.position, audioSource.minDistance);

        // Max Distance 반경 그리기
        Gizmos.color = new Color(1, 0, 0, 0.3f); // 반투명한 빨간색
        Gizmos.DrawWireSphere(transform.position, audioSource.maxDistance);
    }
}
