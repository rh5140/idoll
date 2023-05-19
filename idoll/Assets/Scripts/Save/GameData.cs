using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int storyState;
    public int currentEye;
    public string currentScene;
    public bool companionFollow;
    public int[] position;
    public int direction;
    // If we have time, add time spent playing!
    // Ask Andrew about inventory stuff

    public GameData()
    {
        storyState= (int) GameManager.Instance.StoryState;
        currentEye = GameManager.Instance.CurrentEye;
        currentScene = GameManager.Instance.CurrentScene;
        companionFollow = GameManager.Instance.CompanionFollow;

        GameManager.Instance.GetPlayerPosition();
        position = new int[2];
        position[0] = GameManager.Instance.PlayerPositionX;
        position[1] = GameManager.Instance.PlayerPositionY;
        direction = GameManager.Instance.PlayerFacing;
    }
}
