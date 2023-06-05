using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactDelay = 0.5f; //Time in seconds before it can be interacted with again
    public bool useable = true;
    public bool reusable = true;
    public int activated = 0;
    public Vector3Int enableInAct; // Define the first act/scene/subscene where the interactable shows up
    public Vector3Int disableAfterAct; // Define the last act/scene/subscene where the interactable shows up

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
        // Only enable the interactable for a range of acts/scenes/subscenes
        Vector3Int enableVector = GameManager.Instance.StoryState - enableInAct;
        if (enableVector.x < 0 ||
           (enableVector.x == 0 && enableVector.y < 0) ||
           (enableVector.x == 0 && enableVector.y == 0 && enableVector.z < 0))
        {
            return;
        }
        Vector3Int disableVector = disableAfterAct - GameManager.Instance.StoryState;
        if (disableVector != Vector3Int.zero &&
            disableVector.x < 0 ||
           (disableVector.x == 0 && disableVector.y < 0) ||
           (disableVector.x == 0 && disableVector.y == 0 && disableVector.z < 0))
        {
            return;
        }

        if (useable)
        {
            StartTimer();
            OnInteract();
            activate();
        }
    }

    public virtual void activate()
    {
        activated += 1;
        string sceneName = GameManager.Instance.CurrentScene;
        string objectName = gameObject.name;

        GameManager.Instance.progressDict[sceneName][objectName] = true;
    }

    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
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
