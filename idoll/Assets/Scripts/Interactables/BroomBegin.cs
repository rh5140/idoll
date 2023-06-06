using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomBegin : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInteract()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().SetBroomGame(true);
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
}
