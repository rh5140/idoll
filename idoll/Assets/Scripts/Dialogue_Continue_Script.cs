using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.InputSystem; // New Unity input system!

public class Dialogue_Continue_Script : MonoBehaviour
{
    private Inputs input;

    //private float timer; // Timer is no-longer required with the LineViewCustom script!
    //private float duration = 0.1f; // Add delay between skips
    [SerializeField] LineViewCustom lineview;

    #region InputSystem

    private void Awake()
    {
        input = new Inputs();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Primary.performed += OnInteract;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Primary.performed -= OnInteract;
    }

    #endregion

    public void OnInteract(InputAction.CallbackContext context)
    {
        //if (timer <= 0)
        //{
            lineview.OnContinueClicked();
        //    timer = duration;
        //}
    }
    /*
    // Update is called once per frame
    private void Update()
    {
        if (timer > 0)
        {
            timer = Mathf.Max(0, timer - Time.deltaTime);
        }
    }
    */
}