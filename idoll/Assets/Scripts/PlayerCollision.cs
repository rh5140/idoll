using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerCollision : MonoBehaviour
{
    private bool collisionDetected = false;

    private void OnTriggerEnter2D(Collider2D collision) // Oh no we collided!
    {
        SpriteRenderer s = collision.GetComponent<SpriteRenderer>();
        TilemapRenderer t = collision.GetComponent<TilemapRenderer>();

        if (s == null && t == null)
        {
            return;
        }

        collisionDetected = true;
    }

    private void OnTriggerExit2D(Collider2D collision) // No more collision!
    {
        SpriteRenderer s = collision.GetComponent<SpriteRenderer>();
        TilemapRenderer t = collision.GetComponent<TilemapRenderer>();
        if (s == null && t == null)
        {
            return;
        }

        collisionDetected = false;
    }
    
    public bool GetCollision()
    {
        return collisionDetected;
    }

    public bool CheckCollision(UnityEngine.Vector2 dirVect, float dist) // Check if there is any collision using raycast in specified direction and distance.
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dirVect, dist); // RaycastAll to check for stacked colliders
        bool hasCollided = false;

        for (int numColliders = 0; numColliders < hit.Length; numColliders++)
        {
            //Debug.Log(numColliders + " | " + hit[numColliders].collider.gameObject.name);

            // ABSOLUTELY PERFECT CODE TO check to see if sprites are visible (TODO: probably needs to be made better later)
            SpriteRenderer s = null;
            TilemapRenderer t = null;
            try
            {
                s = hit[numColliders].collider.gameObject.GetComponent<SpriteRenderer>();
            }
            catch { }
            try
            {
                t = hit[numColliders].collider.gameObject.GetComponent<TilemapRenderer>();
            }
            catch { }

            if (s != null && s.gameObject.tag == "Activator")
            {
                s.GetComponentInParent<Interactable>().interact();
            }

            else if (s != null && s.gameObject.tag == "Companion") {
                if (s.gameObject.GetComponent<Companion>().IsGhosted() == false) {
                    hasCollided = true; // Collision if companion is not ghosted!
                }
            }

            else if (s != null || t != null)
            {
                if ((s == null && t.enabled) || (t == null && s.enabled))
                {
                    if (hit[numColliders].collider != null)
                    {
                        hasCollided = true;
                    }
                }
            }
        }

        return hasCollided;
    }
}
