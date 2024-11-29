using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRot : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 8f; // ȸ�� �ӵ�
    [SerializeField] private Transform playerTransform; // �÷��̾��� Transform

    [SerializeField] GameObject player;//�÷��̾�

    private float mouseX = 0f; // �¿� ȸ�� ��
    private float mouseY = 0f; // ���Ʒ� ȸ�� ��
    [SerializeField] private Vector3 offset; // ī�޶�� �÷��̾� ������ ����

    PhotonView pv;

    public GameObject FollowCam;
    public GameObject EquipCamera;

    PlayerState playerState;
    PlayerState.playerState state;

    public bool popup_escMenu = false; //esc T/F����
    void Awake()
    {
       
    }
    void Start()
    {
        player = this.gameObject.GetComponent<Transform>().parent.gameObject;
        playerTransform = player.transform;
        playerState = player.gameObject.GetComponent<PlayerState>();
        if (PhotonNetwork.IsConnected)
        {
            pv = player.GetComponent<PhotonView>();

            if (pv.IsMine)
            {
                GetComponent<AudioListener>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                GetComponent<AudioListener>().enabled = false;
                Destroy(FollowCam);
                Destroy(EquipCamera);
                Destroy(this.gameObject);
            }
        }
        else
        {
            GetComponent<AudioListener>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    void Update()
    {
        playerState.GetState(out state);
        if (popup_escMenu || state == PlayerState.playerState.Die) //esc â�� �����ְų� �׾������� ī�޶� ȸ��X
            return;

        mouseSpeed = GameInfo.MouseSensitivity; //���� ����ȭ

        if (PhotonNetwork.IsConnected)
        {
            if (pv.IsMine)
            {
                cameraPos();
            }
        }
        else
        {
            cameraPos();
        }
    }

    void cameraPos()
    {
        // ���콺 �Է��� �޾� ī�޶� ȸ�� ó��
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSpeed;

        // ���Ʒ� ȸ�� ���� ����
        mouseY = Mathf.Clamp(mouseY, -50f, 30f);

        // ī�޶��� ȸ�� ���� (�÷��̾��� ȸ���� ����)
        this.transform.localEulerAngles = new Vector3(mouseY, mouseX, 0);

        // ī�޶� ��ġ�� �÷��̾� ��ġ�� ����
        this.transform.position = playerTransform.position + offset;
    }
}