using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Photon.Pun;
using System;

public interface IInteractable
{
    string GetInteractPrompt();//������Ʈ �޾ƿ��� �޼���
    void OnInteract();//��ȣ�ۿ� �� ���� �� �޼���
}
public class InteractionManager : MonoBehaviour
{   //������ ��ȣ�ۿ� �Ŵ���

    public float checkRate = 0.05f;
    float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    GameObject curInteractGameobject;
    IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    new Camera camera;

    PhotonView pv;


    void Start()
    {
        if (PhotonNetwork.IsConnected) //��Ƽ
        {
            pv = GetComponent<PhotonView>();

            if (!pv.IsMine)
                return;

            promptText = GameObject.Find("prompt_Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
            promptText.gameObject.SetActive(false);

            camera = Camera.main;
        }
        else // �̱�
        {
            promptText = GameObject.Find("prompt_Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
            promptText.gameObject.SetActive(false);

            camera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!pv.IsMine)
                return;
        }

        //���������� üũ�� �ð��� checkRate�� �Ѱ�ٸ�
        if (Time.time - lastCheckTime > checkRate)
        { 
            lastCheckTime = Time.time;
            // ȭ���� ���߾ӿ� ��ȣ�ۿ� ������ ��ü�� �ִ��� Ȯ���ϱ�

            //ȭ�� �߾ӿ��� Ray �߻�
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
            RaycastHit hit;

            //ray�� ���� �浹�ߴٸ� hit�� �浹�� ������Ʈ�� ���� ������ �Ѿ
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                //�ε��� ������Ʈ�� �츮�� ������ ���� ��ȣ�ۿ��� ������ ������Ʈ���� Ȯ��
                if (hit.collider.gameObject != curInteractGameobject)
                {
                    //�浹�� ��ü ��������
                    curInteractGameobject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                //ȭ�� �߾ӿ� ��ȣ�ۿ� ������ ��ü�� ���� ���
                curInteractGameobject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
        OnInteractInput();

        
    }

    public void OnInteractInput()
    {
        //FŰ�� ���� �������� ���� �ٶ󺸴� curInteractable ������Ʈ�� �ִٸ�
        if (Input.GetKeyDown(KeyManager.Interaction_Key) && curInteractable != null)
        {
            //�������� ŉ���ϸ� �����۰� ��ȣ�ۿ��� �����ϰ� �ʱ�ȭ
            curInteractable.OnInteract();

            //��� ��ȣ ã��
            for (int i = 0; i < GetComponent<Inventory>().slots.Length; i++)
            {
                if (GetComponent<Inventory>().slots[i].item != null)
                {
                    GetComponent<Player_Equip>().numderKeySelectSlot(i + 1);
                }
            }

            curInteractGameobject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }

    void SetPromptText()
    { 
        promptText.gameObject.SetActive(true);

        //<b></b> : <b>�� ��Ʈü
        promptText.text = string.Format($"<b>[{KeyManager.Interaction_Key}]</b> {curInteractable.GetInteractPrompt()}");
    }

    // ���� �޼���� ������Ʈ ������Ʈ
    public static void UpdatePrompt(IInteractable interactable)
    {
        InteractionManager instance = FindObjectOfType<InteractionManager>();
        if (instance != null && interactable != null)
        {
            instance.promptText.text = interactable.GetInteractPrompt();
        }
    }
}
