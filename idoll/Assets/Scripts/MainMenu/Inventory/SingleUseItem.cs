using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleUseItem : Item
{
    public SingleUseItem(string name, int maxCount, string description)
        : base(name, false, maxCount, description)
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

    /*public override Item Copy()
    {
        return (Item) ScriptableObject.CreateInstance(this);
    }*/
}
