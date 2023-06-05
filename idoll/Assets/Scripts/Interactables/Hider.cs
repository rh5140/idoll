using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hider : Interactable
{
    public GameObject target;


    protected override void Start()
    {
        if (this.activated % 2 == 1) // %2 deals with uninteracting
        {
            OnInteract(); // Hides the object if it was hidden beforehand
        }
    }


    public override void OnInteract()
    {
        if (target == null) {
            return;
        }
        SpriteRenderer s = target.GetComponent<SpriteRenderer>();
        TilemapRenderer t = target.GetComponent<TilemapRenderer>();
        if (s != null)
        {
            s.enabled = !s.enabled;
        }
        if (t != null)
        {
            t.enabled = !t.enabled;
        }
    }


}
