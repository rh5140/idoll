using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// References: 
// https://medium.com/nerd-for-tech/implementing-a-game-manager-using-the-singleton-pattern-unity-eb614b9b1a74
// https://bergstrand-niklas.medium.com/setting-up-a-simple-game-manager-in-unity-24b080e9516c
public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("Game Manager is NULL");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // Make sure only one Game Manager exists
        if (_instance)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        
        // Persist across scenes
        DontDestroyOnLoad(this);
    }
    #endregion

    // Call and change GameMode using Yarn Spinner functions
    // GameMode is a variable of enum type, defined in enums.cs
    public GameMode GameMode { get; set; }

    // Get story FSM..?
    public StoryBaseState StoryState { get; set; }

    // List all GameObjects

    // MAYBE Player + Inventory

    // MAYBE Background Music

    // Settings (volume, sfx, text speed)
}
