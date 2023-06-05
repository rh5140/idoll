using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KeyItem : SingleUseItem
{
    public string DoorName;

    public KeyItem() : base("KeyItem",1, "It's a key.")
    {}

    public KeyItem(KeyItem item)
        : base(item)
    {}

    public override bool Use(int i = 1)
    {
        GameObject target = GameObject.Find("PlayerTarget");
        if (target != null)
        {
            Interactable obj = target.GetComponent<PlayerInteractor>().currentInteractable;
            if (obj != null && obj.GetComponent<LockedDoor>() != null)
            {
                LockedDoor door = obj.GetComponent<LockedDoor>();
                if (door.doorName == DoorName)
                {
                    door.Unlock();
                    return base.Use(i);
                }
            }
        }
        return false;
    }

    protected override void Effect()
    {
        base.Effect();
        
        Debug.Log("Attempted to unlock door : " + DoorName);
    }

}
