using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Dialogue : Interactable
{
    private GameObject dialogueSystem;
    private static bool dialogue_is_active = false;

    private void Start() // Automatically link up with other game objects when loading the scene
    {
        try
        {
            dialogueSystem = GameObject.FindGameObjectWithTag("DialogueSystem");
        }
        catch
        {
            Debug.Log("Please add a Dialogue System to the scene!");
        }
    }

    public override void OnInteract()
    {
        if (!dialogueSystem.GetComponent<DialogueRunner>().Dialogue.IsActive)
        {
            try
            {
                dialogueSystem.GetComponent<DialogueRunner>().StartDialogue(transform.name);
            }
            catch
            {
                Debug.Log("No dialogue available");
            }
        }
    }

    [YarnCommand("ToggleMovement")]
    public static bool ToggleMovexment() // TODO: Handle enabling/disabling player with GameManager 
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
