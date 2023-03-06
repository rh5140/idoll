using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hider : Interactable
{
    public GameObject target;
    // Start is called before the first frame update

    protected override void interact()
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
