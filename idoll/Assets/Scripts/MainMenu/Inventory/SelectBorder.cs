using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SelectBorder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ItemSlot parent;
    public Image selfImage;
    public TextMeshProUGUI description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        parent.OnUseItem();
    }

    void Start()
    {
        selfImage.enabled = false;

        description = Inventory.instance.descriptionBox;
        description.text = "";
    }


    public void OnSelect()
    {
        selfImage.enabled = true;

        description.text = "<size=9><color=#8e4b9e>[" + parent.item.name + "]</color> details</size>\n" + parent.itemDescription;

        Debug.Log("Mouse enter");
    }

    public void OnExit()
    {
        selfImage.enabled = false;

        description.text = "";

        Debug.Log("Mouse exit");
    }
}
