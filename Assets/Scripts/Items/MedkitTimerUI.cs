using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MedkitTimerUI : MonoBehaviour
{
    public Image timerImage;
    public TextMeshProUGUI timerText;
    private float totalTime = 5f;

    private void Start()
    {
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    public void StartMedkitTimer()
    {
        Debug.Log("StartMedkitTimer 메서드 실행됨");
        StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        float timeLeft = totalTime;
        timerImage.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerImage.fillAmount = timeLeft / totalTime;
            timerText.text = timeLeft.ToString("F1");
            yield return null;
        }

        timerImage.fillAmount = 0;
        timerText.text = "0";
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    public void ResetTimer()
    {
        timerImage.fillAmount = 0;
        timerText.text = "";
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }
}
