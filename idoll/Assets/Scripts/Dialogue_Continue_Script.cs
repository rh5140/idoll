using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Dialogue_Continue_Script : MonoBehaviour
{
    private float timer;
    private float duration = 0.5f; //Add delay between skips
    [SerializeField] LineView lineview;

    private void Start()
    {
        timer = 0.5f;
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetButton("Interact") && timer == 0) {
            lineview.OnContinueClicked();
            timer = duration;
        }
        if (timer > 0)
        {
            timer = Mathf.Max(0, timer - Time.deltaTime);
        }
    }
}