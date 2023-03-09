using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private Interactable currentInteractable; //Only 1 interactable can be accessed at a time
    public bool CanInteract = true;
    private float Timer = 0.1f; //Add base 100ms delay between interacting. Can be changed as needed.
    private float Duration = 0.1f; //

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
            if (Input.GetButton("Interact") && CanInteract)
            {
                currentInteractable.interact();
                CanInteract = false;
                Timer = Duration;
            }
        }

        if (Timer > 0)
        {
            Timer = Mathf.Max(0, Timer - Time.deltaTime);
            if (Timer == 0)
            {
                CanInteract = true;
            }
        }
    }
}
