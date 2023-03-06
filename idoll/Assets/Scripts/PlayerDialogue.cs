using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerDialogue : MonoBehaviour
{
    private GameObject target = null;
    [SerializeField] GameObject dialogueSystem;
    private static bool dialogue_is_active = false;

    private void Update()
    {
        //To Do: add check for dialogue isActive
        if (target && Input.GetKeyDown("z") && !dialogueSystem.GetComponent<DialogueRunner>().Dialogue.IsActive)
        {
            Debug.Log("Interacted");
            try
            {
                dialogueSystem.GetComponent<DialogueRunner>().StartDialogue(target.name);
            }
            catch
            {
                Debug.Log("No dialogue available");
            }
        }
    }

    /* TODO: Replace these functions once Player Interaction is implemented. */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        target = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        target = null;
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
