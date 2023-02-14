using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Dialogue_Continue_Script : MonoBehaviour
{
    [SerializeField] LineView lineview;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("z")) {
            lineview.OnContinueClicked();
        }
    }
}