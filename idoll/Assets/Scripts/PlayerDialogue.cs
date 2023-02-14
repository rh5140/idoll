using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerDialogue : MonoBehaviour
{
    private bool isNear = false;
    [SerializeField] GameObject dialogueSystem;

    private void Update()
    {
        //To Do: add check for dialogue isActive
        if (isNear && Input.GetKeyDown("z") && !dialogueSystem.GetComponent<DialogueRunner>().Dialogue.IsActive)
        {
            Debug.Log("Interacted");
            dialogueSystem.GetComponent<DialogueRunner>().StartDialogue("Start");
        }
    }

    /* TODO: Replace these functions once Player Interaction is implemented. */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isNear = false;
    }
}
