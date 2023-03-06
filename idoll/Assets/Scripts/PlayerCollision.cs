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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirVect, dist);

        // ABSOLUTELY PERFECT CODE TO check to see if sprites are visible (probably needs to be made better later)
        SpriteRenderer s = null;
        TilemapRenderer t = null;
        try
        {
            s = hit.collider.gameObject.GetComponent<SpriteRenderer>();
        }
        catch { }
        try
        {
            t = hit.collider.gameObject.GetComponent<TilemapRenderer>();
        }
        catch { }

        if (s == null && t == null)
            return false;

        if (s == null &&t.enabled)
        {
            return (hit.collider != null);
        }
        if (t == null && s.enabled)
        {
            return (hit.collider != null);
        }
        return false;
    }
}
