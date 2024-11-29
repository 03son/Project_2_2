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
    string GetInteractPrompt(); // ������Ʈ �޾ƿ��� �޼���
    void OnInteract(); // ��ȣ�ۿ� �� ����� �޼���
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
    private Animator animator; // �ִϸ����� �߰�

    PlayerState.playerState state;
    PlayerState playerState;


    void Start()
    {
        if (PhotonNetwork.IsConnected) // ��Ƽ
        {
            pv = GetComponent<PhotonView>();

            if (!pv.IsMine)
                return;
        }

        playerState = GetComponent<PlayerState>();
        Crosshair = GameObject.Find("Crosshair_Image").GetComponent<Crosshair_Image>();
        camera = Camera.main;

        // Animator ������Ʈ ��������
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator ������Ʈ�� ã�� �� �����ϴ�. �÷��̾� ������Ʈ�� Animator�� �־�� �մϴ�.");
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!pv.IsMine)
                return;
        }

        //escâ�� �����ְ� �÷��̾ ������ �� ����
        playerState.GetState(out state);
        if (!Camera.main.GetComponent<CameraRot>().popup_escMenu && state == PlayerState.playerState.죽음)
            return;

        //���������� üũ�� �ð��� checkRate�� �Ѱ�ٸ�
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // ȭ���� ���߾ӿ� ��ȣ�ۿ� ������ ��ü�� �ִ��� Ȯ���ϱ�
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            // ray�� ���� �浹�ߴٸ� hit�� �浹�� ������Ʈ�� ���� ������ �Ѿ
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // �ε��� ������Ʈ�� �츮�� ������ ���� ��ȣ�ۿ��� ������ ������Ʈ���� Ȯ��
                if (hit.collider.gameObject != curInteractGameobject)
                {
                    // �浹�� ��ü ��������
                    curInteractGameobject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    Crosshair.Interaction();
                }
            }
            else
            {
                // ȭ�� �߾ӿ� ��ȣ�ۿ� ������ ��ü�� ���� ���
                curInteractGameobject = null;
                curInteractable = null;
                Crosshair.Not_Interaction();
            }
        }

        OnInteractInput();
    }

    public void OnInteractInput()
    {
        // FŰ�� ���� �������� ���� �ٶ󺸴� curInteractable ������Ʈ�� �ִٸ�
        if (Input.GetKeyDown(KeyManager.Interaction_Key) && curInteractable != null)
        {
            // �������� ȹ���ϸ� �ִϸ��̼� ����
            if (animator != null)
            {
                animator.SetTrigger("PickupItem"); // ȹ�� �ִϸ��̼� Ʈ���� ����
            }

            // �����۰� ��ȣ�ۿ��� �����ϰ� �ʱ�ȭ
            curInteractable.OnInteract();

            // �� �� ��ȣ ã��
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
