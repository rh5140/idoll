using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KeyItem : SingleUseItem
{
    public string DoorName;

    public KeyItem() : base("KeyItem",1)
    {}

    public KeyItem(KeyItem item)
        : base(item)
    {}

    protected override void Effect()
    {
        base.Effect();
        Debug.Log("Attempted to unlock door : " + DoorName);
    }

}
