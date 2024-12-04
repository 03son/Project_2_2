using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemFunction : MonoBehaviour
{
    protected void Tesettext()
    {
        GameObject.Find("ItemName_Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = "";
    }
}
public interface IItemFunction
{
    void Function(); //?????? ???? ???
}
