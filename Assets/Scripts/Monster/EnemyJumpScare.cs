using UnityEngine;
using System.Collections;
using Photon.Pun;

public class EnemyJumpScare : MonoBehaviourPun
{
    public Transform enemyFacePosition; // ���� �� ��ġ�� �ٶ󺸴� Transform
    public float zoomInDuration = 0.5f; // ���� �ð�
    private Camera mainCamera;
    private float originalFieldOfView;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isJumpScareActive = false;

    void Start()
    {
        if (!photonView.IsMine)
        {
            // PhotonView�� ���� �÷��̾��� ���� �ƴ϶�� �� ��ũ��Ʈ�� ��Ȱ��ȭ
            enabled = false;
            return;
        }

        mainCamera = CameraInfo.MainCam;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera�� ã�� �� �����ϴ�. ī�޶� Ȯ�����ּ���.");
        }
        else
        {
            originalFieldOfView = mainCamera.fieldOfView;
            Debug.Log("Main Camera�� FOV �ʱ�ȭ �Ϸ�");
        }
    }

    void Update()
    {
        if (isJumpScareActive && mainCamera.transform.parent == enemyFacePosition)
        {
            // enemyFacePosition�� �ڽ����� �ִ� ���� ī�޶��� ��ġ�� ȸ���� ���������� ����
            mainCamera.transform.localPosition = Vector3.zero;
            mainCamera.transform.localRotation = Quaternion.identity;
        }
    }

    public void TriggerJumpScare()
    {
        mainCamera = CameraInfo.MainCam;
        if (mainCamera != null)
        {
            Debug.Log("JumpScare ����");
            StartCoroutine(ZoomInOnEnemy());
        }
        else
        {
            Debug.LogError("Main Camera�� �ʱ�ȭ���� �ʾҽ��ϴ�. JumpScare�� ������ �� �����ϴ�.");
        }
    }

    IEnumerator ZoomInOnEnemy()
    {
        // ���� ī�޶��� �θ� ���� �θ�κ��� �и��ϰ�, enemyFacePosition�� �ڽ����� �����մϴ�.
        Transform originalParent = mainCamera.transform.parent;
        CameraRot cameraRotScript = mainCamera.GetComponent<CameraRot>();

        if (cameraRotScript != null)
        {
            cameraRotScript.isControlledExternally = true;  // CameraRot ������Ʈ ���߱�
        }

        mainCamera.transform.SetParent(enemyFacePosition);
        Debug.Log("ī�޶� enemyFacePosition�� �ڽ����� �����Ǿ����ϴ�.");

        // ī�޶��� ��ġ �� ȸ���� enemyFacePosition�� ���� ������ ����
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.identity;
        Debug.Log("ī�޶��� ��ġ�� ȸ���� enemyFacePosition�� ���� �����Ǿ����ϴ�.");

        // ���� �ð� ���� ���� (������ �ʿ� �����Ƿ� �ٷ� ���)
        yield return new WaitForSeconds(zoomInDuration);

        yield return new WaitForSeconds(1f); // 1�� ���� ����

        // ���� �θ�� �ٽ� �����ϰ� ��ġ�� ȸ���� ���ͽ�ŵ�ϴ�.
        mainCamera.transform.SetParent(originalParent);
        mainCamera.transform.position = originalPosition;
        mainCamera.transform.rotation = originalRotation;
        Debug.Log("ī�޶� ���� �θ�� �ٽ� ����Ǿ����ϴ�.");

        if (cameraRotScript != null)
        {
            cameraRotScript.isControlledExternally = false;  // CameraRot ������Ʈ �簳
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �浹�� ��ü���� PhotonView ������Ʈ�� �����ɴϴ�.
            PhotonView playerPhotonView = other.GetComponent<PhotonView>();

            // �÷��̾ ���� Ŭ���̾�Ʈ�� ��쿡�� ���� ���ɾ ����
            if (playerPhotonView != null && playerPhotonView.IsMine)
            {
                Debug.Log("�÷��̾�� �浹 ����. JumpScare Ʈ���� ȣ��");
                TriggerJumpScare();
            }
        }
    }

}
