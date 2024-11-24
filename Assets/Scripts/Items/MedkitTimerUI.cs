using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MedkitTimerUI : MonoBehaviour
{
    public Image timerImage; // ���� ���α׷��� �� �̹���
    //public TextMeshProUGUI timerText; // TextMeshProUGUI Ÿ������ ����
    private float totalTime = 5f; // �޵�Ŷ ��� �ð�

    private void Start()
    {
        // ���� ���� �� UI ��Ȱ��ȭ
        //timerImage.gameObject.SetActive(false);
        // timerText.gameObject.SetActive(false);

        timerImage.fillAmount = 0;
    }

    public void StartMedkitTimer()
    {
        Debug.Log("StartMedkitTimer �޼��� �����");
        // Ÿ�̸� UI Ȱ��ȭ
        timerImage.gameObject.SetActive(true);
       // timerText.gameObject.SetActive(true);
        StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        float timeLeft = totalTime;
        timerImage.fillAmount = 0; // �ʱ� ����

        while (timeLeft < 1)
        {
            timeLeft -= Time.deltaTime;
            timerImage.fillAmount = timeLeft / totalTime;
           // timerText.text = timeLeft.ToString("F1"); // �Ҽ��� �� �ڸ��� ǥ��
            yield return null;
        }

        timerImage.fillAmount = 1;
       // timerText.text = "0";

        // Ÿ�̸� ���� �� UI ��Ȱ��ȭ
        //timerImage.gameObject.SetActive(false);
        //timerText.gameObject.SetActive(false);

        Debug.Log("�޵�Ŷ ��� �Ϸ�!");
    }

    public void ResetTimer()
    {
        timerImage.fillAmount = 0; // Ÿ�̸� �̹����� �ʱ� ���·� ����
        //timerText.text = ""; // �ؽ�Ʈ ����
        //timerImage.gameObject.SetActive(false); // �̹��� ����
        //timerText.gameObject.SetActive(false); // �ؽ�Ʈ ����
    }
}
