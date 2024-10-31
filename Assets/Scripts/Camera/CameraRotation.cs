using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRot : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 8f; // 회전 속도
    [SerializeField] private Transform playerTransform; // 플레이어의 Transform

    [SerializeField] GameObject player;//플레이어

    private float mouseX = 0f; // 좌우 회전 값
    private float mouseY = 0f; // 위아래 회전 값
    [SerializeField] private Vector3 offset; // 카메라와 플레이어 사이의 간격

    PhotonView pv;

    public GameObject FollowCam;
    public GameObject EquipCamera;

    void Awake()
    {

    }
    void Start()
    {
        player = this.gameObject.GetComponent<Transform>().parent.gameObject;
        playerTransform = player.transform;
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

    void Update()
    {
        if (pv.IsMine)
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
        mouseY = Mathf.Clamp(mouseY, -50f, 30f);

        // 카메라의 회전 적용 (플레이어의 회전을 따라감)
        this.transform.localEulerAngles = new Vector3(mouseY, mouseX, 0);

        // 카메라 위치를 플레이어 위치에 고정
        this.transform.position = playerTransform.position + offset;
    }
}