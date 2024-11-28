using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Photon.Pun;
using System;
using UnityEngine.UI;

public interface IInteractable
{
    string GetInteractPrompt(); // 프롬프트 받아오는 메서드
    void OnInteract(); // 상호작용 후 실행될 메서드
}

public class InteractionManager : MonoBehaviour
{
    public float checkRate = 0.05f;
    float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    GameObject curInteractGameobject;
    IInteractable curInteractable;
    public Crosshair_Image Crosshair;

    new Camera camera;

    PhotonView pv;
    private Animator animator; // 애니메이터 추가

    void Start()
    {
        if (PhotonNetwork.IsConnected) // 멀티
        {
            pv = GetComponent<PhotonView>();

            if (!pv.IsMine)
                return;
        }

        Crosshair = GameObject.Find("Crosshair_Image").GetComponent<Crosshair_Image>();
        camera = Camera.main;

        // Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator 컴포넌트를 찾을 수 없습니다. 플레이어 오브젝트에 Animator가 있어야 합니다.");
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!pv.IsMine)
                return;
        }

        // 마지막으로 체크한 시간이 checkRate를 넘겼다면
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // 화면의 정중앙에 상호작용 가능한 물체가 있는지 확인하기
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            // ray에 뭔가 충돌했다면 hit에 충돌한 오브젝트에 대한 정보가 넘어감
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // 부딪힌 오브젝트가 우리가 저장해 놓은 상호작용이 가능한 오브젝트인지 확인
                if (hit.collider.gameObject != curInteractGameobject)
                {
                    // 충돌한 물체 가져오기
                    curInteractGameobject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    Crosshair.Interaction();
                }
            }
            else
            {
                // 화면 중앙에 상호작용 가능한 물체가 없는 경우
                curInteractGameobject = null;
                curInteractable = null;
                Crosshair.Not_Interaction();
            }
        }

        OnInteractInput();
    }

    public void OnInteractInput()
    {
        // F키를 누른 시점에서 현재 바라보는 curInteractable 오브젝트가 있다면
        if (Input.GetKeyDown(KeyManager.Interaction_Key) && curInteractable != null)
        {
            // 아이템을 획득하면 애니메이션 실행
            if (animator != null)
            {
                animator.SetTrigger("PickupItem"); // 획득 애니메이션 트리거 설정
            }

            // 아이템과 상호작용을 진행하고 초기화
            curInteractable.OnInteract();

            // 빈 곳 번호 찾기
            for (int i = 0; i < GetComponent<Inventory>().slots.Length; i++)
            {
                if (GetComponent<Inventory>().slots[i].item != null)
                {
                    GetComponent<Player_Equip>().numderKeySelectSlot(i + 1);
                }
            }

            curInteractGameobject = null;
            curInteractable = null;
            Crosshair.Not_Interaction();
        }
    }
}
