using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public int itemStackIndex;
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI amountOrDurability;
    new public TextMeshProUGUI name;

    private Item item;

    void Start()
    {
        itemStackIndex = -1;
        removeButton.interactable = false;
        amountOrDurability.text = "";
        item = null;
        name.text = "";
    }

    public void AddItem(Item i, int n, int stackIndex = -1)
    {
        item = i;
        icon.sprite = item.sprite;
        icon.enabled = true;
        removeButton.interactable = true;
        name.text = item.name;

        itemStackIndex = stackIndex;

        if (item.hasDurability)
        {
            DurabilityItem durItem = (DurabilityItem) item;
            amountOrDurability.text = n + "/" + durItem.maxDurability;
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
        if (item == null)
        {
            Debug.Log("No item to use in this slot!");
        }
        else
        {
            Inventory.instance.UseItem(item, 1);
        }
    }

    public void OnRemoveItem()
    {
        if (item == null)
        {
            Debug.Log("No item to remove in this slot!");
        }
        else
        {
            Inventory.instance.RemoveItemStack(item, itemStackIndex);
        }
    }
}
