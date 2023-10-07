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
    private float DEFAULT_SPEED = 4.0f; // TODO: Move to separate settings class
    public float DASH_MULTIPLIER = 1.5f;
    public bool isDashing = false;
    private float Turn_Time = 0.1f;
    private float MAX_TURN_TIME = 0.1f;

    private float speed;
    private float scan_distance_addition = 0f;
    private bool skip_move = false;
    public bool broom_game = false;
    [SerializeField] private GameObject BroomObject;

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
        input.Player.Navigate.performed += OnNavigatePerformed;
        input.Player.Navigate.canceled += OnNavigateCanceled;
        input.Player.QSave.performed += OnQSavePerformed;
        input.Player.QLoad.performed += OnQLoadPerformed;
        input.Player.Dash.performed += OnDashPerformed;
        input.Player.Dash.canceled += OnDashCanceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Navigate.performed -= OnNavigatePerformed;
        input.Player.Navigate.canceled -= OnNavigateCanceled;
        input.Player.QSave.performed -= OnQSavePerformed;
        input.Player.QLoad.performed -= OnQLoadPerformed;
        input.Player.Dash.performed -= OnDashPerformed;
        input.Player.Dash.canceled -= OnDashCanceled;
    }

    private void OnNavigatePerformed(InputAction.CallbackContext value)
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

    private void OnNavigateCanceled(InputAction.CallbackContext value)
    {
        moveVector = UnityEngine.Vector2.zero;
    }

    public UnityEngine.Vector2 GetMoveVector()
    {
        return moveVector;
    }

    private void OnQSavePerformed(InputAction.CallbackContext value)
    {
        GameManager.Instance.SaveGame();
    }

    private void OnQLoadPerformed(InputAction.CallbackContext value)
    {
        GameManager.Instance.LoadGame();
    }

    private void OnDashPerformed(InputAction.CallbackContext value)
    {
        isDashing = true;
    }

    private void OnDashCanceled(InputAction.CallbackContext value)
    {
        isDashing = false;
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
            scan_distance_addition = 1f;
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

          //  if (Turn_Time > 0)
           //     return;

            if (broom_game)
            {
                if (Turn_Time > 0)
                    return;


                UnityEngine.Vector2 myVect = new UnityEngine.Vector2(xAxis, 0);
                bool gotCollision = CollisionHandler.checkMovable(myVect, 1.25f, transform.Find("BroomObj").transform.position); //;.position
                if (!gotCollision)
                    gotCollision = CollisionHandler.checkMovable(myVect, 1.25f, transform.position, (xAxis == 1 ? 3 : 2), faceDirection);
                Debug.Log(gotCollision);
                //if (CollisionHandler.checkMovable(new UnityEngine.Vector2(xAxis, 0), 1.5f, transform.position))
                // return;
                int newFaceDir = xAxis == 1 ? 3 : 2;

                // if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(-xAxis, 0), 0.5f)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(xAxis, 0), 0.2f)) // Check if we can move in the specified direction
                if (faceDirection != newFaceDir && gotCollision)
                {
                    if (faceDirection != newFaceDir)
                    {
                        Turn_Time = MAX_TURN_TIME;
                    }
                    faceDirection = newFaceDir;
                    skip_move = true;
                    return;
                }

                if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(-xAxis, 0), 0.1f)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(xAxis, 0), 0.1f)) // Check if we can move in the specified direction
                {
                    if (faceDirection != newFaceDir)
                    {
                        Turn_Time = MAX_TURN_TIME;
                        faceDirection = newFaceDir;
                        return;
                    }
                    faceDirection = newFaceDir;

                }
            }
            else
            {
                int newFaceDir = xAxis == 1 ? 3 : 2;
              //  if (faceDirection != newFaceDir)
              //  {
             ////       Turn_Time = MAX_TURN_TIME;
             //       faceDirection = newFaceDir;
                //    return;
              //  }
                faceDirection = newFaceDir;
            }
           // faceDirection = xAxis == 1 ? 3 : 2;



            if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(-xAxis, 0), 0.45f + scan_distance_addition)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(xAxis, 0), 0.2f + scan_distance_addition)) // Check if we can move in the specified direction
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

            //if (Turn_Time > 0)
             //   return;

            if (broom_game)
            {
                if (Turn_Time > 0)
                    return;
                UnityEngine.Vector2 myVect = new UnityEngine.Vector2(0, yAxis);
                bool gotCollision = CollisionHandler.checkMovable(myVect, 1.25f, transform.Find("BroomObj").transform.position); //transform.position);
                if (!gotCollision)
                    gotCollision = CollisionHandler.checkMovable(myVect, 1.25f, transform.position, (yAxis == 1 ? 1 : 0), faceDirection);
                //if (CollisionHandler.checkMovable(new UnityEngine.Vector2(0, yAxis), 1.5f, transform.position))
                //   return;

                // if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, -yAxis), 0.5f)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, yAxis), 0.5f)) // Check if we can move in the specified direction
                // faceDirection = yAxis == 1 ? 1 : 0;
                int newFaceDir = yAxis == 1 ? 1 : 0;

                if (faceDirection != newFaceDir && gotCollision)
                {
                    if (faceDirection != newFaceDir)
                    {
                        Turn_Time = MAX_TURN_TIME;
                    }
                    faceDirection = yAxis == 1 ? 1 : 0;
                    Debug.Log("SKIP!!!");
                    skip_move = true;
                    return;
                }

                if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, -yAxis), 0.1f)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, yAxis), 0.1f)) // Check if we can move in the specified direction
                {
                    if (faceDirection != newFaceDir)
                    {
                        Turn_Time = MAX_TURN_TIME;
                        faceDirection = newFaceDir;
                        return;
                    }
                    faceDirection = newFaceDir;
                }
            }
            else
            {
                int newFaceDir = yAxis == 1 ? 1 : 0;
                //if (faceDirection != newFaceDir)
                //{
                   // Turn_Time = MAX_TURN_TIME;
                  //  faceDirection = newFaceDir;
                 //   return;
               // }
                faceDirection = newFaceDir;
            }

            if ((!CollisionHandler.GetCollision() || !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, -yAxis), 0.45f + scan_distance_addition)) && !CollisionHandler.CheckCollision(new UnityEngine.Vector2(0, yAxis), 0.2f + scan_distance_addition)) // Check if we can move in the specified direction
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
        if (Input.GetKeyDown("6"))
        {
            if (!broom_game) { SetBroomGame(true); }
            else { SetBroomGame(false); }
        }

        if (isDashing)
            speed = DEFAULT_SPEED * DASH_MULTIPLIER;
        else
            speed = DEFAULT_SPEED;

        if (Turn_Time > 0)
            Turn_Time -= Time.deltaTime;
        if (!playerAnchored && !currentlyMoving) // && Turn_Time <= 0
            UpdateLocation();

        if (currentlyMoving) {
            MoveSprite();
        }
        float yAxis = faceDirection == 1 ? 1 : (faceDirection == 0) ? -1 : 0;
        float xAxis = faceDirection == 3 ? 1 : (faceDirection == 2) ? -1 : 0;
        try
        {
            BroomObject.transform.position = new UnityEngine.Vector2(transform.position.x + xAxis, transform.position.y + yAxis);
        }
        catch
        {
            Debug.Log("No broom object found!");
        }

    }
}