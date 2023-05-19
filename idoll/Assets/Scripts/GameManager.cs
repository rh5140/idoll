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

        // Check if scene dictionary exists, if not, create scene dictionary
        
        // Persist across scenes
        DontDestroyOnLoad(this);
    }
    #endregion

    #region Properties
    // Properties are Capitalized

    // Call and change GameMode using Yarn Spinner functions
    // GameMode is a variable of enum type, defined in enums.cs
    public GameMode GameMode { get; set; }

    // Get story state... also idk we need to maintain bools somewhere...
    public StoryState StoryState { get; set; }

    // Store the eyeball
    public int CurrentEye { get; set; } = 1;

    // Current scene
    public string CurrentScene {get; set; }

    // Is the companion following you?
    public bool CompanionFollow { get; set;}
    
    // Player coordinates
    public int PlayerPositionX {get; set;}
    public int PlayerPositionY {get; set;}

    // Player facing direction
    public int PlayerFacing {get; set;}

    //
    public bool LoadedSave {get; set;} = false;
 
    #endregion
    
    // Game Manager Dictionary: key = scene name, value = scene-specific dictionary
    // scene-specific dictionary: key = object name, value = bool
    // nested dictionary might have weird things going on be sure to check it works
    public Dictionary<string, Dictionary<string, bool>> progressDict = new Dictionary<string, Dictionary<string, bool>>();

    private Inventory inventory; // Temporary inventory code for the winter showcase
    private bool inventoryCooldown = false;
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        inventory.gameObject.transform.parent.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown("e")) // Oh wait why am I using a Coroutine? I forgot GetKeyDown only triggers on the 'down' press haha
        {
            StartCoroutine(ToggleInventory());
        }
        
        if (Input.GetKeyDown("o"))
        {
            SaveGame();
        }
        
        if (Input.GetKeyDown("p"))
        {
            LoadGame();
        }
    }
    private IEnumerator ToggleInventory()
    {
        if (!inventoryCooldown)
        {
            inventoryCooldown = true;
            inventory.gameObject.transform.parent.gameObject.SetActive(!inventory.gameObject.transform.parent.gameObject.activeSelf);
            yield return new WaitForSeconds(0.3f); // Prevent multi-presses
            inventoryCooldown = false;
        }
    }

    // Stores the x/y position and the facing direction of the player when switching scenes
    public Vector3Int playerSpawnLocation = new Vector3Int(0, 0, 0);
       
    // Public function to change scene can be called from anywhere w/ access to GameManager
    public void ChangeToScene(string sceneName, Vector2Int playerPos)
    {
        playerSpawnLocation = new Vector3Int(playerPos.x, playerPos.y, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().GetFaceDirection());
        SceneManager.LoadScene(sceneName);
        CurrentScene = sceneName;
    }

    public void GetPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2Int playerPos = new Vector2Int((int) player.transform.position.x, (int) player.transform.position.y);
        PlayerPositionX = playerPos[0];
        PlayerPositionY = playerPos[1];

        int playerFacing = player.GetComponent<PlayerMovement>().GetFaceDirection();
        PlayerFacing = playerFacing;
    }

    // Prayge Save System works
    public void SaveGame()
    {
        Debug.Log("Saved game!");
        SaveSystem.SaveGame();
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();

        StoryState = (StoryState) data.storyState;
        CurrentEye = data.currentEye;
        CurrentScene = data.currentScene;
        CompanionFollow = data.companionFollow;

        PlayerPositionX = data.position[0];
        PlayerPositionY = data.position[1];
        PlayerFacing = data.direction;

        playerSpawnLocation = new Vector3Int(PlayerPositionX, PlayerPositionY, PlayerFacing);

        // Need to load scene

        LoadedSave = true;
        SceneManager.LoadScene(CurrentScene);

        // Execution order problem -- wait for scene to fully load before the below
        // GameObject player = GameObject.FindGameObjectWithTag("Player");
        // player.transform.position = new Vector2(PlayerPositionX, PlayerPositionY);
        // player.GetComponent<PlayerMovement>().SetFaceDirection(PlayerFacing);
    }

    // MAYBE Player + Inventory

    // MAYBE Background Music

    // Settings (volume, sfx, text speed)
}
