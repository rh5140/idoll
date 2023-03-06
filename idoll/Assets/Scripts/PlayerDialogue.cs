using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerDialogue : MonoBehaviour
{
    private bool isNear = false;
    [SerializeField] GameObject dialogueSystem;
    private static bool dialogue_is_active = false;

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

    [YarnCommand("ToggleMovement")]
    public static bool ToggleMovement()
    {
        dialogue_is_active = !dialogue_is_active;
        if (dialogue_is_active)
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
            return dialogue_is_active;
        }
        else
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
            return dialogue_is_active;
        }
    }
}
