using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private Interactable currentInteractable; //Only 1 interactable can be accessed at a time

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
            if (Input.GetButton("Interact"))
            {
                currentInteractable.OnInteract();
            }
        }
    }
}
