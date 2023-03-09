using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurabilityItem : Item
{
    public int maxDurability;

    public DurabilityItem(string name, int maxDur)
        : base(name, true, 1)
    {    
        maxDurability = maxDur;
    }

    protected DurabilityItem(DurabilityItem item)
        : base(item)
    {
        maxDurability = item.maxDurability;
    }

    public override void Use(int i = 1)
    {
        Effect();
    }

    protected override void Effect()
    {
        Debug.Log("Used item: " + name + "\nMax durability: " + maxDurability);
    }

    /*public override Item Copy()
    {
        return (Item) ScriptableObject.CreateInstance(this);
    }*/
}
