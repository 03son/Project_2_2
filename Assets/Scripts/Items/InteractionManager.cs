using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;

public interface IInteractable
{
    string GetInteractPrompt(); // ������Ʈ �޽����� �޾ƿ��� �޼���
    void OnInteract(); // ��ȣ�ۿ� �� ����� �޼���
}

public class InteractionManager : MonoBehaviour
{
    public float checkRate = 0.05f; // ��ȣ�ۿ� �˻� ��
    float lastCheckTime;
    public float maxCheckDistance = 3f; // ��ȣ�ۿ� ������ �ִ� �Ÿ�
    public LayerMask layerMask; // ��ȣ�ۿ��� ���̾�

    GameObject curInteractGameobject; // ���� ��ȣ�ۿ� ��� ������Ʈ
    IInteractable curInteractable; // ���� ��ȣ�ۿ� ������ �������̽�

    public TextMeshProUGUI promptText; // ��ȣ�ۿ� ������Ʈ �ؽ�Ʈ
    Camera camera; // �÷��̾��� ���� ī�޶�
    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();

        // �ڽ��� �����ϴ� ĳ���Ͱ� �ƴ� ��� ��ȯ
        if (!pv.IsMine)
            return;

        // ������Ʈ �ؽ�Ʈ UI ��������
        promptText = GameObject.Find("prompt_Text (TMP)").GetComponent<TextMeshProUGUI>();
        promptText.gameObject.SetActive(false); // ó������ ������ �ʰ� ����

        camera = Camera.main; // ���� ī�޶� ����
    }

    void Update()
    {
        // �ڽ��� �����ϴ� ĳ���Ͱ� �ƴ� ��� ��ȯ
        if (!pv.IsMine)
            return;

        // ������ �˻� �󵵿� ���� ��ȣ�ۿ� ��� �˻�
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            CheckForInteractable(); // ��ȣ�ۿ� ������ ������Ʈ üũ
        }

        // ��ȣ�ۿ� �Է� ó��
        OnInteractInput();
    }

    void CheckForInteractable()
    {
        // ȭ�� �߾ӿ��� Raycast �߻�
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // ��ȣ�ۿ� ������ ������Ʈ�� �ִ��� �˻�
        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            GameObject hitObject = hit.collider.gameObject;

            // ���ο� ������Ʈ�� ��ȣ�ۿ��� �õ��� ���
            if (hitObject != curInteractGameobject)
            {
                curInteractGameobject = hitObject;
                curInteractable = hitObject.GetComponent<IInteractable>();

                if (curInteractable != null)
                {
                    SetPromptText(); // ��ȣ�ۿ� ������Ʈ �ؽ�Ʈ ����
                }
            }
        }
        else
        {
            ClearInteractable(); // ��ȣ�ۿ� ������ ����� ���� ��� �ʱ�ȭ
        }
    }

    void ClearInteractable()
    {
        // ��ȣ�ۿ� ���� �ؽ�Ʈ �ʱ�ȭ
        curInteractGameobject = null;
        curInteractable = null;
        promptText.gameObject.SetActive(false);
    }

    public void OnInteractInput()
    {
        // FŰ �Է� �� ���� �ٶ󺸰� �ִ� ��ȣ�ۿ� ������ ������Ʈ�� ��ȣ�ۿ�
        if (Input.GetKeyDown(KeyCode.F) && curInteractable != null)
        {
            Debug.Log("��ȣ�ۿ� �Է� ���� - ��ȣ�ۿ� ������ ������Ʈ �߰�");
            curInteractable.OnInteract(); // ��ȣ�ۿ� ����
            ClearInteractable(); // ��ȣ�ۿ� �� �ʱ�ȭ
        }
    }


    void SetPromptText()
    {
        // ������Ʈ �ؽ�Ʈ UI ǥ�� �� �ؽ�Ʈ ����
        promptText.gameObject.SetActive(true);
        promptText.text = string.Format("<b>[F]</b> {0}", curInteractable.GetInteractPrompt());
    }
}
