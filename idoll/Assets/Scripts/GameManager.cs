using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public string GameMode { get; set; }

    // Get story state... also idk we need to maintain bools somewhere...
    public Vector3Int StoryState { get; set; } = new Vector3Int(0,0,0); // (Act, Scene, Subscene)

    // Store the eyeball
    public int CurrentEye { get; set; } = 1;

    // Current scene
    public string CurrentScene { get; set; }

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

    #region InspectorVariables

    [SerializeField]
    private Vector3Int actSceneSubscene;

    // Stores the x/y position and the facing direction of the player when switching scenes
    [SerializeField]
    public Vector3Int playerSpawnLocation = new Vector3Int(0, 0, 0);

    [SerializeField]
    public GameObject globalLight;

    [SerializeField]
    public TMP_Text locationText;

    [SerializeField]
    public Image blackScreen;
    private bool isChangingScene;

    #endregion

    // Game Manager Dictionary: key = scene name, value = scene-specific dictionary
    // scene-specific dictionary: key = object name, value = bool
    // nested dictionary might have weird things going on be sure to check it works
    public Dictionary<string, Dictionary<string, bool>> progressDict = new Dictionary<string, Dictionary<string, bool>>();

    private MainMenu mainMenu;
    private void Start()
    {
        mainMenu = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenu>();
        locationText.text = System.Text.RegularExpressions.Regex.Replace(SceneManager.GetActiveScene().name, "([a-z])([A-Z])", "$1 $2"); /* https://stackoverflow.com/questions/272633/add-spaces-before-capital-letters */
        blackScreen.gameObject.SetActive(true);
        blackScreen.color = new Color(255, 255, 255, 0);
        globalLight.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            SaveGame();
        }

        if (Input.GetKeyDown("p"))
        {
            LoadGame();
        }

        // Jump to scene using the Unity Inspector
        if (actSceneSubscene != StoryState) // If the player manually changes the scene
        {
            StoryState = actSceneSubscene;
            // Update act/scene logic!
        }
    }

    //private IEnumerator ToggleInventory()
    //{
    //    if (!inventoryCooldown)
    //    {
    //        inventoryCooldown = true;
    //        inventory.gameObject.transform.parent.gameObject.SetActive(!inventory.gameObject.transform.parent.gameObject.activeSelf);
    //        yield return new WaitForSeconds(0.3f); // Prevent multi-presses
    //        inventoryCooldown = false;
    //    }
    //}
   
    public void GetPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2Int playerPos = new Vector2Int((int) player.transform.position.x, (int) player.transform.position.y);
        PlayerPositionX = playerPos[0];
        PlayerPositionY = playerPos[1];
        int playerFacing = player.GetComponent<PlayerMovement>().GetFaceDirection();
        PlayerFacing = playerFacing;
    }

    public void NextStoryAct()
    {
        StoryState += new Vector3Int(1, 0, 0);
        actSceneSubscene = StoryState;
        // Update act/scene logic!
    }
    public void NextStoryScene()
    {
        StoryState += new Vector3Int(0, 1, 0);
        actSceneSubscene = StoryState;
        // Update act/scene logic!
    }
    public void NextStorySubscene()
    {
        StoryState += new Vector3Int(0, 0, 1);
        actSceneSubscene = StoryState;
        // Update act/scene logic!
    }

    public void SetGameMode(string gm)
    {
        switch(gm)
        {
            case "dialogue":
                GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
                GameObject.Find("PlayerTarget").GetComponent<PlayerInteractor>().enabled = false;
                GameMode = "dialogue";
                break;
            case "gameplay":
            case "":
                GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
                GameObject.Find("PlayerTarget").GetComponent<PlayerInteractor>().enabled = true;
                GameMode = "gameplay";
                break;
            case "menu":
                GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
                GameObject.Find("PlayerTarget").GetComponent<PlayerInteractor>().enabled = false;
                GameMode = "menu";
                break;
            case "chase":
                GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
                GameObject.Find("PlayerTarget").GetComponent<PlayerInteractor>().enabled = true;
                GameMode = "chase";
                break;
            default:
                Debug.Log("Invalid GameMode!");
                break;
        }
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

        StoryState = new Vector3Int(data.storyState[0], data.storyState[1], data.storyState[2]);
        CurrentEye = data.currentEye;
        CurrentScene = data.currentScene;
        CompanionFollow = data.companionFollow;

        PlayerPositionX = data.position[0];
        PlayerPositionY = data.position[1];
        PlayerFacing = data.direction;

        playerSpawnLocation = new Vector3Int(PlayerPositionX, PlayerPositionY, PlayerFacing);

        // Need to load scene

        LoadedSave = true;
        ChangeToScene(CurrentScene, new Vector2Int(PlayerPositionX, PlayerPositionY), false);

        // Execution order problem -- wait for scene to fully load before the below
        // GameObject player = GameObject.FindGameObjectWithTag("Player");
        // player.transform.position = new Vector2(PlayerPositionX, PlayerPositionY);
        // player.GetComponent<PlayerMovement>().SetFaceDirection(PlayerFacing);
    }

    // MAYBE Player + Inventory
    public void ToggleMenu()
    {
        if (GameMode == "menu")
        {
            SetGameMode("gameplay");
            mainMenu.SlideMenu("off");
        }
        else if (GameMode == "gameplay" || GameMode == "")
        {
            SetGameMode("menu");
            mainMenu.SlideMenu("on");
        }
    }

    // MAYBE Background Music

    // Settings (volume, sfx, text speed)

    // Public function to change scene can be called from anywhere w/ access to GameManager
    public void ChangeToScene(string sceneName, Vector2Int playerPos, bool fade = true)
    {
        if (isChangingScene) return;
        playerSpawnLocation = new Vector3Int(playerPos.x, playerPos.y, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().GetFaceDirection());
        StartCoroutine(SceneChange(sceneName, playerPos, fade));
    }
    IEnumerator SceneChange(string sceneName, Vector2Int playerPos, bool fade)
    {
        isChangingScene = true;

        if (fade) { yield return StartCoroutine(ToggleFadeToBlack(true)); }

        SceneManager.LoadScene(sceneName);
        CurrentScene = sceneName;
        locationText.text = System.Text.RegularExpressions.Regex.Replace(CurrentScene, "([a-z])([A-Z])", "$1 $2"); /* https://stackoverflow.com/questions/272633/add-spaces-before-capital-letters */

        if (fade) { yield return StartCoroutine(ToggleFadeToBlack(false)); }

        isChangingScene = false;
        LoadedSave = false; // The save has been loaded, disable this variable to prevent every scene change from loading incorrectly
        yield return null;
    }

    IEnumerator ToggleFadeToBlack(bool fadeToBlack)
    {
        if (!fadeToBlack) // Black -> transparent
        {
            for (float alpha = blackScreen.color.a; alpha > 0f; alpha -= 0.05f)
            {
                blackScreen.color = new Color(255f, 255f, 255f, alpha);
                yield return new WaitForSeconds(0.02f);
            }
        }
        else // Transparent -> Black
        {
            for (float alpha = blackScreen.color.a; alpha < 1f; alpha += 0.05f)
            {
                blackScreen.color = new Color(255f, 255f, 255f, alpha);
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}
