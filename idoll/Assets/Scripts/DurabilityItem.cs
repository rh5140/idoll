using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DurabilityItem : Item
{
    public int durability {get; private set;}
    public int maxDurability {get; private set;}

    public DurabilityItem(string name, Sprite sprite, int dur, int maxDur)
        : base(name, true, sprite)
    {    
        durability = dur;
        maxDurability = maxDurability;
    }

    // All descendant Classes must have a similar constructor to copy itself.
    protected DurabilityItem(DurabilityItem item)
        : base(item)
    {
        durability = item.durability;
        maxDurability = item.maxDurability;
    }

    public override void Use(int i = 1)
    {
        durability -= i;

        Effect();
    }

    protected override void Effect()
    {
        Debug.Log("Used item: " + name + "\nDurability left: " + durability + "\\" + maxDurability);
    }
}
