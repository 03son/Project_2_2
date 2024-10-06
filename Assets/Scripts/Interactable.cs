using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void OnInteract()
    {
        Debug.Log("상호작용됨: " + gameObject.name);
    }
}