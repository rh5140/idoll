using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Tooltip("Drag in the Animator Component'")]
    [SerializeField]
    private Animator animator;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = this.transform.GetComponentInParent<PlayerMovement>();
    }

    private void SpriteAnimation()
    {
        Vector3 input = new Vector3(playerMovement.GetMoveVector().x, playerMovement.GetMoveVector().y);
        Vector2 movement = new Vector2(0f, -1f);
        int direction = playerMovement.GetFaceDirection();
        
        switch(direction)
        {
            case 0: // Down
                break;
            case 1: // Up
                movement = new Vector2(0f, 1f);
                break;
            case 2: // Left
                movement = new Vector2(-1f, 0f);
                break;
            case 3: // Right
                movement = new Vector2(1f, 0f);
                break;
            default:
                break;
        }

        //send value to anitatior parameters
        animator.SetFloat("xAxis", movement.x);
        animator.SetFloat("yAxis", movement.y);
        animator.SetFloat("magnitude", input.magnitude);
    }

    private void Update()
    {
        SpriteAnimation();
    }
}
