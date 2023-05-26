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
    // Call and change GameMode using Yarn Spinner functions
    public string gameMode { get; set; }

    // Get story state... also idk we need to maintain bools somewhere...
    public Vector3Int storyState { get; set; } = new Vector3Int(0,0,0); // (Act, Scene, Subscene)

    [SerializeField]
    private Vector3Int actSceneSubscene;

    // Store the eyeball
    public int currentEye { get; set; } = 1;

    // Current scene
    public string currentScene { get; set; }

    // Is the companion following you?
    public bool companionFollow { get; set;}
 
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

        // Jump to scene using the Unity Inspector
        if (actSceneSubscene != storyState) // If the player manually changes the scene
        {
            storyState = actSceneSubscene;
            // Update act/scene logic!
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
        currentScene = sceneName;
    }

    public void NextStoryAct()
    {
        storyState += new Vector3Int(1, 0, 0);
        actSceneSubscene = storyState;
        // Update act/scene logic!
    }
    public void NextStoryScene()
    {
        storyState += new Vector3Int(0, 1, 0);
        actSceneSubscene = storyState;
        // Update act/scene logic!
    }
    public void NextStorySubscene()
    {
        storyState += new Vector3Int(0, 0, 1);
        actSceneSubscene = storyState;
        // Update act/scene logic!
    }

    public void SetGameMode(string gm)
    {
        switch(gm)
        {
            case "dialogue":
                GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
                GameObject.Find("PlayerTarget").GetComponent<PlayerInteractor>().enabled = false;
                break;
            case "gameplay":
            case "":
                GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
                GameObject.Find("PlayerTarget").GetComponent<PlayerInteractor>().enabled = true;
                break;
            case "menu":
                GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
                GameObject.Find("PlayerTarget").GetComponent<PlayerInteractor>().enabled = false;
                break;
            case "chase":
                GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
                GameObject.Find("PlayerTarget").GetComponent<PlayerInteractor>().enabled = true;
                break;
            default:
                Debug.Log("Invalid GameMode!");
                break;
        }
    }


    // MAYBE Player + Inventory

    // MAYBE Background Music

    // Settings (volume, sfx, text speed)
}
