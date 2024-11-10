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
    private bool isTimerRunning = false; // Ÿ�̸Ӱ� ���� ������ ���θ� �����ϴ� ����

    private void Start()
    {
        // ���� ���� �� UI ��Ȱ��ȭ
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    public void TriggerMedkitTimer()
    {
        if (!isTimerRunning) // Ÿ�̸Ӱ� ���� ���� �ƴ� ���� ����
        {
            Debug.Log("TriggerMedkitTimer �޼��� ���� ��");
            isTimerRunning = true; // Ÿ�̸� ���� ���� ����
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
        Debug.Log("RunTimer �ڷ�ƾ ����");
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

        Debug.Log("RunTimer �ڷ�ƾ ����");

        // Ÿ�̸Ӱ� ������ UI ��Ȱ��ȭ
        timerImage.fillAmount = 0;
        timerText.text = "0";
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        isTimerRunning = false; // Ÿ�̸� ���� ���� �ʱ�ȭ
    }

    public void ResetTimer()
    {
        currentTime = 0;
        timerImage.fillAmount = 0;
        timerText.text = "";
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        isTimerRunning = false; // Ÿ�̸� ���� ���� �ʱ�ȭ
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
