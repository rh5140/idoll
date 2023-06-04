using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable
{
    // Start is called before the first frame update
    public Item target;

    public PickupItem()
    {
        reusable = false;
    }

    public override void OnInteract()
    {
        if (target == null)
        {
            return;
        }

        GameObject inv = GameObject.FindGameObjectWithTag("Inventory");
        if (inv != null)
        {
            inv.GetComponent<Inventory>().AddItem(target);
            Destroy(gameObject);
        } else
        {
            //Do nothing i guess?
            Debug.Log("Could not find inventory!");
        }
        
    }
}
