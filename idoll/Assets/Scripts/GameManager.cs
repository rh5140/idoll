using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    #region Properties
    // Call and change GameMode using Yarn Spinner functions
    // GameMode is a variable of enum type, defined in enums.cs
    public GameMode GameMode { get; set; }

    // Get story state... also idk we need to maintain bools somewhere...
    public StoryState StoryState { get; set; }

    #endregion

    // Stores the x/y position and the facing direction of the player when switching scenes
    public Vector3Int playerSpawnLocation = new Vector3Int(0, 0, 0);

    // Public function to change scene can be called from anywhere w/ access to GameManager
    public void ChangeToScene(string sceneName, Vector2Int playerPos)
    {
        playerSpawnLocation = new Vector3Int(playerPos.x, playerPos.y, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().GetFaceDirection());
        SceneManager.LoadScene(sceneName);
    }

    // MAYBE Player + Inventory

    // MAYBE Background Music

    // Settings (volume, sfx, text speed)
}
