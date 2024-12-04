using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRot : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 8f; // ȸ�� �ӵ�
    [SerializeField] private Transform playerTransform; // �÷��̾��� Transform
    [SerializeField] private Transform cameraObject; // �� ������Ʈ Transform
    [SerializeField] GameObject player;//�÷��̾�

    public Transform mainCamera; // 메인 카메라 Transform
    public Transform playerModel; // 플레이어 모델링 Transform


    private float mouseX = 0f; // �¿� ȸ�� ��
    private float mouseY = 0f; // ���Ʒ� ȸ�� ��
    [SerializeField] private Vector3 offset; // ī�޶�� �÷��̾� ������ ����

    PhotonView pv;

    public GameObject FollowCam;
    public GameObject EquipCamera;

    PlayerState playerState;
    PlayerState.playerState state;

    public bool popup_escMenu = false; //esc T/F����

    public bool isControlledExternally = false;  // 외부에서 카메라를 제어하는 동안 true로 설정
    void Awake()
    {
       
    }
    void Start()
    {
        // �θ� ������Ʈ�� ã�� ���� �õ�
        if (this.transform.parent != null)
        {
            player = this.transform.parent.gameObject;
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player's parent object not found. Make sure the prefab is instantiated correctly.");
            return;
        }

        // ī�޶� ������Ʈ �Ҵ��� ���� �� ������Ʈ ã��
        if (cameraObject == null)
        {
            cameraObject = playerTransform.Find("Camera"); // �÷��̾��� �ڽ� �� "Camera"��� �̸��� �� ������Ʈ�� ã��
            if (cameraObject == null)
            {
                Debug.LogError("Camera object not found under player. Ensure that there is a child named 'Camera'.");
                return;
            }
        }

        player = this.gameObject.GetComponent<Transform>().parent.gameObject;
        playerTransform = player.transform;
        playerState = player.gameObject.GetComponent<PlayerState>();
        if (PhotonNetwork.IsConnected)
        {
            pv = player.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                // ���� �÷��̾��� ��� ī�޶� ����
                GetComponent<AudioListener>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                // ��Ʈ��ũ���� �� �÷��̾ �ƴ� ��� ó��
                GetComponent<AudioListener>().enabled = false;
                Destroy(FollowCam);
                Destroy(EquipCamera);
                Destroy(this.gameObject);
            }
        }
        else
        {
            // �̱� �÷��̾��� ���
            GetComponent<AudioListener>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // 동적으로 생성된 플레이어 오브젝트 찾기
        if (this.transform.parent != null)
        {
            player = this.transform.parent.gameObject;
            playerTransform = player.transform;

            // Player Modeling 찾기
            playerModel = playerTransform.Find("토끼모델링"); // "PlayerModel"은 모델링의 이름
            if (playerModel == null)
            {
                Debug.LogError("PlayerModel object not found under the player prefab.");
                return;
            }
        }
        else
        {
            Debug.LogError("Player's parent object not found. Make sure the prefab is instantiated correctly.");
            return;
        }
    }



    void Update()
    {
        playerState.GetState(out state);
        if (popup_escMenu || state == PlayerState.playerState.Die) //esc â�� �����ְų� �׾������� ī�޶� ȸ��X
            return;

        if (isControlledExternally)
            return; // 외부에서 제어 중일 때는 Update 중단

        mouseSpeed = GameInfo.MouseSensitivity;

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

        if (mainCamera != null && playerModel != null)
        {
            // 카메라의 Y축 회전을 모델링에 동기화
            Vector3 cameraRotation = mainCamera.rotation.eulerAngles;
            playerModel.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
        }
    }

    void cameraPos()
    {
        // ���콺 �Է��� �޾� ī�޶� ȸ�� ó��
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSpeed;

        // ���Ʒ� ȸ�� ���� ����
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        // ī�޶��� ȸ�� ���� (�÷��̾��� ȸ���� ����)
        this.transform.localEulerAngles = new Vector3(mouseY, mouseX, 0);

        // ī�޶� ��ġ�� �� ������Ʈ(Camera) ��ġ�� ����
        this.transform.position = cameraObject.position;
    }
}