using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float DetectionRadius = 10f; //Detection Radius for player. Can try different iterations.
    public float Speed = 2.0f;
    public bool CanMove = true;

    private Vector3 StartPosition;

    private bool DetectPlayer() //Whether or not the AI detects the player
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player is null)
        {
            return false;
        }
        if (Vector2.Distance(StartPosition,player.transform.Find("PlayerSprite").position) <= DetectionRadius)
        {
            return true;
        }
        return false;
    }

    private Vector2 DecideChaseDirection() //Decide the direction. A* or fixed-depth will probably replace this
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player is null)
        {
            return Vector2.zero;
        }
        Vector2 direction = (player.transform.Find("PlayerSprite").position - transform.position);
        return direction;
    }

    private Vector2 DecideReturnDirection() //Decide the direction. A* or fixed-depth will probably replace this
    {
 
        Vector2 direction = (StartPosition - transform.position);
        return direction;
    }

    private bool AttemptMove(UnityEngine.Vector2 direction) //Attempt to move in a given direction
    {
        direction = transform.GetComponent<AICollider>().NormalizeByCollision(direction);
        if (direction == Vector2.zero)
        {
            return false;
        }

        transform.Translate(direction.normalized * Speed * Time.deltaTime);
        return true;
    }

    void Start()
    {
        StartPosition = transform.position; //Set the starting position
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            if (DetectPlayer())
            {
                AttemptMove(DecideChaseDirection());
            } else {
                AttemptMove(DecideReturnDirection());
            }
            
        }
    }
}
