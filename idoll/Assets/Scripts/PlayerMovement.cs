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
        input.Player.Movement.canceled += OnMovementCanceled;
        //input.Player.Secondary.performed += OnSecondaryPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;
        //input.Player.Secondary.performed -= OnSecondaryPerformed;
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

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = UnityEngine.Vector2.zero;
    }

    public UnityEngine.Vector2 GetMoveVector()
    {
        return moveVector;
    }

    /*
    private void OnSecondaryPerformed(InputAction.CallbackContext value)
    {
        if (GameManager.Instance.GameMode == "gameplay" || GameManager.Instance.GameMode == "")
        {
            GameManager.Instance.ToggleMenu();
        }
    }
    */

    #endregion

    void Start()
    {
        this.transform.position = new Vector3Int(GameManager.Instance.playerSpawnLocation.x, GameManager.Instance.playerSpawnLocation.y);
        curPos = transform.position;
        prevPos = transform.position;
        target = transform.position;
        faceDirection = GameManager.Instance.playerSpawnLocation.z;
        speed = DEFAULT_SPEED;
        PlayerTarget.transform.position = curPos;
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
        //float yAxis = Input.GetAxisRaw("Vertical"); // Old Unity movement
        //float xAxis = Input.GetAxisRaw("Horizontal");

        float xAxis = moveVector.x;
        float yAxis = moveVector.y;

        if (xAxis != 0 && !currentlyMoving) // X Axis Movement
        {
            currentlyMoving = true;
            PlayerTarget.transform.position = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y); // Move playerTarget to the target movement tile

            faceDirection = xAxis == 1 ? 3 : 2;

            if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(-xAxis, 0), 0.45f)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(xAxis, 0), 0.2f)) // Check if we can move in the specified direction
            {
                target = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y);
                currentlyMoving = true;
                prevPos = curPos;
                curPos = target;
                PlayerTarget.transform.position = new UnityEngine.Vector2(curPos.x + xAxis, curPos.y); // Move playerTarget to the next tile, for interactions
            }
        }

        if (yAxis != 0 && !currentlyMoving) // Y Axis Movement
        {
            currentlyMoving = true;
            PlayerTarget.transform.position = new UnityEngine.Vector2(curPos.x, curPos.y + yAxis);

            faceDirection = yAxis == 1 ? 1 : 0;

            if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, -yAxis), 0.45f)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, yAxis), 0.2f)) // Check if we can move in the specified direction
            {
                target = new UnityEngine.Vector2(curPos.x, curPos.y + yAxis);
                currentlyMoving = true;
                prevPos = curPos;
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
    }
}