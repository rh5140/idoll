using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool playerAnchored = false;
    private float speed = 5.0f;
    private Grid myGrid;
    private UnityEngine.Vector2 target = new UnityEngine.Vector2(0, 0);
    private bool currentlyMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        myGrid = (GameObject.Find("Grid")).GetComponent<Grid>();
    }

    public void AnchorPlayer(bool anchorState)
    {
        playerAnchored = anchorState;
    }

    private void UpdateLocation()
    {
        float yAxis = Input.GetAxisRaw("Vertical");
        float xAxis = Input.GetAxisRaw("Horizontal");

        if (xAxis != 0 && yAxis != 0)
        {
            currentlyMoving = true;
            target = new UnityEngine.Vector2(transform.Find("PlayerSprite").position.x + xAxis, transform.Find("PlayerSprite").position.y + yAxis);
        }

        if (xAxis != 0 && !currentlyMoving)
        {
            currentlyMoving = true;
            target = new UnityEngine.Vector2(transform.Find("PlayerSprite").position.x + xAxis, transform.Find("PlayerSprite").position.y);
        }

        if (yAxis != 0 && !currentlyMoving)
        {
            currentlyMoving = true;
            target = new UnityEngine.Vector2(transform.Find("PlayerSprite").position.x, transform.Find("PlayerSprite").position.y + yAxis);
        }

    }

    private void MoveSprite()
    {
        transform.Find("PlayerSprite").position = UnityEngine.Vector3.MoveTowards(transform.Find("PlayerSprite").position, target, speed * Time.deltaTime);

        if (Mathf.Abs(transform.Find("PlayerSprite").position.x - target.x) < 0.1f)
        {
            currentlyMoving = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerAnchored && !currentlyMoving)
            UpdateLocation();

        if (currentlyMoving)
            MoveSprite();

    }
}
