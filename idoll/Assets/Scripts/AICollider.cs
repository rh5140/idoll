using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICollider : MonoBehaviour //Copied from PlayerCollision.cs
{

    private bool collisionDetected = false;
    private bool[] CollisionDir = { false, false, false, false }; // left, right, up, down

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

    public Vector2 NormalizeByCollision(Vector2 direction) { //Change movement to remove the colliding vector

        if (direction.y < 0)
        {
            direction.y = direction.y * (CollisionDir[3] ? 0 : 1); //Down
        } else
        {
            direction.y = direction.y * (CollisionDir[2] ? 0 : 1); //Up
        }

        if (direction.x < 0)
        {

            direction.x = direction.x * (CollisionDir[1] ? 0 : 1); //Left
        }
        else
        {
            direction.x = direction.x * (CollisionDir[0] ? 0 : 1); //Right
        }

        return direction;
    }

    private void FixedUpdate()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, UnityEngine.Vector2.left, 1.0f);
        RaycastHit2D[] hit2 = Physics2D.RaycastAll(transform.position, UnityEngine.Vector2.right, 1.0f);
        RaycastHit2D[] hit3 = Physics2D.RaycastAll(transform.position, UnityEngine.Vector2.up, 1.0f);
        RaycastHit2D[] hit4 = Physics2D.RaycastAll(transform.position, UnityEngine.Vector2.down, 1.0f);

        if (hit.Length > 1)
            CollisionDir[0] = true;
        else
            CollisionDir[0] = false;


        if (hit2.Length > 1)
            CollisionDir[1] = true;
        else
            CollisionDir[1] = false;

        if (hit3.Length > 1)
            CollisionDir[2] = true;
        else
            CollisionDir[2] = false;

        if (hit4.Length > 1)
            CollisionDir[3] = true;
        else
            CollisionDir[3] = false;

    }

    void Update()
    {
    }
}