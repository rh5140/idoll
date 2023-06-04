using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.InputSystem; // New Unity input system!

public class Dialogue_Continue_Script : MonoBehaviour
{
    private Inputs input;

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
        lineview.OnContinueClicked();
    }

}