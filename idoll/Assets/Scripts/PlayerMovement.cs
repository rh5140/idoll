using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem; // New Unity input system!

public class PlayerMovement : MonoBehaviour
{
    // InputSystem
    private Inputs input = null;
    private UnityEngine.Vector2 moveVector = UnityEngine.Vector2.zero;

    private bool playerAnchored = false;
    private float DEFAULT_SPEED = 5.0f; // TODO: Move to separate settings class
    private float speed;
    private float scan_distance_addition = 0.8f;
    private bool skip_move = false;
    public bool broom_game = true;
    private GameObject BroomObject;

    public enum Facing // TODO: Move facing to enums.cs
    {
        Down,
        Up,
        Left,
        Right,
    }

    private UnityEngine.Vector2 target = UnityEngine.Vector2.zero;
    private UnityEngine.Vector2 curPos = UnityEngine.Vector2.zero;
    private UnityEngine.Vector2 prevPos = UnityEngine.Vector2.zero;

    private bool currentlyMoving = false;
    private int faceDirection = 0; // 0 - Down, 1 - Up, 2 - Left, 3 - Right

    private GameObject PlayerTarget;
    private PlayerCollision CollisionHandler;

    private void Awake()
    {
        input = new Inputs();
        PlayerTarget = GameObject.Find("PlayerTarget");
        PlayerTarget.transform.parent = null;
        CollisionHandler = PlayerTarget.GetComponent<PlayerCollision>();
    }

    #region InputSystem

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<UnityEngine.Vector2>();
        if (moveVector.x != 0f)
        {
            moveVector.x = moveVector.x / Mathf.Abs(moveVector.x); // Set magnitude to 1
        }
        if (moveVector.y != 0f)
        {
            moveVector.y = moveVector.y / Mathf.Abs(moveVector.y); // Set magnitude to 1
        }
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = UnityEngine.Vector2.zero;
    }

    public UnityEngine.Vector2 GetMoveVector()
    {
        return moveVector;
    }
    #endregion

    public void SetBroomGame(bool status)
    {
        broom_game = status;
        if (!status)
        {
            BroomObject.GetComponent<SpriteRenderer>().enabled = false;
            scan_distance_addition = 0.0f;
        }
        else
        {
            BroomObject.GetComponent<SpriteRenderer>().enabled = true;
            scan_distance_addition = 0.8f;
        }
    }
    void Start()
    {
        this.transform.position = new Vector3Int(GameManager.Instance.playerSpawnLocation.x, GameManager.Instance.playerSpawnLocation.y);
        curPos = transform.position;
        prevPos = transform.position;
        target = transform.position;
        faceDirection = GameManager.Instance.playerSpawnLocation.z;
        speed = DEFAULT_SPEED;
        PlayerTarget.transform.position = curPos;
        BroomObject = GameObject.Find("BroomObj");
    }

    public void AnchorPlayer(bool anchorState) // If anchorState == true then player will be locked in place
    {
        playerAnchored = anchorState;
    }

    public int GetFaceDirection() // Get direction player is facing.  0 - Down, 1 - Up, 2 - Left, 3 - Right
    {
        return faceDirection;
    }

    public void SetFaceDirection(int direction) // Set direction (from loading a save)
    {
        faceDirection = direction;
    }

    public UnityEngine.Vector2 GetPrevPos()
    {
        return prevPos;
    }

    public bool IsMoving() // return whether player is moving or not
    {
        return currentlyMoving;
    }

    private void UpdateLocation() // In charge of setting our new target location based on input
    {
        float yAxis = Input.GetAxisRaw("Vertical");
        float xAxis = Input.GetAxisRaw("Horizontal");


        if (yAxis == 0 && xAxis == 0)
            skip_move = false;
        if (skip_move)
            return;

        if (xAxis != 0 && !currentlyMoving) // X Axis Movement
        {
            currentlyMoving = true;
            PlayerTarget.transform.position = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y); // Move playerTarget to the target movement tile

            if (broom_game)
            {
                UnityEngine.Vector2 myVect = new UnityEngine.Vector2(xAxis, 0);
                bool gotCollision = CollisionHandler.checkMovable(myVect, 1.25f, transform.Find("BroomObj").transform.position); //;.position
                if (!gotCollision)
                    gotCollision = CollisionHandler.checkMovable(myVect, 1.25f, transform.position, (xAxis == 1 ? 3 : 2), faceDirection);
                Debug.Log(gotCollision);
                //if (CollisionHandler.checkMovable(new UnityEngine.Vector2(xAxis, 0), 1.5f, transform.position))
                // return;

                // if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(-xAxis, 0), 0.5f)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(xAxis, 0), 0.2f)) // Check if we can move in the specified direction
                if (faceDirection != 2 && faceDirection != 3 && gotCollision)
                {
                    faceDirection = xAxis == 1 ? 3 : 2;
                    skip_move = true;
                    return;
                }

                if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(-xAxis, 0), 0.1f)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(xAxis, 0), 0.1f)) // Check if we can move in the specified direction
                {
                    int newFaceDir = xAxis == 1 ? 3 : 2;
                    if (faceDirection != newFaceDir)
                    {
                        faceDirection = newFaceDir;
                        return;
                    }
                    faceDirection = newFaceDir;

                }
            }
            else
                faceDirection = xAxis == 1 ? 3 : 2;

            if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(-xAxis, 0), 0.75f + scan_distance_addition)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(xAxis, 0), 0.4f + scan_distance_addition)) // Check if we can move in the specified direction
            {
                target = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y);
                currentlyMoving = true;
                curPos = target;
                PlayerTarget.transform.position = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y); // Move playerTarget to the next tile, for interactions
            }
        }

        if (yAxis != 0 && !currentlyMoving) // Y Axis Movement
        {
            currentlyMoving = true;
            PlayerTarget.transform.position = new UnityEngine.Vector2(curPos.x, curPos.y + yAxis);

            if (broom_game)
            {
                UnityEngine.Vector2 myVect = new UnityEngine.Vector2(0, yAxis);
                bool gotCollision = CollisionHandler.checkMovable(myVect, 1.25f, transform.Find("BroomObj").transform.position); //transform.position);
                if (!gotCollision)
                    gotCollision = CollisionHandler.checkMovable(myVect, 1.25f, transform.position, (yAxis == 1 ? 1 : 0), faceDirection);
                //if (CollisionHandler.checkMovable(new UnityEngine.Vector2(0, yAxis), 1.5f, transform.position))
                //   return;

                // if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, -yAxis), 0.5f)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, yAxis), 0.5f)) // Check if we can move in the specified direction
                // faceDirection = yAxis == 1 ? 1 : 0;
                if (faceDirection != 1 && faceDirection != 0 && gotCollision)
                {
                    faceDirection = yAxis == 1 ? 1 : 0;
                    Debug.Log("SKIP!!!");
                    skip_move = true;
                    return;
                }

                if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, -yAxis), 0.1f)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, yAxis), 0.1f)) // Check if we can move in the specified direction
                {
                    int newFaceDir = yAxis == 1 ? 1 : 0;
                    if (faceDirection != newFaceDir)
                    {
                        faceDirection = newFaceDir;
                        return;
                    }
                    faceDirection = newFaceDir;
                }
            }
            else
                faceDirection = yAxis == 1 ? 1 : 0;

            if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, -yAxis), 0.6f + scan_distance_addition)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, yAxis), 0.4f + scan_distance_addition)) // Check if we can move in the specified direction
            {
                target = new UnityEngine.Vector2(curPos.x, curPos.y + yAxis);
                currentlyMoving = true;
                curPos = target;
                PlayerTarget.transform.position = new UnityEngine.Vector2(curPos.x, curPos.y + yAxis); // Move playerTarget to the next tile, for interactions
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

        if (currentlyMoving) {
            MoveSprite();
        }
        float yAxis = faceDirection == 1 ? 1 : (faceDirection == 0) ? -1 : 0;
        float xAxis = faceDirection == 3 ? 1 : (faceDirection == 2) ? -1 : 0;
        BroomObject.transform.position = new UnityEngine.Vector2(transform.position.x + xAxis, transform.position.y + yAxis);

    }
}