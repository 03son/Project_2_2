using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;   // ��ȣ�ۿ� ������ �Ÿ�
    public LayerMask interactionLayer;    // ��ȣ�ۿ� ������ ���̾� ����
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;       // �÷��̾��� ���� ī�޶� ��������
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))  // E Ű�� ������ �� ��ȣ�ۿ� �õ�
        {
            Interact();
        }
    }

    void Interact()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // interactionLayer�� ������ ������Ʈ�� �����ϵ��� ����ĳ��Ʈ
        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            Debug.Log("����ĳ��Ʈ ��Ʈ: " + hit.collider.gameObject.name);

            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.OnInteract();
            }
        }
        else
        {
            Debug.Log("����ĳ��Ʈ�� �ƹ��͵� �������� ���߽��ϴ�.");
        }
    }
}
