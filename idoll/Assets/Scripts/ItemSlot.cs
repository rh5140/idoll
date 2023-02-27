using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI amountOrDurability;
    new public TextMeshProUGUI name;

    private Item item;

    void Start()
    {
        removeButton.interactable = false;
        amountOrDurability.text = "";
        item = null;
        name.text = "";
    }

    public void AddItem(Item i, int n)
    {
        item = i;

        icon.sprite = i.sprite;
        icon.enabled = true;
        removeButton.interactable = true;
        name.text = i.name;

        if (item.hasDurability)
        {
            DurabilityItem durItem = (DurabilityItem) item;
            amountOrDurability.text = durItem.durability + "/" + durItem.maxDurability;
        }
        else
        {
            amountOrDurability.text = n.ToString();
        }
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        amountOrDurability.text = "";
        name.text = "";
    }

    public void OnUseItem()
    {
        Inventory.instance.UseItem(item, 1);
    }

    public void OnRemoveItem()
    {
        Inventory.instance.RemoveItem(item);
    }
}
