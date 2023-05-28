using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable
{
    // Start is called before the first frame update
    public Item target;

    PickupItem()
    {
        reusable = false;
    }

    public override void OnInteract()
    {
        if (target == null)
        {
            return;
        }

        GameManager.Instance.GetComponentInChildren<Inventory>().AddItem(target);
        Destroy(gameObject);
    }
}
