using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Photon.Pun;

public class MedkitTimerUI : MonoBehaviourPun
{
    public Image timerImage; // ���� ���α׷��� �� �̹���
    public TextMeshProUGUI timerText; // TextMeshProUGUI Ÿ������ ����
    private float totalTime = 5f; // �޵�Ŷ ��� �ð�

    private void Start()
    {
        // ���� ���� �� UI ��Ȱ��ȭ
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    public void TriggerMedkitTimer()
    {
        // Ÿ�̸� UI Ȱ��ȭ
        timerImage.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);

        // Ÿ�̸� �ڷ�ƾ ����
        StartCoroutine(RunTimer());
    }


    [PunRPC]
    public void StartMedkitTimer()
    {
        Debug.Log("Medkit Ÿ�̸� ����");
        timerImage.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        float timeLeft = totalTime;
        timerImage.fillAmount = 1; // �ʱ� ����

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerImage.fillAmount = timeLeft / totalTime;
            timerText.text = timeLeft.ToString("F1"); // �Ҽ��� �� �ڸ��� ǥ��
            yield return null;
        }

        timerImage.fillAmount = 0;
        timerText.text = "0";

        // Ÿ�̸� ���� �� UI ��Ȱ��ȭ
        timerImage.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        // ��Ȱ ������ �Ϸ�� ��
        Debug.Log("�޵�Ŷ ��� �Ϸ�!");
    }

    public void ResetTimer()
    {
        timerImage.fillAmount = 0; // Ÿ�̸� �̹����� �ʱ� ���·� ����
        timerText.text = ""; // �ؽ�Ʈ ����
        timerImage.gameObject.SetActive(false); // �̹��� ����
        timerText.gameObject.SetActive(false); // �ؽ�Ʈ ����
    }
}
