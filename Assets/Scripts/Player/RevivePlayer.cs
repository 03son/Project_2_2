using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class RevivePlayer : MonoBehaviourPun
{
    public float holdTime = 5f; // �Է��� �����ؾ� �ϴ� �ð�
    private float holdCounter = 0f;
    private bool isHolding = false;

    private PlayerDeathManager targetPlayer; // Ÿ�� �÷��̾��� PlayerDeathManager

    private Image timerBar; // TimerBar�� Image ������Ʈ
    private RectTransform timerBarTransform; // TimerBar�� Transform

    PlayerState playerState;
    PlayerState.playerState state;

    PhotonItem _PhotonItem; 

    private Player_Equip playerEquip; // Player_Equip ����

    RaycastHit hit;
    float MaxDistance = 5f;
    void Start()
    {
        playerState = GetComponent<PlayerState>();
        playerEquip = GetComponent<Player_Equip>(); // Player_Equip ������Ʈ ��������


        // TimerBar ã��
        timerBar = FindTimerBar();

        if (timerBar != null)
        {
            Debug.Log("TimerBar�� ���������� ã�ҽ��ϴ�.");
            timerBar.fillAmount = 0f; // �ʱ�ȭ
                                      // ���⼭ gameObject�� ��Ȱ��ȭ���� ����.
        }
        else
        {
            Debug.LogError("TimerBar�� ã�� �� �����ϴ�. UI ������ Ȯ���ϼ���.");
        }
    }


    void Update()
    {
        playerState.GetState(out state);
        if (CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Survival)
            return;

        if (targetPlayer != null && playerEquip.HasEquippedMedkit()) // Medkit ���� ���� Ȯ��
        {
            if (targetPlayer.GetComponent<PlayerState>().State == PlayerState.playerState.Die)
            {
                Debug.Log("Ÿ�� �÷��̾� �߰�: " + targetPlayer.gameObject.name);

                // Ÿ�ӹ� Ȱ��ȭ
                if (timerBar != null && !timerBar.gameObject.activeSelf)
                if (Input.GetKey(KeyManager.Interaction_Key)) // ��ȣ�ۿ� Ű Ȯ��
                {
                    Debug.Log("���ͷ��� Ű ����. Ȧ�� ���� ��...");
                    isHolding = true;
                    holdCounter += Time.deltaTime;


                    // Ÿ�ӹ� Ȱ��ȭ
                    if (timerBar != null && !timerBar.gameObject.activeSelf)
                    {
                        timerBar.gameObject.SetActive(true); // Ÿ�ӹ� Ȱ��ȭ
                        Debug.Log("Ÿ�ӹ� Ȱ��ȭ��");
                    }

                    if (timerBar != null)
                    {
                        timerBar.fillAmount = holdCounter / holdTime; // Ÿ�̸� ���� ���� ������Ʈ
                                                                      // Debug.Log($"Ÿ�̸� ���� ��: {timerBar.fillAmount * 100}% �Ϸ�");
                    }

                    if (holdCounter >= holdTime) // ������ �ð��� ������ ��Ȱ
                    {
                        Debug.Log("��Ȱ ���� ����. ��Ȱ �õ� ��...");
                        ReviveTargetPlayer(); // ��Ȱ ȣ��
                        holdCounter = 0f; // Ÿ�̸� �ʱ�ȭ
                    }
                }
                else
                {
                    Debug.Log("���ͷ��� Ű�� ������ �ʾҽ��ϴ�.");
                }
            }
        }
        else
        {
            Debug.Log("Ÿ�� �÷��̾ Medkit ����.");
        }

        if (Input.GetKeyUp(KeyManager.Interaction_Key) || !isHolding) // Ű�� ���ų� ��ȣ�ۿ� �ߴ�
        {
            if (timerBar != null)
            {
                timerBar.fillAmount = 0f; // Ÿ�̸� �ʱ�ȭ
            }
            ResetHold();
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerState otherPlayerState = other.GetComponent<PlayerState>();
            if (otherPlayerState != null)
            {
                Debug.Log($"PlayerState �߰�: {other.gameObject.name}");

                if (otherPlayerState.State == PlayerState.playerState.Die)
                {
                    targetPlayer = other.GetComponent<PlayerDeathManager>();
                    Debug.Log($"���� �÷��̾� �߰�: {targetPlayer.gameObject.name}. ��ȣ�ۿ� ����.");
                }
                else
                {
                    Debug.Log("�ش� �÷��̾�� ���� ���°� �ƴ�.");
                }
            }
            else
            {
                Debug.Log("PlayerState�� �����ϴ�.");
            }
        }
        else
        {
          //  Debug.Log($"Player �±װ� �ƴ�: {other.gameObject.name}");
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
            Debug.Log($"Ÿ�� �÷��̾�: {targetPlayer.gameObject.name}, PhotonView: {targetPlayer.photonView.ViewID}");


            // Ÿ�� �÷��̾��� ���¸� �����̹��� ���� (RPC�� ó��)
            targetPlayer.photonView.RPC("SyncStateToSurvival", RpcTarget.Others, targetPlayer.gameObject.GetComponent<PhotonView>().Owner.ActorNumber);

            // ���ÿ����� Survival �޼��� ȣ��
            if (targetPlayer.photonView.IsMine)
            {
                Debug.Log("���� �÷��̾ ��Ȱ ó�� ��...");
                targetPlayer.Survival(); // PlayerDeathManager�� Survival �޼��� ȣ��
            }

            // MedkitFunction ȣ��
            MedkitFunction medkitFunction = playerEquip.Item.GetComponent<MedkitFunction>();
            if (medkitFunction != null)
            {
                medkitFunction.Function();
            }
            else
            {
                Debug.LogWarning("MedkitFunction�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Ÿ�� �÷��̾ null�Դϴ�. ��Ȱ ó�� ����.");
        }
    }




    [PunRPC]
    void SyncStateToSurvival(int actorsNuber)
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().Owner.ActorNumber == actorsNuber)
            {
                player.GetComponent<PlayerState>().State = PlayerState.playerState.Survival; // ���� ����
                Debug.Log("��� Ŭ���̾�Ʈ���� Survival ���� ����ȭ.");
            }
        }

        // ���� �÷��̾��� ��쿡�� Survival �޼��� ȣ��
        if (photonView.IsMine)
        {
            Debug.Log("���� �÷��̾ ��Ȱ ó�� ��...");

            // PlayerDeathManager �ν��Ͻ��� �����ͼ� Survival �޼��� ȣ��
            PlayerDeathManager playerDeathManager = GetComponent<PlayerDeathManager>();
            if (playerDeathManager != null)
            {
                playerDeathManager.Survival(); // Survival �޼��� ȣ��
            }
            else
            {
                Debug.LogError("PlayerDeathManager�� ã�� �� �����ϴ�.");
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
                    Debug.Log($"���� �÷��̾� �߰�: {targetPlayer.gameObject.name}. ��ȣ�ۿ� ����.");
                }
                else
                {
                    Debug.Log("�ش� �÷��̾�� ���� ���°� �ƴ�.");
                }
            }
            else
            {
                Debug.Log("PlayerState ������Ʈ�� �����ϴ�.");
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

        if(timerBar != null)
            timerBar.fillAmount = 0;
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
