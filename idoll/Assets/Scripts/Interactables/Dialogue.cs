using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Dialogue : Interactable
{
    private GameObject dialogueSystem;
    //private string modeBeforeDialogue;
    private static bool dialogue_is_active = false;
    [SerializeField] private bool autoInteractWhenSceneLoads = false;

    protected override void Start() // Automatically link up with other game objects when loading the scene
    {
        try
        {
            dialogueSystem = GameObject.FindGameObjectWithTag("DialogueSystem");
        }
        catch
        {
            Debug.Log("Please add a Dialogue System to the scene!");
        }

        if (autoInteractWhenSceneLoads)
        {
            interact();
        }
    }

    protected override void Update()
    {
        // Since the Update() in Interactable is being overriden, I copy-pasted this. There's definitely a better way to do this - Alexander
        base.Update();

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

    [YarnCommand("TeleportSprite")]
    public static void TeleportSprite(string gameObjectName, int x, int y)
    {
        GameObject gobj = GameObject.Find(gameObjectName);
        gobj.transform.localPosition = new Vector3(x, y);
    }

    [YarnCommand("NextAct")]
    public static void NextAct()
    {
        GameManager.Instance.NextStoryAct();
    }

    [YarnCommand("NextScene")]
    public static void NextScene()
    {
        GameManager.Instance.NextStoryScene();
    }

    [YarnCommand("NextSubscene")]
    public static void NextSubscene()
    {
        GameManager.Instance.NextStorySubscene();
    }

    [YarnCommand("SaveGame")]
    public static void SaveFromDialogue()
    {
        GameManager.Instance.SaveGame();
    }

    [YarnFunction("story_is")] // Returns true if the current Act/Scene/Subscene is exactly the same
    public static bool IsStory(int act, int scene, int sub)
    {
        return GameManager.Instance.StoryState == new Vector3Int(act, scene, sub);
    }

    [YarnFunction("story_between")] // Returns true if the current Act/Scene/Subscene is between a range
    public static bool BetweenStory(int act1, int scene1, int sub1, int act2, int scene2, int sub2)
    {
        Vector3Int enableVector = GameManager.Instance.StoryState - new Vector3Int(act1, scene1, sub1);
        Vector3Int disableVector = new Vector3Int(act2, scene2, sub2) - GameManager.Instance.StoryState;

        if (enableVector.x < 0 ||
           (enableVector.x == 0 && enableVector.y < 0) ||
           (enableVector.x == 0 && enableVector.y == 0 && enableVector.z < 0))
            return false;

        if (disableVector != Vector3Int.zero &&
            disableVector.x < 0 ||
           (disableVector.x == 0 && disableVector.y < 0) ||
           (disableVector.x == 0 && disableVector.y == 0 && disableVector.z < 0))
            return false;

        return true;
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
