using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Referencing this: https://medium.com/nerd-for-tech/implementing-a-game-manager-using-the-singleton-pattern-unity-eb614b9b1a74
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private int gameMode;

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
        _instance = this;
    }


    // Call and change GameMode using Yarn Spinner functions
    public int GameMode { get; set; }
}
