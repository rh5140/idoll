using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem; // New Unity input system!

public class PlayerInteractor : MonoBehaviour
{
    private Interactable currentInteractable; // Only 1 interactable can be accessed at a time

    [Tooltip("False when your ability to interact with interactables is on a cooldown (Displayed for testing purposes)'")]
    [SerializeField]
    private bool canInteract = true;
    private float timer;
    private float interactDelay = 0.1f; // Add base 100ms delay between interacting. Can be changed as needed.

    private Inputs input;

    #region InputSystem

    private void Awake()
    {
        input = new Inputs();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Primary.performed += OnInteract;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Primary.performed -= OnInteract;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        Interactable obj = other.GetComponentInParent<Interactable>();
        if (obj != null)
        {
            currentInteractable = obj;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Interactable obj = other.GetComponentInParent<Interactable>();
        if (obj != null)
        {
            if (obj == currentInteractable)
            {
                currentInteractable = null;
            }
            
        }
    }
  
    // This function runs when the 'primary interact' key is pressed
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (currentInteractable == null)
        {
            return;
        }

        if (canInteract)
        {
            if ((currentInteractable.GetComponent<SpriteRenderer>() != null && currentInteractable.GetComponent<SpriteRenderer>().enabled)
                || (currentInteractable.GetComponent<TilemapRenderer>() != null && currentInteractable.GetComponent<TilemapRenderer>().enabled))
            {
                currentInteractable.interact();
            }
            canInteract = false;
            timer = interactDelay;
        }
    }

    // Decreases the timer between interactions
    private void Update()
    {
        if (currentInteractable != null) // Check if the target is hovering over a tile
        {
            currentInteractable.OnHover();
        }

        if (timer > 0)
        {
            timer = Mathf.Max(0, timer - Time.deltaTime);
            if (timer == 0)
            {
                canInteract = true;
            }
        }
    }
}