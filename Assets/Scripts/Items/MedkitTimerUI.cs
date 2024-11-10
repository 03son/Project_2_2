using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MedkitTimerUI : MonoBehaviour
{
    public Image timerImage; // 원형 프로그레스 바 이미지
    public TextMeshProUGUI timerText; // TextMeshProUGUI 타입으로 설정
    private float totalTime = 5f; // 메딧킷 사용 시간

    private void Start()
    {
        // 게임 시작 시 UI 비활성화
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    public void StartMedkitTimer()
    {
        Debug.Log("ds");
        // 타이머 UI 활성화
        timerImage.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        float timeLeft = totalTime;
        timerImage.fillAmount = 1; // 초기 상태

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerImage.fillAmount = timeLeft / totalTime;
            timerText.text = timeLeft.ToString("F1"); // 소수점 한 자리로 표시
            yield return null;
        }

        timerImage.fillAmount = 0;
        timerText.text = "0";

        // 타이머 종료 후 UI 비활성화
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        // 부활 로직 실행
        Debug.Log("메딧킷 사용 완료!");
    }

    public void ResetTimer()
    {
        timerImage.fillAmount = 0; // 타이머 이미지를 초기 상태로 설정
        timerText.text = ""; // 텍스트 숨김
        timerImage.gameObject.SetActive(false); // 이미지 숨김
        timerText.gameObject.SetActive(false); // 텍스트 숨김
    }

}
