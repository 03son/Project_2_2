using Photon.Pun;
using UnityEngine;
using UnityEngine.Video; // VideoPlayer�� ����ϱ� ���� �߰�
using System.Collections; // �ڷ�ƾ�� ����ϱ� ���� �߰�

public class SubmarineStart : MonoBehaviourPun, IInteractable
{
    public SubmarineController submarineController; // Inspector���� ������ �� �ֵ��� public���� ����
    public VideoPlayer videoPlayer; // �������� ����� VideoPlayer
    public VideoClip escapeVideoClip; // Ż�� ������ ���� Ŭ��

    private bool playerInRange = false;

    private void Start()
    {
        if (submarineController == null)
        {
            Debug.LogError("SubmarineController�� �Ҵ���� �ʾҽ��ϴ�. Inspector���� ������ �ּ���.");
        }

        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>(); // VideoPlayer�� ������ �߰�
        }

        videoPlayer.playOnAwake = false; // ���� ���� �� �ڵ� ��� ����
        videoPlayer.loopPointReached += OnVideoEnd; // ���� ��� ���� �� �̺�Ʈ ����
    }

    // IInteractable �������̽� ����
    public string GetInteractPrompt()
    {
        return "�� ���� ����� �õ�";
    }

    public void OnInteract()
    {
        if (submarineController != null && submarineController.CanStart())
        {
            submarineController.StartSubmarine();
            Debug.Log("Ż�� �������� ���۵˴ϴ�! (�ִϸ��̼� ��ü �����)");

            // 3�� �� Ż�� ������ ���� ��� ����
            StartCoroutine(PlayEscapeVideoWithDelay());
        }
        else
        {
            Debug.Log("��� ��ǰ�� �������� �ʾҽ��ϴ�. ������� ������ �� �����ϴ�.");
        }
    }

    private IEnumerator PlayEscapeVideoWithDelay()
    {
        yield return new WaitForSeconds(3f); // 3�� ���

        if (escapeVideoClip != null)
        {
            videoPlayer.clip = escapeVideoClip;
            videoPlayer.gameObject.SetActive(true); // VideoPlayer Ȱ��ȭ
            videoPlayer.Play(); // ���� ���
        }
        else
        {
            Debug.LogError("Ż�� ���� Ŭ���� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("����� �õ� ������ ����. F Ű�� ���� �õ��� �� �� �ֽ��ϴ�.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("�õ� �������� ���.");
        }
    }

    // ���� ��� ���� �� ȣ��Ǵ� �޼���
    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Ż�� ������ ����Ǿ����ϴ�. ������ �����մϴ�.");
        Application.Quit(); // ���� ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
