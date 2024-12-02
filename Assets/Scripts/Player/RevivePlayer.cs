using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RevivePlayer : MonoBehaviour
{
    public float holdTime = 5f; // �Է��� �����ؾ� �ϴ� �ð�
    private float holdCounter = 0f;
    private bool isHolding = false;

    private PlayerDeathManager targetPlayer; // Ÿ�� �÷��̾��� PlayerDeathManager
    private Image timerBar; // TimerBar�� Image ������Ʈ
    private RectTransform timerBarTransform; // TimerBar�� Transform

    PlayerState playerState;
    PlayerState.playerState state;

    void Start()
    {
        playerState = GetComponent<PlayerState>();

        // TimerBar ã��
        timerBar = FindTimerBar();
        if (timerBar != null)
        {
            timerBar.fillAmount = 0f; // �ʱ�ȭ
            timerBar.gameObject.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
        }
        else
        {
            Debug.LogError("TimerBar�� ã�� �� �����ϴ�. UI ������ Ȯ���ϼ���.");
        }
    }

    void Update()
    {
        playerState.GetState(out state);
        if (CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Die)
            return;

        if (targetPlayer != null && Input.GetKey(KeyManager.Interaction_Key)) // ��ȣ�ۿ�
        {
            isHolding = true;
            holdCounter += Time.deltaTime;

            if (timerBar != null)
            {
                timerBar.gameObject.SetActive(true); // TimerBar Ȱ��ȭ
                timerBar.fillAmount = holdCounter / holdTime; // ���� ���� ������Ʈ
            }

            if (holdCounter >= holdTime) // ������ �ð��� ������ ��Ȱ
            {
                ReviveTargetPlayer(); // ��Ȱ ȣ��
                ResetHold();
            }
        }
        else if (Input.GetKeyUp(KeyManager.Interaction_Key) || !isHolding) // Ű�� ���ų� ��ȣ�ۿ� �ߴ�
        {
            if (timerBar != null)
            {
                timerBar.gameObject.SetActive(false); // TimerBar ��Ȱ��ȭ
                timerBar.fillAmount = 0f; // �ʱ�ȭ
            }
            ResetHold();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerState otherPlayerState = other.GetComponent<PlayerState>();
            if (otherPlayerState != null && otherPlayerState.State == PlayerState.playerState.Die)
            {
                targetPlayer = other.GetComponent<PlayerDeathManager>();
                Debug.Log("���� �÷��̾� �߰�. ��ȣ�ۿ����� ��Ȱ ����.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetPlayer = null;
            ResetHold();
        }
    }

    private void ReviveTargetPlayer()
    {
        if (targetPlayer != null)
        {
            targetPlayer.GetComponent<PlayerState>().State = PlayerState.playerState.Survival;
            Debug.Log("�÷��̾ ��Ȱ�߽��ϴ�.");

            if (timerBar != null)
            {
                timerBar.gameObject.SetActive(false); // TimerBar ��Ȱ��ȭ
                timerBar.fillAmount = 0; // �ʱ�ȭ
            }
        }
    }

    void CheckForDeadPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f)) // 5f�� ���� ����
        {
            Debug.Log($"���� �浹: {hit.collider.gameObject.name}"); // �浹�� ������Ʈ�� �̸� ���

            PlayerState otherPlayerState = hit.collider.GetComponent<PlayerState>();
            if (otherPlayerState != null)
            {
                Debug.Log("PlayerState ������Ʈ �߰�."); // PlayerState�� �ִ� ��� ���

                if (otherPlayerState.State == PlayerState.playerState.Die)
                {
                    targetPlayer = hit.collider.GetComponent<PlayerDeathManager>();
                    Debug.Log("���� �÷��̾� �߰�. ��ȣ�ۿ����� ��Ȱ ����.");
                }
            }
            else
            {
                Debug.Log("PlayerState ������Ʈ�� �����ϴ�.");
                targetPlayer = null;
            }
        }
        else
        {
            Debug.Log("����ĳ��Ʈ�� �ƹ��͵� �������� ���߽��ϴ�.");
        }
    }



    private void ResetHold()
    {
        isHolding = false;
        holdCounter = 0f;
    }

    private Image FindTimerBar()
    {
        // �������� TimerBar �˻�
        GameObject hudCanvas = GameObject.Find("HUD_Canvas");
        if (hudCanvas != null)
        {
            Transform timerBarTransform = hudCanvas.transform.Find("TimerBar");
            if (timerBarTransform != null)
            {
                return timerBarTransform.GetComponent<Image>();
            }
        }
        return null;
    }
}
