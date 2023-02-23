using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool playerAnchored = false;
    private float DEFAULT_SPEED = 5.0f; // TODO: Move to separate settings class
    private float speed;

    public enum Facing // TODO: Move facing to enums.cs
    {
        Down,
        Up,
        Left,
        Right,
    }

    private UnityEngine.Vector2 target = new UnityEngine.Vector2(0, 0);
    private UnityEngine.Vector2 curPos;

    private bool currentlyMoving = false;
    private int faceDirection = 0; // 0 - Down, 1 - Up, 2 - Left, 3 - Right

    private GameObject PlayerTarget;
    private PlayerCollision CollisionHandler;

    void Start()
    {
        curPos = transform.position;
        //transform.Find("PlayerTarget").parent = null;
        PlayerTarget = GameObject.Find("PlayerTarget");
        PlayerTarget.transform.parent = null;
        CollisionHandler = PlayerTarget.GetComponent<PlayerCollision>();
        speed = DEFAULT_SPEED;
        PlayerTarget.transform.position = new UnityEngine.Vector2(curPos.x, curPos.y - 1);
    }

    public void AnchorPlayer(bool anchorState) // If anchorState == true then player will be locked in place
    {
        playerAnchored = anchorState;
    }

    public int GetFaceDirection() // Get direction player is facing.  0 - Down, 1 - Up, 2 - Left, 3 - Right
    {
        return faceDirection;
    }

    public bool IsMoving() // return whether player is moving or not
    {
        return currentlyMoving;
    }

    private void UpdateLocation() // In charge of setting our new target location based on input
    {
        float yAxis = Input.GetAxisRaw("Vertical");
        float xAxis = Input.GetAxisRaw("Horizontal");
        
        if (xAxis != 0 && !currentlyMoving ) // X Axis Movement
        {
            currentlyMoving = true;
            PlayerTarget.transform.position = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y);

            faceDirection = xAxis == 1 ? 3 : 2;

            if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(xAxis, 0), 1f)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(xAxis, 0), 0.45f)) // Check if we can move in the specified direction
            {
                target = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y);
                currentlyMoving = true;
                curPos = target;
                PlayerTarget.transform.position = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y);
            }

        }

        if (yAxis != 0 && !currentlyMoving) // Y Axis Movement
        {
            currentlyMoving = true;
            PlayerTarget.transform.position = new UnityEngine.Vector2(curPos.x, curPos.y + yAxis);

            faceDirection = yAxis == 1 ? 1 : 0;

            if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, yAxis), 1f)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, yAxis), 0.45f)) // Check if we can move in the specified direction
            {
                target = new UnityEngine.Vector2(curPos.x, curPos.y + yAxis);
                currentlyMoving = true;
                curPos = target;
                PlayerTarget.transform.position = new UnityEngine.Vector2(curPos.x, curPos.y + yAxis);
            }
        }

    }
    
    private void MoveSprite() // move the Player towards the target location 
    {
        transform.position = UnityEngine.Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if ((transform.position.x == target.x) && (transform.position.y == target.y))
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
    }
}
