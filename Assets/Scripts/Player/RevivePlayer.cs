using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RevivePlayer : MonoBehaviour
{
    public float holdTime = 5f; // 입력을 유지해야 하는 시간
    private float holdCounter = 0f;
    private bool isHolding = false;

    private PlayerDeathManager targetPlayer; // 타겟 플레이어의 PlayerDeathManager
    private Image timerBar; // TimerBar의 Image 컴포넌트
    private RectTransform timerBarTransform; // TimerBar의 Transform

    PlayerState playerState;
    PlayerState.playerState state;

    void Start()
    {
        playerState = GetComponent<PlayerState>();

        // TimerBar 찾기
        timerBar = FindTimerBar();

        if (timerBar != null)
        {
            Debug.Log("TimerBar를 성공적으로 찾았습니다.");
            timerBar.fillAmount = 0f; // 초기화
                                      // 여기서 gameObject를 비활성화하지 않음.
        }
        else
        {
            Debug.LogError("TimerBar를 찾을 수 없습니다. UI 설정을 확인하세요.");
        }
    }


    void Update()
    {
        playerState.GetState(out state);
        if (CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Die)
            return;

        if (targetPlayer != null)
        {
            Debug.Log("타겟 플레이어 발견: " + targetPlayer.gameObject.name);

            if (Input.GetKey(KeyManager.Interaction_Key)) // 상호작용 키 확인
            {
                Debug.Log("인터랙션 키 눌림. 홀드 진행 중...");
                isHolding = true;
                holdCounter += Time.deltaTime;

                if (timerBar != null)
                {
                    timerBar.fillAmount = holdCounter / holdTime; // 타이머 진행 상태 업데이트
                    Debug.Log($"타이머 진행 중: {timerBar.fillAmount * 100}% 완료");
                }

                if (holdCounter >= holdTime) // 지정된 시간이 지나면 부활
                {
                    Debug.Log("부활 조건 충족. 부활 시도 중...");
                    ReviveTargetPlayer(); // 부활 호출
                    holdCounter = 0f; // 타이머 초기화
                }
            }
            else
            {
              Debug.Log("인터랙션 키가 눌리지 않았습니다.");
            }
        }
        else
        {
        //    Debug.Log("타겟 플레이어가 설정되지 않았습니다.");
        }

        if (Input.GetKeyUp(KeyManager.Interaction_Key) || !isHolding) // 키를 떼거나 상호작용 중단
        {
            if (timerBar != null)
            {
                timerBar.fillAmount = 0f; // 타이머 초기화
            }
       //     Debug.Log("인터랙션 키가 떼어짐 또는 상호작용 중단됨.");
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
                Debug.Log($"PlayerState 발견: {other.gameObject.name}");

                if (otherPlayerState.State == PlayerState.playerState.Die)
                {
                    targetPlayer = other.GetComponent<PlayerDeathManager>();
                    Debug.Log($"죽은 플레이어 발견: {targetPlayer.gameObject.name}. 상호작용 가능.");
                }
                else
                {
                    Debug.Log("해당 플레이어는 죽은 상태가 아님.");
                }
            }
            else
            {
                Debug.Log("PlayerState가 없습니다.");
            }
        }
        else
        {
            Debug.Log($"Player 태그가 아님: {other.gameObject.name}");
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
            Debug.Log("플레이어가 부활했습니다.");

            if (timerBar != null)
            {
                timerBar.gameObject.SetActive(false); // TimerBar 비활성화
                timerBar.fillAmount = 0; // 초기화
            }
        }
    }

    void CheckForDeadPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f)) // 5f는 레이 길이
        {
            Debug.Log($"레이 충돌: {hit.collider.gameObject.name}"); // 충돌한 오브젝트의 이름 출력

            PlayerState otherPlayerState = hit.collider.GetComponent<PlayerState>();
            if (otherPlayerState != null)
            {
                Debug.Log("PlayerState 컴포넌트 발견."); // PlayerState가 있는 경우 출력

                if (otherPlayerState.State == PlayerState.playerState.Die)
                {
                    targetPlayer = hit.collider.GetComponent<PlayerDeathManager>();
                    Debug.Log($"죽은 플레이어 발견: {targetPlayer.gameObject.name}. 상호작용 가능.");
                }
                else
                {
                    Debug.Log("해당 플레이어는 죽은 상태가 아님.");
                }
            }
            else
            {
                Debug.Log("PlayerState 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.Log("레이캐스트가 아무것도 감지하지 못했습니다.");
        }
    }




    private void ResetHold()
    {
        isHolding = false;
        holdCounter = 0f;
    }

    private Image FindTimerBar()
    {
        // 동적으로 TimerBar 검색
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
