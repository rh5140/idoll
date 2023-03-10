using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactDelay = 0.5f; //Time in seconds before it can be interacted with again
    public bool useable = true;
    public bool reusable = true;

    public float timer = 0;

    public virtual void OnInteract()
    {
        
        Debug.Log("Interacted!");
    }

    public virtual void OnHover() //Triggered when the player presses the Interact key
    {

    }

    public virtual void StartTimer()
    {
        useable = false;
        timer = interactDelay;
    }

    public virtual void interact() //Triggered when the player presses the Interact key
    {
        if (useable)
        {
            StartTimer();
            OnInteract();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (reusable)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = 0;
                    useable = true;
                }
            }
        }
    }
}
