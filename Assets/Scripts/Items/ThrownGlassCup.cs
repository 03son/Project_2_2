using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownGlassCup : MonoBehaviour
{
    public AudioClip breakSound;  // 충돌 시 재생할 소리
    private bool isBroken = false;  // 중복 파괴 방지
    private bool audioAssigned = false;  // 오디오가 할당되었는지 확인

    void Start()
    {
        // 처음에 오디오 클립이 할당되었는지 확인
        AssignAudioClip();
    }

    // 부모가 변경될 때 호출되는 이벤트
    void OnTransformParentChanged()
    {
        // 부모가 변경될 때마다 오디오 클립이 할당되었는지 다시 확인
        AssignAudioClip();
    }

    // 충돌 이벤트 처리
    void OnCollisionEnter(Collision collision)
    {
        if (!isBroken)
        {
            isBroken = true;

            // breakSound가 할당되었는지 확인 후 재생
            if (breakSound != null)
            {
                AudioSource.PlayClipAtPoint(breakSound, transform.position);
            }
            else
            {
                Debug.LogWarning("breakSound가 할당되지 않았습니다!");
            }

            // 유리컵 파괴
            Destroy(gameObject, 1f);  // 소리 재생 후 1초 뒤 오브젝트 파괴
        }
    }

    // 오디오 클립 할당을 확인하는 함수
    void AssignAudioClip()
    {
        if (!audioAssigned && breakSound != null)
        {
            audioAssigned = true;  // 오디오 클립이 할당되었음을 기록
        }
        else if (breakSound == null)
        {
            Debug.LogWarning("오디오 클립이 할당되지 않았습니다.");
        }
    }
}
