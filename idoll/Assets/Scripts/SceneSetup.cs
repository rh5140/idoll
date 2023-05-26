using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // CHECK IF THE DICTIONARY ALREADY EXISTS
        string sceneName = SceneManager.GetActiveScene().name;
        GameManager.Instance.currentScene = sceneName;
        if (!GameManager.Instance.progressDict.ContainsKey(sceneName))
        {
            // Scene-specific dictionary generator
            Dictionary<string, bool> sceneDict = new Dictionary<string, bool>();

            // Get all interactables in scene
            var foundInteractables = FindObjectsOfType<Interactable>();
            int length = foundInteractables.Length;

            // for each interactable
            // dictionary.add (prob also check if already added just in case)
            for (int i = 0; i < length; i++)
            {
                string objName = foundInteractables[i].name;
                bool objBool = foundInteractables[i].activated > 0; 
                sceneDict.Add(objName, objBool);
            }
            GameManager.Instance.progressDict.Add(sceneName, sceneDict);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
