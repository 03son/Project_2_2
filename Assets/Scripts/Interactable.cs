using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void OnInteract()
    {
        Debug.Log("��ȣ�ۿ��: " + gameObject.name);
    }
}