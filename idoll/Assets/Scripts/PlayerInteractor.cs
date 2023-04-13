using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInteractor : MonoBehaviour
{
    private Interactable currentInteractable; // Only 1 interactable can be accessed at a time

    [Tooltip("False when your ability to interact with interactables is on a cooldown (For testing purposes)'")]
    [SerializeField]
    private bool canInteract = true;
    private float timer;
    private float interactDelay = 0.1f; // Add base 100ms delay between interacting. Can be changed as needed.

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

  
    void Update()
    {
        if (currentInteractable != null)
        {
            currentInteractable.OnHover();
            if (Input.GetButton("Interact") && canInteract)
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
