using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRot : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 8f; // 회전 속도
    [SerializeField] private Transform playerTransform; // 플레이어의 Transform
    [SerializeField] private Transform cameraObject; // 빈 오브젝트 Transform
    [SerializeField] GameObject player;//플레이어

    private float mouseX = 0f; // 좌우 회전 값
    private float mouseY = 0f; // 위아래 회전 값
    [SerializeField] private Vector3 offset; // 카메라와 플레이어 사이의 간격

    PhotonView pv;

    public GameObject FollowCam;
    public GameObject EquipCamera;

    public bool popup_escMenu = false; //esc T/F여부
    void Awake()
    {
       
    }
    void Start()
    {
        // 부모 오브젝트를 찾기 위한 시도
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

        // 카메라 오브젝트 할당을 위해 빈 오브젝트 찾기
        if (cameraObject == null)
        {
            cameraObject = playerTransform.Find("Camera"); // 플레이어의 자식 중 "Camera"라는 이름의 빈 오브젝트를 찾음
            if (cameraObject == null)
            {
                Debug.LogError("Camera object not found under player. Ensure that there is a child named 'Camera'.");
                return;
            }
        }

        if (PhotonNetwork.IsConnected)
        {
            pv = player.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                // 로컬 플레이어인 경우 카메라 설정
                GetComponent<AudioListener>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                // 네트워크에서 내 플레이어가 아닌 경우 처리
                GetComponent<AudioListener>().enabled = false;
                Destroy(FollowCam);
                Destroy(EquipCamera);
                Destroy(this.gameObject);
            }
        }
        else
        {
            // 싱글 플레이어일 경우
            GetComponent<AudioListener>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }



    void Update()
    {
        if (popup_escMenu) //esc 창이 열려있으면 카메라 회전X
            return;

        mouseSpeed = GameInfo.MouseSensitivity; //감도 동기화

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
        // 마우스 입력을 받아 카메라 회전 처리
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSpeed;

        // 위아래 회전 각도 제한
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        // 카메라의 회전 적용 (플레이어의 회전을 따라감)
        this.transform.localEulerAngles = new Vector3(mouseY, mouseX, 0);

        // 카메라 위치를 빈 오브젝트(Camera) 위치로 고정
        this.transform.position = cameraObject.position;
    }
}