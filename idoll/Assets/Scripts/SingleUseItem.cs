using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleUseItem : Item
{
    public SingleUseItem(string name, Sprite sprite)
        : base(name, false, sprite)
    {
        
    }

    public SingleUseItem(SingleUseItem item)
        : base(item)
    {
        
    }

    public override void Use(int i = 1)
    {
        Effect();
    }

    protected override void Effect() 
    {
        Debug.Log("Used item " + name);
    }
}
