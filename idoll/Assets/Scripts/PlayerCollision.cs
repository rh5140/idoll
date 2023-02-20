using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private bool collisionDetected = false;

    private void OnTriggerEnter2D(Collider2D collision) // Oh no we collided!
    {
        collisionDetected = true;
    }

    private void OnTriggerExit2D(Collider2D collision) // No more collision!
    {
        collisionDetected = false;
    }
    
    public bool GetCollision()
    {
        return collisionDetected;
    }

    public bool CheckCollision(UnityEngine.Vector2 dirVect, float dist) // Check if there is any collision using raycast in specified direction and distance.
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirVect, dist);
        return (hit.collider != null);
    }
}
