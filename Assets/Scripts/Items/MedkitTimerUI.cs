using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Collections;

public class MedkitTimerUI : MonoBehaviourPun, IPunObservable
{
    public Image timerImage;
    public TextMeshProUGUI timerText;
    private float totalTime = 5f;
    private float currentTime;
    private bool isTimerRunning = false; // 타이머가 실행 중인지 여부를 추적하는 변수

    private void Start()
    {
        // 게임 시작 시 UI 비활성화
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    public void TriggerMedkitTimer()
    {
        if (!isTimerRunning) // 타이머가 실행 중이 아닐 때만 실행
        {
            Debug.Log("TriggerMedkitTimer 메서드 실행 중");
            isTimerRunning = true; // 타이머 실행 상태 설정
            currentTime = totalTime;
            timerImage.gameObject.SetActive(true);
            timerText.gameObject.SetActive(true);
            StartCoroutine(RunTimer());
        }
    }

    [PunRPC]
    public void RPC_TriggerMedkitTimer()
    {
        TriggerMedkitTimer();
    }

    private IEnumerator RunTimer()
    {
        Debug.Log("RunTimer 코루틴 시작");
        timerImage.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);

        float timeLeft = totalTime;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerImage.fillAmount = timeLeft / totalTime;
            timerText.text = timeLeft.ToString("F1");
            yield return null;
        }

        Debug.Log("RunTimer 코루틴 종료");

        // 타이머가 끝나면 UI 비활성화
        timerImage.fillAmount = 0;
        timerText.text = "0";
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        isTimerRunning = false; // 타이머 실행 상태 초기화
    }

    public void ResetTimer()
    {
        currentTime = 0;
        timerImage.fillAmount = 0;
        timerText.text = "";
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        isTimerRunning = false; // 타이머 실행 상태 초기화
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentTime);
        }
        else
        {
            currentTime = (float)stream.ReceiveNext();
        }
    }
}
