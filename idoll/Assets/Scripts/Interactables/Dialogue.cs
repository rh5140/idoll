using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Dialogue : Interactable
{
    [SerializeField] GameObject dialogueSystem;
    private static bool dialogue_is_active = false;

    protected override void interact()
    {
        //To Do: add check for dialogue isActive
        if (!dialogueSystem.GetComponent<DialogueRunner>().Dialogue.IsActive)
        {
            Debug.Log("Interacted");
            dialogueSystem.GetComponent<DialogueRunner>().StartDialogue("Start");
        }
    }

    [YarnCommand("ToggleMovement")]
    public static bool ToggleMovement()
    {
        dialogue_is_active = !dialogue_is_active;
        if (dialogue_is_active)
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
            GameObject.Find("PlayerTarget").GetComponent<PlayerInteractor>().enabled = false;
            return dialogue_is_active;
        }
        else
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
            GameObject.Find("PlayerTarget").GetComponent<PlayerInteractor>().enabled = true;
            return dialogue_is_active;
        }
    }
}
