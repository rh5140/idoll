using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Animator animator;

    [SerializeField] private float walk_time = 5.0f; // TODO - match moveSpeed with player
    private Vector2 destinationTile;
    private Vector2 spawnLocation; // tile where the Companion is originally placed
    private bool currentlyMoving;
    private int faceDirection; // 0 - Down, 1 - Up, 2 - Left, 3 - Right

    // Implementation of ghosting
    private bool is_ghosted = false;
    private bool facing_player = false;
    private bool timer_running = false;
    private float time_remaining;
    private float collision_timer_length = 0.2f;

    void Start()
    {
        spawnLocation = this.transform.position; // Store the original location of the companion in this scene

        // Finding the player GameObject
        if (player == null)
        {
            try
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            catch
            {
                Debug.Log("Companion could not find either 'Player' or 'PlayerTarget'!");
            }
        }

        if (!GameManager.Instance.CompanionFollow) return; // Don't follow the player if CompanionFollow == false

        // Spawn location in the scene
        faceDirection = GameManager.Instance.playerSpawnLocation.z;
        this.transform.position = new Vector3Int(GameManager.Instance.playerSpawnLocation.x, GameManager.Instance.playerSpawnLocation.y);
        switch (faceDirection)
        {
            case 0: // Down
                //this.transform.position = transform.position + new Vector3Int(0, 1);
                animator.SetFloat("xAxis", 0f);
                animator.SetFloat("yAxis", -1f);
                break;
            case 1: // Up
                //this.transform.position = transform.position + new Vector3Int(0, -1);
                animator.SetFloat("xAxis", 0f);
                animator.SetFloat("yAxis", 1f);
                break;
            case 2: // Left
                //this.transform.position = transform.position + new Vector3Int(1, 0);
                animator.SetFloat("xAxis", -1f);
                animator.SetFloat("yAxis", 0f);
                break;
            case 3: // Right
                //this.transform.position = transform.position + new Vector3Int(-1, 0);
                animator.SetFloat("xAxis", 1f);
                animator.SetFloat("yAxis", 0f);
                break;
        }
    }

    void Update()
    {
        if (!GameManager.Instance.CompanionFollow) // Don't follow the player if CompanionFollow == false
        {
            //this.transform.position = spawnLocation;
            return;
        }

        // Set the player's previous position as the destination
        destinationTile = player.GetComponent<PlayerMovement>().GetPrevPos();

        // Move the companion if the player moves
        if (player.GetComponent<PlayerMovement>().IsMoving())
        {
            currentlyMoving = true;
        }

        // Check if player is trying to walk through companion
        int playerDirection = player.GetComponent<PlayerMovement>().GetFaceDirection();
        if ((playerDirection == 0 && player.transform.position.y > this.transform.position.y) ||
            (playerDirection == 1 && player.transform.position.y < this.transform.position.y) ||
            (playerDirection == 2 && player.transform.position.x > this.transform.position.x) ||
            (playerDirection == 3 && player.transform.position.x < this.transform.position.x))
        {
            if (facing_player == false)
            {
                // Start timer to ghost the companion
                timer_running = true;
                time_remaining = collision_timer_length;
            }
            facing_player = true;
        }
        // Once the companion and player stop facing one another, disable ghosting
        else
        {
            facing_player = false;
            is_ghosted = false;
        }

        // Ghosting timer
        if (timer_running)
        {
            if (time_remaining > 0)
            {
                time_remaining -= Time.deltaTime;
            }
            else
            {
                timer_running = false;
                time_remaining = 0;
                is_ghosted = true;
            }
        }

        // If companion is moving, glide to the destinationTile
        if (currentlyMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinationTile, walk_time * Time.deltaTime);

            // Animations
            if (transform.position.y > destinationTile.y) // Down
            {
                faceDirection = 0;
                animator.SetFloat("xAxis", 0f);
                animator.SetFloat("yAxis", -1f);
                animator.SetFloat("magnitude", 1);
            }
            else if (transform.position.y < destinationTile.y) // Up
            {
                faceDirection = 1;
                animator.SetFloat("xAxis", 0f);
                animator.SetFloat("yAxis", 1f);
                animator.SetFloat("magnitude", 1);
            }
            else if (transform.position.x > destinationTile.x) // Left
            {
                faceDirection = 2;
                animator.SetFloat("xAxis", -1f);
                animator.SetFloat("yAxis", 0f);
                animator.SetFloat("magnitude", 1);
            }
            else if (transform.position.x < destinationTile.x) // Right
            {
                faceDirection = 3;
                animator.SetFloat("xAxis", 1f);
                animator.SetFloat("yAxis", 0f);
                animator.SetFloat("magnitude", 1);
            }

            // Stop moving when destination has been reached
            else if ((transform.position.x == destinationTile.x) && (transform.position.y == destinationTile.y))
            {
                currentlyMoving = false;
                animator.SetFloat("magnitude", 0);
            }
        }
    }

    public void FacePlayer()
    {
        switch (player.GetComponent<PlayerMovement>().GetFaceDirection())
        {
            case 0: // Down
                animator.SetFloat("xAxis", 0f);
                animator.SetFloat("yAxis", 1f);
                break;
            case 1: // Up
                animator.SetFloat("xAxis", 0f);
                animator.SetFloat("yAxis", -1f);
                break;
            case 2: // Left
                animator.SetFloat("xAxis", 1f);
                animator.SetFloat("yAxis", 0f);
                break;
            case 3: // Right
                animator.SetFloat("xAxis", -1f);
                animator.SetFloat("yAxis", 0f);
                break;
        }
    }

    public bool IsGhosted()
    {
        return is_ghosted;
    }

    public void ToggleFollowPlayer(string s = "toggle")
    {
        if (s == "off")
        {
            GameManager.Instance.CompanionFollow = false;
        }
        else if (s == "on")
        {
            GameManager.Instance.CompanionFollow = true;
        }
        else
        {
            GameManager.Instance.CompanionFollow = !GameManager.Instance.CompanionFollow;
        }

        if (GameManager.Instance.CompanionFollow) // Update position when following is switched on
        {
            this.transform.position = player.GetComponent<PlayerMovement>().GetPrevPos();
        }
    }

    //private IEnumerator MoveHelper(Vector2 destination, float seconds)
    //{
    //    is_ghosted = false;
    //    facing_player = false;
    //    float elapsedTime = 0;
    //    Vector2 startingPos = this.transform.position;
    //    while (elapsedTime < seconds)
    //    {
    //        this.transform.position = Vector2.Lerp(startingPos, destination, (elapsedTime / seconds));
    //        elapsedTime += Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //    }
    //    this.transform.position = destination;
        
    //    int player_direction = player.GetComponent<PlayerMovement>().GetFaceDirection();
    //    Vector2 movement = new Vector2(0f, -1f);
    //    switch (player_direction)
    //    {
    //        case 0: // Down
    //            break;
    //        case 1: // Up
    //            movement = new Vector2(0f, 1f);
    //            break;
    //        case 2: // Left
    //            movement = new Vector2(-1f, 0f);
    //            break;
    //        case 3: // Right
    //            movement = new Vector2(1f, 0f);
    //            break;
    //        default:
    //            break;
    //    }

    //    //send value to animator parameters
    //    animator.SetFloat("xAxis", movement.x);
    //    animator.SetFloat("yAxis", movement.y);
    //    animator.SetFloat("magnitude", 1);
    //}
}
