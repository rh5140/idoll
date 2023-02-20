using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleUseItem : Item
{
    public SingleUseItem(string name, Sprite sprite)
        : base(name, false, sprite)
    {
        
    }

    public override bool Use(int i = 1)
    {
        if (inventory.Count(this) != 0)
        {
            inventory.RemoveItem(this, i);

            Effect();

            return true;
        }

        else
        {
            return false;
        }
    }

    protected override void Effect() 
    {
        Debug.Log("Used item " + name);
    }
}
