using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneChanger : Interactable
{
    public string targetScene;
    public Vector2Int spawnLocation;
    // Start is called before the first frame update

    public override void OnInteract()
    {
        if (targetScene == null || spawnLocation == null)
        {
            Debug.Log("Error: Please specify the target scene of this door");
            return;
        }
        else
        {
            GameManager.Instance.ChangeToScene(targetScene, spawnLocation);
        }
    }
}
