using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.UI.Image;

public class PlayerCollision : MonoBehaviour
{
    private bool collisionDetected = false;
    private ObjectMovement movementScript = null;
    private PlayerMovement PlrMovement;

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

    // Start is called before the first frame update
    void Start()
    {
        PlrMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    public bool CheckCollision(UnityEngine.Vector2 dirVect, float dist, GameObject center = null) // Check if there is any collision using raycast in specified direction and distance.
    {

        RaycastHit2D[] hit;
        if (center != null)
        {
            center.layer = LayerMask.NameToLayer("Ignore Raycast");
            hit = Physics2D.RaycastAll(center.transform.position, dirVect, dist);
            center.layer = LayerMask.NameToLayer("Default");

        }
        else
            hit = Physics2D.RaycastAll(transform.position, dirVect, dist); // RaycastAll to check for stacked colliders

        for (int numColliders = 0; numColliders < hit.Length; numColliders++)
        {
            // ABSOLUTELY PERFECT CODE TO check to see if sprites are visible (TODO: probably needs to be made better later)
            if ((hit[numColliders].collider.gameObject).tag == "Movable")
            {
                if (CheckCollision(dirVect, 1.5f, (hit[numColliders].collider.gameObject)) || !PlrMovement.broom_game || !(hit[numColliders].collider.gameObject).GetComponent<ObjectMovement>().game_active)
                    return true;
                return false;
            }
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

            if (s != null || t != null)
            {
                if (s == null && t.enabled)
                {
                    //Debug.Log("Collided");
                    return (hit[numColliders].collider != null);
                }
                if (t == null && s.enabled)
                {
                    return (hit[numColliders].collider != null);
                }
            }
        }
        return false;
    }

    public bool checkMovable(UnityEngine.Vector2 dirVect, float dist, UnityEngine.Vector2 centerPos, int special_move = -1, int old_dir = -1)
    {
 
            RaycastHit2D[] hit = Physics2D.RaycastAll(centerPos, dirVect.normalized, 1.25f); // RaycastAll to check for stacked colliders
            Debug.Log("FAF");
            Debug.Log(hit.Length);

            for (int numColliders = 0; numColliders < hit.Length; numColliders++)
            {
                SpriteRenderer s = null;
                TilemapRenderer t = null;
                try
                {
                    s = hit[numColliders].collider.gameObject.GetComponent<SpriteRenderer>();
                }
                catch { }

                if (s != null && s.enabled)
                {
                    Debug.Log("EBA");

                    if ((hit[numColliders].collider.gameObject).tag == "Movable")
                    {
                        Debug.Log("Found Movable!");

                        movementScript = (hit[numColliders].collider.gameObject).GetComponent<ObjectMovement>();

                        UnityEngine.Vector2 move_vector = dirVect.normalized;
                        UnityEngine.Vector2 cent = centerPos;
                        bool skip_adjust = false;
                        if (special_move != -1)
                        {
                            skip_adjust = true;
                            cent = hit[numColliders].transform.position;
                            Debug.Log("Center: ");
                            Debug.Log(cent);
                            if (special_move == 2)
                            {
                                if (old_dir == 1)
                                    move_vector = new UnityEngine.Vector2(0f, -1f);
                                else
                                    move_vector = new UnityEngine.Vector2(0f, 1f);
                            }
                            else if (special_move == 3)
                            {
                                if (old_dir == 1)
                                    move_vector = new UnityEngine.Vector2(0f, -1f);
                                else
                                    move_vector = new UnityEngine.Vector2(0f, 1f);
                            }
                            else if (special_move == 1)
                            {
                                if (old_dir == 2)
                                    move_vector = new UnityEngine.Vector2((-1f) + (2 * special_move), 0f);
                                else
                                    move_vector = new UnityEngine.Vector2((1f) - (2 * special_move), 0f);
                            }
                            else
                            {
                                if (old_dir == 2)
                                    move_vector = new UnityEngine.Vector2((1f) - (2 * special_move), 0f);
                                else
                                    move_vector = new UnityEngine.Vector2((-1f) + (2 * special_move), 0f);
                            }

                        }

                        if (CheckCollision(move_vector, 0.75f, (hit[numColliders].collider.gameObject)))
                        {
                            Debug.Log("UNGA MEH BUNGA");

                            return (special_move == -1); // true;
                        }

                        movementScript.MoveObjTo(move_vector, cent, skip_adjust);
                        return true;
                    }
                    else
                    {
                    Debug.Log("NAY!");

                    //return true;
                }
            }
            }
      

        return false;
    }
}
