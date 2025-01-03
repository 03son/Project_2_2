using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // Photon 네임스페이스
using UnityEngine.UI;
using TMPro;

public interface IInteractable
{
    string GetInteractPrompt(); // 상호작용 프롬프트를 반환
    void OnInteract(); // 상호작용 실행
}

public class InteractionManager : MonoBehaviour
{
    public float checkRate = 0.05f; // 체크 주기
    private float lastCheckTime;
    public float maxCheckDistance; // 상호작용 최대 거리
    public LayerMask layerMask; // 상호작용 가능한 레이어
    public LayerMask layerMask2; // 상호작용 가능한 레이어
    LayerMask layerMask15; // 상호작용 가능한 레이어

    private GameObject curInteractGameobject;
    private IInteractable curInteractable;
    public Crosshair_Image Crosshair;
    TextMeshProUGUI SystemText;

    private Camera camera;
    private PhotonView pv;
    private Animator animator; // 애니메이터

    private PlayerState.playerState state;
    private PlayerState playerState;

    void Awake()
    {
        GameObject.Find("SystemText").gameObject.GetComponent<TextMeshProUGUI>().text = "";
    }
    void Start()
    {
        // Photon 연결 확인
        if (PhotonNetwork.IsConnected)
        {
            pv = GetComponent<PhotonView>();
            if (!pv.IsMine) return; // 자신의 객체가 아니면 실행하지 않음
        }
        SystemText = GameObject.Find("SystemText").gameObject.GetComponent<TextMeshProUGUI>();
        playerState = GetComponent<PlayerState>();
        Crosshair = GameObject.Find("Crosshair_Image").GetComponent<Crosshair_Image>();
        camera = Camera.main;

        layerMask15 = LayerMask.NameToLayer("Submarine");

        // Animator 컴포넌트 확인
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator 컴포넌트를 찾을 수 없습니다. 애니메이션이 비활성화됩니다.");
        }
    }

    void Update()
    {
        // 자신의 객체만 동작
        if (PhotonNetwork.IsConnected && !pv.IsMine) return;

        // ESC 메뉴 또는 죽음 상태일 때 중단
        playerState.GetState(out state);
        if (!CameraInfo.MainCam.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.Die)
            return;

        // 상호작용 대상 확인 (checkRate 주기로 실행)
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            CheckForInteractable();
        }

        // 상호작용 입력 처리
        OnInteractInput();
    }

    // 상호작용 가능한 객체 확인
    private void CheckForInteractable()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // 첫 번째 Raycast: 충돌 검사
        if (Physics.Raycast(ray, out hit, maxCheckDistance))
        {
            // 충돌한 오브젝트의 레이어 확인
            if (((1 << hit.collider.gameObject.layer) & layerMask) == 0 && ((1 << hit.collider.gameObject.layer) & layerMask2) == 0)
            {
                // 레이어가 layerMask 또는 layerMask2에 포함되지 않으면 초기화
                ResetInteraction();
                return;
            }
        }
        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            
            // 상호작용 가능한 객체가 바뀌었을 때만 갱신
            if (hit.collider.gameObject != curInteractGameobject)
            {
                if (hit.collider.gameObject.tag == "Submarine")
                    SystemText.text = "배터리 2개와 프로펠러가 필요해 보인다.";
                else
                    SystemText.text = "";
                curInteractGameobject = hit.collider.gameObject;

                curInteractable = hit.collider.GetComponent<IInteractable>();
                Crosshair.Interaction(); // 크로스헤어 상호작용 표시
            }
        }
        else if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask2))
        {
            // 상호작용 가능한 객체가 바뀌었을 때만 갱신
            if (hit.collider.gameObject != curInteractGameobject)
            {
                if (hit.collider.gameObject.tag == "Door")
                    SystemText.text = "열쇠가 필요해 보인다";
                curInteractGameobject = hit.collider.gameObject;

                curInteractable = hit.collider.GetComponent<IInteractable>();
                Crosshair.Interaction(); // 크로스헤어 상호작용 표시
            }
        }
        else
        {
            // 상호작용 가능한 객체가 없을 때 초기화
            ResetInteraction();
            //curInteractGameobject = null;
            //curInteractable = null;
            //Crosshair.Not_Interaction();
            //SystemText.text = "";
        }
    }

    // 상호작용 입력 처리
    public void OnInteractInput()
    {
        if (Input.GetKeyDown(KeyManager.Interaction_Key) && curInteractable != null)
        {
            // 상호작용 애니메이션
            animator?.SetTrigger("PickupItem");

            // 상호작용 실행
            curInteractable.OnInteract();

            // 상호작용 초기화
            curInteractGameobject = null;
            curInteractable = null;
            Crosshair.Not_Interaction();
        }
    }
    // 상호작용 초기화 메서드
    private void ResetInteraction()
    {
        curInteractGameobject = null;
        curInteractable = null;
        Crosshair.Not_Interaction();
        SystemText.text = "";
    }

}
