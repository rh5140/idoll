using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurabilityItem : Item
{
    public int durability;
    public int maxDurability;

    public DurabilityItem(string name, int dur, int maxDur, string description)
        : base(name, true, 1, description)
    {    
        durability = dur;
        maxDurability = maxDur;
    }

    protected DurabilityItem(DurabilityItem item)
        : base(item)
    {
        durability = item.durability;
        maxDurability = item.maxDurability;
    }

    public override void Use(int i = 1)
    {
        Effect();
    }

    protected override void Effect()
    {
        Debug.Log("Used item: " + name + "\nDurability left: " + durability + "\\" + maxDurability);
    }

    /*public override Item Copy()
    {
        return (Item) ScriptableObject.CreateInstance(this);
    }*/
}
