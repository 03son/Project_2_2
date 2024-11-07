using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;

public interface IInteractable
{
    string GetInteractPrompt(); // 프롬프트 메시지를 받아오는 메서드
    void OnInteract(); // 상호작용 후 실행될 메서드
}

public class InteractionManager : MonoBehaviour
{
    public float checkRate = 0.05f; // 상호작용 검사 빈도
    float lastCheckTime;
    public float maxCheckDistance = 3f; // 상호작용 가능한 최대 거리
    public LayerMask layerMask; // 상호작용할 레이어

    GameObject curInteractGameobject; // 현재 상호작용 대상 오브젝트
    IInteractable curInteractable; // 현재 상호작용 가능한 인터페이스

    public TextMeshProUGUI promptText; // 상호작용 프롬프트 텍스트
    Camera camera; // 플레이어의 메인 카메라
    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();

        // 자신이 조종하는 캐릭터가 아닐 경우 반환
        if (!pv.IsMine)
            return;

        // 프롬프트 텍스트 UI 가져오기
        promptText = GameObject.Find("prompt_Text (TMP)").GetComponent<TextMeshProUGUI>();
        promptText.gameObject.SetActive(false); // 처음에는 보이지 않게 설정

        camera = Camera.main; // 메인 카메라 설정
    }

    void Update()
    {
        // 자신이 조종하는 캐릭터가 아닐 경우 반환
        if (!pv.IsMine)
            return;

        // 지정된 검사 빈도에 따라 상호작용 대상 검사
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            CheckForInteractable(); // 상호작용 가능한 오브젝트 체크
        }

        // 상호작용 입력 처리
        OnInteractInput();
    }

    void CheckForInteractable()
    {
        // 화면 중앙에서 Raycast 발사
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // 상호작용 가능한 오브젝트가 있는지 검사
        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            GameObject hitObject = hit.collider.gameObject;

            // 새로운 오브젝트와 상호작용을 시도할 경우
            if (hitObject != curInteractGameobject)
            {
                curInteractGameobject = hitObject;
                curInteractable = hitObject.GetComponent<IInteractable>();

                if (curInteractable != null)
                {
                    SetPromptText(); // 상호작용 프롬프트 텍스트 설정
                }
            }
        }
        else
        {
            ClearInteractable(); // 상호작용 가능한 대상이 없을 경우 초기화
        }
    }

    void ClearInteractable()
    {
        // 상호작용 대상과 텍스트 초기화
        curInteractGameobject = null;
        curInteractable = null;
        promptText.gameObject.SetActive(false);
    }

    public void OnInteractInput()
    {
        // F키 입력 시 현재 바라보고 있는 상호작용 가능한 오브젝트와 상호작용
        if (Input.GetKeyDown(KeyCode.F) && curInteractable != null)
        {
            Debug.Log("상호작용 입력 받음 - 상호작용 가능한 오브젝트 발견");
            curInteractable.OnInteract(); // 상호작용 실행
            ClearInteractable(); // 상호작용 후 초기화
        }
    }


    void SetPromptText()
    {
        // 프롬프트 텍스트 UI 표시 및 텍스트 설정
        promptText.gameObject.SetActive(true);
        promptText.text = string.Format("<b>[F]</b> {0}", curInteractable.GetInteractPrompt());
    }
}
