using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer currentBroomSprite;
    [SerializeField] Sprite[] broomSprites;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < player.transform.position.y) // Below player
        {
            currentBroomSprite.sprite = broomSprites[0];
        }
        else if (this.transform.position.x < player.transform.position.x) // Left of player
        {
            currentBroomSprite.sprite = broomSprites[1];
        }
        else if (this.transform.position.x > player.transform.position.x) // Right of player
        {
            currentBroomSprite.sprite = broomSprites[2];
        }
        else // Above player
        {
            currentBroomSprite.sprite = broomSprites[3];
        }
    }
}
