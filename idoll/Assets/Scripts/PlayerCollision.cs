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

        if (s == null && t == null) // if no renderers simply return
        {
            return;
        }

        collisionDetected = true; // we collided!
    }

    private void OnTriggerExit2D(Collider2D collision) // No more collision!
    {
        SpriteRenderer s = collision.GetComponent<SpriteRenderer>();
        TilemapRenderer t = collision.GetComponent<TilemapRenderer>();
        if (s == null && t == null) // if no renderers, ignore
        {
            return;
        }

        collisionDetected = false; // no more collisions!
    }
    
    public bool GetCollision() // Current Collision status
    {
        return collisionDetected;
    }

    public bool CheckCollision(UnityEngine.Vector2 dirVect, float dist) // Check if there is any collision using raycast in specified direction and distance.
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dirVect, dist); // RaycastAll to check for stacked colliders

        for (int numColliders = 0; numColliders < hit.Length; numColliders++) // Go through results from RayCast
        {
            // ABSOLUTELY PERFECT CODE TO check to see if sprites are visible (TODO: probably needs to be made better later)
            SpriteRenderer s = null;
            TilemapRenderer t = null;

            try 
            {
                s = hit[numColliders].collider.gameObject.GetComponent<SpriteRenderer>();
            }
            catch
            {
                Debug.Log("No SpriteRenderer");
            }

            try
            {
                t = hit[numColliders].collider.gameObject.GetComponent<TilemapRenderer>();
            }
            catch
            {
                Debug.Log("No TilemapRenderer");
            }

            if (s != null || t != null)
            {
                if (s == null && t.enabled) // If has enabled TilemapRenderer but no SpriteRenderer
                {
                    Debug.Log("Collided");
                    return (hit[numColliders].collider != null);
                }
                if (t == null && s.enabled) // If has enabled SpriteRenderer but no TilemapRenderer
                {
                    return (hit[numColliders].collider != null);
                }
            }
        }

        return false;
    }
}
