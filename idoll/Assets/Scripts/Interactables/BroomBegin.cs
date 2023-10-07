using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomBegin : Interactable
{
    public override void OnInteract()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().SetBroomGame(true);
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
}
