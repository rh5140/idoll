using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSetup : MonoBehaviour
{
    [SerializeField] int sceneMusic;

    // Start is called before the first frame update
    void Start()
    {
        // Set player position if from save
        if (GameManager.Instance.LoadedSave)
        {           
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector2(GameManager.Instance.PlayerPositionX, GameManager.Instance.PlayerPositionY);
            player.GetComponent<PlayerMovement>().SetFaceDirection(GameManager.Instance.PlayerFacing);

            // Set Player Target to have the same position
        }

        // CHECK IF THE DICTIONARY ALREADY EXISTS
        string sceneName = SceneManager.GetActiveScene().name;
        GameManager.Instance.CurrentScene = sceneName;
        if (!GameManager.Instance.progressDict.ContainsKey(sceneName))
        {
            // Scene-specific dictionary generator
            Dictionary<string, bool> sceneDict = new Dictionary<string, bool>();

            // Get all interactables in scene
            var foundInteractables = FindObjectsOfType<Interactable>();
            int length = foundInteractables.Length;

            // for each interactable
            for (int i = 0; i < length; i++)
            {
                string objName = foundInteractables[i].name;
                bool objBool = foundInteractables[i].activated > 0;
                // dictionary.add (prob also check if already added just in case)
                if (!sceneDict.ContainsKey(objName))
                    sceneDict.Add(objName, objBool);
            }
            GameManager.Instance.progressDict.Add(sceneName, sceneDict);
        }

        // Set GameMode to "gameplay"
        GameManager.Instance.SetGameMode("gameplay");

        // Set the music for the scene
        // TODO - loewr priority so that other things can override scene music\

        GameManager.Instance.musicPlayer.StopMusic();
        if (sceneMusic > 0)
        {
            // Note, music list starts at 1, not 0
            GameManager.Instance.musicPlayer.SetTrack(sceneMusic-1);
        }
    }
}
