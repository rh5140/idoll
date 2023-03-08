using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Dialogue : Interactable
{
    [SerializeField] GameObject dialogueSystem;
    private static bool dialogue_is_active = false;

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
    public static bool ToggleMovexment()
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
