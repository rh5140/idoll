using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public string name {get; private set;}
    public bool hasDurability {get; private set;}
    public Sprite sprite {get; private set;}
    public Inventory inventory {get; private set;}

    public Item(string n, bool hasDur, Sprite s)
    {
        name = n;
        hasDurability = hasDur;
        sprite = s;
        inventory = Inventory.instance;
    }

    public Item(Item item)
    {
        name = item.name;
        hasDurability = item.hasDurability;
        sprite = item.sprite;
        inventory = item.inventory;
    }

    public abstract Item Copy();

    // Remove one instance/damage item in inventory, then perform Effect.
    public abstract bool Use(int i = 1);

    // Do whatever this item should do
    protected abstract void Effect();
}
