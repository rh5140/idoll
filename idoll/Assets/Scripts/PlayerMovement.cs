using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool playerAnchored = false;
    private float speed = 5.0f;

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

            transform.Find("PlayerTarget").GetComponent<Rigidbody2D>().MovePosition(new UnityEngine.Vector2(curPos.x + xAxis, curPos.y + yAxis));

            if (!transform.Find("PlayerTarget").GetComponent<PlayerCollision>().GetCollision() && !transform.Find("PlayerTarget").GetComponent<PlayerCollision>().CheckCollision(new UnityEngine.Vector2(xAxis, yAxis), 0.85f))
            {
                target = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y + yAxis);
                currentlyMoving = true;
                curPos = target;
            }
        }

        if (xAxis != 0 && !currentlyMoving ) // X Axis Movement
        { 
            currentlyMoving = true;
            transform.Find("PlayerTarget").GetComponent<Rigidbody2D>().MovePosition(new UnityEngine.Vector2(curPos.x + xAxis, curPos.y));
            bool[] collisionDirs = transform.Find("PlayerTarget").GetComponent<PlayerCollision>().GetCollisionDirections(); // left, right, up, down


            if ((!transform.Find("PlayerTarget").GetComponent<PlayerCollision>().GetCollision() || !((xAxis == 1 && collisionDirs[1]) || (xAxis == -1 && collisionDirs[0]))) && !transform.Find("PlayerTarget").GetComponent<PlayerCollision>().CheckCollision(new UnityEngine.Vector2(xAxis, 0), 0.75f))
            {
                target = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y);
                currentlyMoving = true;
                curPos = target;
            }

        }

        if (yAxis != 0 && !currentlyMoving) // Y Axis Movement
        {
            currentlyMoving = true;
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
        transform.Find("PlayerSprite").position = UnityEngine.Vector3.MoveTowards(transform.Find("PlayerSprite").position, target, speed * Time.deltaTime);

        if ((transform.Find("PlayerSprite").position.x == target.x) && (transform.Find("PlayerSprite").position.y == target.y))
        {
            currentlyMoving = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerAnchored && !currentlyMoving)
            UpdateLocation();

        if (currentlyMoving)
            MoveSprite();

        transform.Find("PlayerTarget").position = transform.Find("PlayerSprite").position;
    }
}
