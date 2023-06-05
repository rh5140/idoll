using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Dialogue : Interactable
{
    private GameObject dialogueSystem;
    //private string modeBeforeDialogue;
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

    private void Update()
    {
        // Since the Update() in Interactable is being overriden, I copy-pasted this. There's definitely a better way to do this - Alexander
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

        if (dialogue_is_active != dialogueSystem.GetComponent<DialogueRunner>().Dialogue.IsActive)
        {
            if (dialogue_is_active == false) // If dialogue is being enabled
            {
                dialogue_is_active = true;
                GameManager.Instance.SetGameMode("dialogue");
            }
            else
            {
                dialogue_is_active = false;
                GameManager.Instance.SetGameMode("gameplay");
            }
        }
    }

    public override void OnInteract()
    {
        if (this.tag == "Companion")
        {
            this.GetComponent<Companion>().FacePlayer();
        }

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


    [YarnCommand("ToggleCompanionFollow")]
    public static void ToggleCompanionFollow(string s = "toggle")
    {
        Companion companion = GameObject.FindGameObjectWithTag("Companion").GetComponent<Companion>();
        companion.ToggleFollowPlayer(s);
    }

    /*
    [YarnCommand("ToggleMovement")]
    public static bool ToggleMovement(string newGameMode = "default")
    {
        if (newGameMode == "default")
        {
            dialogue_is_active = !dialogue_is_active;
            if (dialogue_is_active)
            {
                GameManager.Instance.SetGameMode("dialogue");
            }
            else
            {
                GameManager.Instance.SetGameMode("gameplay");
            }
        }
        else
        {
            GameManager.Instance.SetGameMode(newGameMode);
            if (GameManager.Instance.GameMode == "dialogue")
            {
                dialogue_is_active = true;
            }
            else
            {
                dialogue_is_active = false;
            }
        }
        return dialogue_is_active;
    }
    */
}
