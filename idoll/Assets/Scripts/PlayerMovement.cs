using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float Speed = 5.0f; //Public so it is accessible by other objects
    private bool playerAnchored = false;

    private UnityEngine.Vector2 directionOffset = UnityEngine.Vector2.up; //TODO : Add directions to enums

    private Grid myGrid;

    private UnityEngine.Vector2 target = new UnityEngine.Vector2(0, 0);
    private UnityEngine.Vector2 curPos;

    private bool currentlyMoving = false;


    // Start is called before the first frame update
    void Start()
    {
        myGrid = (GameObject.Find("Grid")).GetComponent<Grid>();
        curPos = transform.Find("PlayerSprite").position;

    }

    //Get direction from 
    private UnityEngine.Vector2 getDirectionFromAxes(float x, float y, UnityEngine.Vector2 previous)
    {
        if (x == y && x == 0) //If the player is not moving, return the previously facing direction
        {
            return previous;
        }

        return new UnityEngine.Vector2(x == 0 ? 0 : (x / Math.Abs(x)), y == 0 ? 0 : (y / Math.Abs(y)));
    }

    public void AnchorPlayer(bool anchorState)
    {
        playerAnchored = anchorState;
    }


    private void UpdateLocation()
    {
        float yAxis = Input.GetAxisRaw("Vertical");
        float xAxis = Input.GetAxisRaw("Horizontal");

        

        if (xAxis != 0 && yAxis != 0) // Diagonal Movement
        {
            currentlyMoving = true;
            directionOffset = getDirectionFromAxes(xAxis, yAxis, directionOffset);
            transform.Find("PlayerTarget").GetComponent<Rigidbody2D>().MovePosition(new UnityEngine.Vector2(curPos.x + xAxis, curPos.y + yAxis));

            if (!transform.Find("PlayerTarget").GetComponent<PlayerCollision>().GetCollision() && !transform.Find("PlayerTarget").GetComponent<PlayerCollision>().CheckCollision(new UnityEngine.Vector2(xAxis, yAxis), 0.85f))
            {
                target = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y + yAxis);
                currentlyMoving = true;
                curPos = target;
            }
        }

        else if (xAxis != 0 && !currentlyMoving ) // X Axis Movement
        { 
            currentlyMoving = true;
            directionOffset = getDirectionFromAxes(xAxis, yAxis, directionOffset);
            transform.Find("PlayerTarget").GetComponent<Rigidbody2D>().MovePosition(new UnityEngine.Vector2(curPos.x + xAxis, curPos.y));
            bool[] collisionDirs = transform.Find("PlayerTarget").GetComponent<PlayerCollision>().GetCollisionDirections(); // left, right, up, down


            if ((!transform.Find("PlayerTarget").GetComponent<PlayerCollision>().GetCollision() || !((xAxis == 1 && collisionDirs[1]) || (xAxis == -1 && collisionDirs[0]))) && !transform.Find("PlayerTarget").GetComponent<PlayerCollision>().CheckCollision(new UnityEngine.Vector2(xAxis, 0), 0.75f))
            {
                target = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y);
                currentlyMoving = true;
                curPos = target;
            }

        }

        else if (yAxis != 0 && !currentlyMoving) // Y Axis Movement
        {
            currentlyMoving = true;
            directionOffset = getDirectionFromAxes(xAxis, yAxis, directionOffset);
            transform.Find("PlayerTarget").GetComponent<Rigidbody2D>().MovePosition(new UnityEngine.Vector2(curPos.x, curPos.y + yAxis));
            bool[] collisionDirs = transform.Find("PlayerTarget").GetComponent<PlayerCollision>().GetCollisionDirections(); // left, right, up, down


            if ((!transform.Find("PlayerTarget").GetComponent<PlayerCollision>().GetCollision() || !((yAxis == 1 && collisionDirs[2]) || (yAxis == -1 && collisionDirs[3]))) && !transform.Find("PlayerTarget").GetComponent<PlayerCollision>().CheckCollision(new UnityEngine.Vector2(0, yAxis), 0.75f))
            {
                target = new UnityEngine.Vector2(curPos.x, curPos.y + yAxis);
                currentlyMoving = true;
                curPos = target;
            }
        }

    }
    
    private void MoveSprite()
    {
        transform.Find("PlayerSprite").position = UnityEngine.Vector3.MoveTowards(transform.Find("PlayerSprite").position, target, Speed * Time.deltaTime);

        if ((transform.Find("PlayerSprite").position.x == target.x) && (transform.Find("PlayerSprite").position.y == target.y))
        {
            currentlyMoving = false;
        }
    }

    private void MoveInteractor()
    {
        UnityEngine.Vector3 dir = directionOffset;
        transform.Find("PlayerInteractor").position = transform.Find("PlayerSprite").position + dir;

    }

    // Update is called once per frame
    void Update()
    {
        if (!playerAnchored && !currentlyMoving)
            UpdateLocation();

        if (currentlyMoving) {
            MoveSprite();
            MoveInteractor();
        }

        transform.Find("PlayerTarget").position = transform.Find("PlayerSprite").position;
    }
}
