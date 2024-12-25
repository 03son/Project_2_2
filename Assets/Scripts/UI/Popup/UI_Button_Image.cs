using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Button_Image : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    void Start()
    {
        if (this.gameObject.activeSelf == true)
        {
            GetComponent<Image>().enabled = false;
        }
    }
    void OnEnable()
    {
        if (gameObject.activeSelf == true)
        {
            GetComponent<Image>().enabled = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().enabled = false;
    }
}
