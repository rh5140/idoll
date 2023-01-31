using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    private bool collisionDetected = false;
    private bool[] CollisionDir = {false, false, false, false}; // left, right, up, down

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionDetected = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionDetected = false;
    }

    public bool GetCollision()
    {
        return collisionDetected;
    }

    public bool[] GetCollisionDirections()
    {
        return CollisionDir;
    }

    public bool CheckCollision(UnityEngine.Vector2 dirVect, float dist)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirVect, dist);
        return (hit.collider != null);
    }
    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, UnityEngine.Vector2.left, 1.0f);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, UnityEngine.Vector2.right, 1.0f);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, UnityEngine.Vector2.up, 1.0f);
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, UnityEngine.Vector2.down, 1.0f);

        if (hit.collider != null)
            CollisionDir[0] = true;
        else
            CollisionDir[0] = false;


        if (hit2.collider != null)
            CollisionDir[1] = true;
        else
            CollisionDir[1] = false;

        if (hit3.collider != null)
            CollisionDir[2] = true;
        else
            CollisionDir[2] = false;

        if (hit4.collider != null)
            CollisionDir[3] = true;
        else
            CollisionDir[3] = false;
    }

    void Update()
    {
    }
}
