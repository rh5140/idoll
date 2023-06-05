using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LockedDoor : Dialogue
{
    // Start is called before the first frame update
    public string doorName;
    public bool locked = true;
    public string targetScene;
    public Vector2Int spawnLocation;

    protected override void Start()
    {
        base.Start();
    }

    [YarnCommand("UnlockDoor")]
    public void Unlock()
    {
        locked = false;
    }

    public override void OnInteract()
    {
        if (locked)
        {
            base.OnInteract();
        }
        else
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

}
