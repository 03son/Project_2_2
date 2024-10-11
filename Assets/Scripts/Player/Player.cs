using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance; // 플레이어 인스턴스 싱글턴
    public AudioSource audioSource; // 오디오 소스

    void Awake()
    {
        // 싱글턴 설정: 이미 존재하는 인스턴스가 있으면 파괴하고, 없으면 할당
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // 오디오 소스 가져오기
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("Player 오브젝트에 AudioSource 컴포넌트가 없음. 오디오 소스를 추가해야 함.");
        }
    }

    // 다른 Player 관련 메서드나 변수를 여기 추가하면 됨.
}
