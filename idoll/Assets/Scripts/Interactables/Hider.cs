using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : Interactable
{
    public GameObject target;
    // Start is called before the first frame update

    public override void OnInteract()
    {
        if (target == null) {
            return;
        }
        SpriteRenderer s = target.GetComponent<SpriteRenderer>();
        if (s != null)
        {
            s.enabled = !s.enabled;
        }
    }


}
