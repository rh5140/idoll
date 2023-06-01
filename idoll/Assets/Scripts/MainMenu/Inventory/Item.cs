using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    new public string name;
    public bool hasDurability;
    public int maxCount;
    public Sprite sprite;
    public string description;

    public Item(string n, bool hasDur, int maxC, string description)
    {
        name = n;
        hasDurability = hasDur;
        maxCount = maxC;
        this.description = description;
    }

    public Item(Item item)
    {
        name = item.name;
        hasDurability = item.hasDurability;
        maxCount = item.maxCount;
        sprite = item.sprite;
        description = item.description;
    }

    //public abstract Item Copy();

    // Remove one instance/damage item in inventory, then perform Effect.
    public abstract void Use(int i = 1);

    // Do whatever this item should do
    protected abstract void Effect();
}
