using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // New Unity input system!

public class MainMenu : MonoBehaviour
{
    public GameObject backgroundPanel;
    public GameObject profileMenuContainer;
    public GameObject inventoryMenuContainer;
    public GameObject settingsMenuContainer;

    Coroutine slideTransition;

    private Vector2 navVector = Vector2.zero;
    bool cancelPressed = false;
    bool submitPressed = false;
    private int curTab = 0; // 0 = profile, 1 = inventory, 2 = settings
    private bool inTab = false; // selecting between tabs or navigating within a tab
    private float inputDelay = 0.05f;
    public bool inputBlocked = false;

    // InputSystem
    private Inputs input = null;

    private void Awake()
    {
        input = new Inputs();
    }

        #region InputSystem

        private void OnEnable()
    {
        input.Enable();
        input.UI.Submit.performed += OnSubmitPerformed;
        input.UI.Submit.canceled += OnSubmitCanceled;
        input.UI.Cancel.performed += OnCancelPerformed;
        input.UI.Cancel.canceled += OnCancelCanceled;
        input.UI.Navigate.performed += OnNavigatePerformed;
        input.UI.Navigate.canceled += OnNavigateCanceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.UI.Submit.performed -= OnSubmitPerformed;
        input.UI.Submit.canceled -= OnSubmitCanceled;
        input.UI.Cancel.performed -= OnCancelPerformed;
        input.UI.Cancel.canceled -= OnCancelCanceled;
        input.UI.Navigate.performed -= OnNavigatePerformed;
        input.UI.Navigate.canceled -= OnNavigateCanceled;
    }

    private void OnSubmitPerformed(InputAction.CallbackContext value)
    {
        if (inputBlocked) return;
        submitPressed = true;
    }

    private void OnSubmitCanceled(InputAction.CallbackContext value)
    {
        submitPressed = false;
    }

    private void OnCancelPerformed(InputAction.CallbackContext value)
    {
        if (inputBlocked) return;
        cancelPressed = true;
    }

    private void OnCancelCanceled(InputAction.CallbackContext value)
    {
        cancelPressed = false;
    }

    private void OnNavigatePerformed(InputAction.CallbackContext value)
    {
        if (inputBlocked) return;
        if (GameManager.Instance.GameMode == "menu")
        {
            navVector = value.ReadValue<Vector2>();
            if (navVector.x != 0f)
            {
                navVector.x = navVector.x / Mathf.Abs(navVector.x); // Set magnitude to 1
            }
            if (navVector.y != 0f)
            {
                navVector.y = navVector.y / Mathf.Abs(navVector.y); // Set magnitude to 1
            }
        }
    }

    private void OnNavigateCanceled(InputAction.CallbackContext value)
    {
        navVector = Vector2.zero;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        backgroundPanel.transform.localPosition = new Vector3(0, -480, 0);
        OnProfileClick();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputBlocked) return;
        if (submitPressed == false && cancelPressed == false && navVector == Vector2.zero) return;
        
        StartCoroutine(MenuInputDelay());
        
        if (GameManager.Instance.GameMode == "menu")
        {

            if (!inTab && cancelPressed) { GameManager.Instance.ToggleMenu(); }
            if (!inTab && submitPressed) { inTab = true; }
            if (inTab && cancelPressed) { inTab = false; }

            switch (curTab)
            {
                case 0: // Profile
                    if (!inTab)
                    {
                        if (navVector.x < -0.5) { curTab = 2; OnSettingsClick(); }
                        if (navVector.x > 0.5) { curTab = 1; OnInventoryClick(); }
                    }
                    break;
                case 1: // Inventory
                    if (!inTab)
                    {
                        if (navVector.x < -0.5) { curTab = 0; OnProfileClick(); }
                        if (navVector.x > 0.5) { curTab = 2; OnSettingsClick(); }
                    }
                    break;
                case 2: // Settings
                    if (!inTab)
                    {
                        if (navVector.x < -0.5) { curTab = 1; OnInventoryClick(); }
                        if (navVector.x > 0.5) { curTab = 0; OnProfileClick(); }
                    }
                    break;
                default:
                    Debug.Log("Current menu tab is invalid");
                    break;
            }
        }
        else if (GameManager.Instance.GameMode == "gameplay" && cancelPressed)
        {
            GameManager.Instance.ToggleMenu();
        }

        submitPressed = false;
        cancelPressed = false;
        navVector = Vector2.zero;
    }

    public void OnProfileClick()
    {
        profileMenuContainer.SetActive(true);
        inventoryMenuContainer.SetActive(false);
        settingsMenuContainer.SetActive(false);
    }

    public void OnInventoryClick()
    {
        profileMenuContainer.SetActive(false);
        inventoryMenuContainer.SetActive(true);
        settingsMenuContainer.SetActive(false);

        Inventory.instance.UpdateUI();
    }

    public void OnSettingsClick()
    {
        profileMenuContainer.SetActive(false);
        inventoryMenuContainer.SetActive(false);
        settingsMenuContainer.SetActive(true);
    }

    public void SlideMenu(string s)
    {
        if (slideTransition != null)
        {
            StopCoroutine(slideTransition);
        }
        slideTransition = StartCoroutine(Slide(s));

        /*
        switch(s)
        {
            case "on":
                backgroundPanel.transform.localPosition = new Vector3(0, 0, 0);
                break;
            case "off":
                backgroundPanel.transform.localPosition = new Vector3(0, -480, 0);
                break;

            default:
                Debug.Log("Main Menu Transition Error");
                break;
        }
        */
    }

    IEnumerator MenuInputDelay()
    {
        if (!inputBlocked)
        {
            Debug.Log("Input Blocked, waiting");
            inputBlocked = true;
            yield return new WaitForSeconds(inputDelay);
            inputBlocked = false;
            Debug.Log("Input unblocked");
        }
    }

    IEnumerator Slide(string s)
    {
        Vector3 endY = Vector3.zero;
        switch (s)
        {
            case "on":
                endY = new Vector3(0f, 0f, 0f);
                break;
            case "off":
                endY = new Vector3(0f, -480f, 0f);
                break;
            default:
                Debug.Log("Invalid argument passed to Menu Slide function");
                break;
        }
        Vector3 curV = new Vector3(0f, 0f, 0f);

        while (backgroundPanel.transform.localPosition.y != endY.y)
        {
            backgroundPanel.transform.localPosition = Vector3.SmoothDamp(
                backgroundPanel.transform.localPosition, endY, ref curV, 0.25f);
            yield return null;
        }
        //backgroundPanel.transform.localPosition = new Vector3(0, -480, 0);
    }
}
