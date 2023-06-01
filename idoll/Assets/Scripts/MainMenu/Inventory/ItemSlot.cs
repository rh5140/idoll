using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int itemStackIndex;

    public Item item;
    public Image icon;
    public string itemDescription;
    public TextMeshProUGUI amountOrDurability;
    public Image selectBorder;
    public TextMeshProUGUI description;

    void Start()
    {
        itemStackIndex = -1;
        //amountOrDurability.text = "";
        itemDescription = "";
        icon.enabled = false;
        item = null;

        selectBorder.enabled = false;
    }

    public void AddItem(Item i, int n, int stackIndex = -1)
    {
        item = i;
        icon.enabled = true;
        icon.sprite = item.sprite;
        itemDescription = item.description;

        itemStackIndex = stackIndex;

        if (item.hasDurability)
        {
            DurabilityItem durItem = (DurabilityItem) item;
            //amountOrDurability.text = n + "/" + durItem.maxDurability;
        }
        else
        {
            //amountOrDurability.text = n.ToString();
        }
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        //amountOrDurability.text = "";
        itemDescription = "";
    }

    public void OnUseItem()
    {
        if (item == null)
        {
            Debug.Log("No item to use in this slot!");
        }
        else
        {
            Debug.Log("Used " + item.name);
            Inventory.instance.UseItem(item, 1);
        }
    }

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
        OnUseItem();
    }

    //Use these methods for keyboard navigation too
    public void OnSelect()
    {
        selectBorder.enabled = true;

        if (item == null)
            description.text = "";
        else
            description.text = "<size=11><color=#8e4b9e>[" + item.name + "]</color> details</size><br>" + itemDescription;

        //Debug.Log("Mouse enter");
    }

    public void OnExit()
    {
        selectBorder.enabled = false;

        description.text = "";

        //Debug.Log("Mouse exit");
    }
}
