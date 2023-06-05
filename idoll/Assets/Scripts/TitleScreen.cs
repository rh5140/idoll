using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Image[] Menu;
    [SerializeField] private Image eye;
    [SerializeField] private Image overlay;

    Coroutine fade;

    private Vector2 navVector = Vector2.zero;
    bool cancelPressed = false;
    bool submitPressed = false;
    private int curMenuSelect = 0;
    private float inputDelay = 0.05f;
    public bool inputBlocked = false;

    // InputSystem
    private Inputs input = null;

    void Awake()
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

    private void OnNavigateCanceled(InputAction.CallbackContext value)
    {
        navVector = Vector2.zero;
    }

    #endregion

    private void Start()
    {
        HighlightMenuOption(0);
        fade = StartCoroutine(Fade());
        overlay.color = new Color(0f, 0f, 0f, 0f);
        GameManager.Instance.SetGameMode("title");
        GameManager.Instance.musicPlayer.SetTrack(1);
    }

    void Update()
    {
        SelectMenuOption();
    }

    private void SelectMenuOption()
    {
        if (inputBlocked) return;
        if (submitPressed == false && cancelPressed == false && navVector == Vector2.zero) return;

        StartCoroutine(MenuInputDelay());

        if (cancelPressed == true) // Highlight exit
        {
            HighlightMenuOption(Menu.Length - 1); // Select exit
            return;
        }

        switch(curMenuSelect)
        {
            case 0: // Start
                if (submitPressed)
                {
                    if (fade != null) StopCoroutine(fade);
                    GameManager.Instance.SetStory(0, 0, 1);
                    GameManager.Instance.musicPlayer.StopMusic();
                    GameManager.Instance.ChangeToScene("Poppy's Room", new Vector2Int(1, 0));
                }
                if (navVector.y > 0) HighlightMenuOption(3);
                if (navVector.y < 0) HighlightMenuOption(1);
                break;
            case 1: // Load
                if (submitPressed)
                {
                    if (fade != null) StopCoroutine(fade);
                    GameManager.Instance.musicPlayer.StopMusic();
                    GameManager.Instance.LoadGame();
                }
                // TODO: modifiy for more save files
                if (navVector.y > 0) HighlightMenuOption(0);
                if (navVector.y < 0) HighlightMenuOption(2);
                break;
            case 2: // Credits
                if (submitPressed) Debug.Log("No credits yet, check itch.io");
                if (navVector.y > 0) HighlightMenuOption(1);
                if (navVector.y < 0) HighlightMenuOption(3);
                break;
            case 3: // Exit
                if (submitPressed) Application.Quit();
                if (navVector.y > 0) HighlightMenuOption(2);
                if (navVector.y < 0) HighlightMenuOption(0);
                break;
            default:
                Debug.Log("Menu option does not exist!");
                break;
        }
        submitPressed = false;
        cancelPressed = false;
        navVector = Vector2.zero;
    }

    private void HighlightMenuOption(int select)
    {
        //Debug.Log("Highlighting menu option " + select);
        for (int i = 0; i < Menu.Length; i++)
        {
            Menu[i].color = new Color(1f, 1f, 1f);
            Menu[i].gameObject.transform.localScale = Vector3.one;
        }
        Menu[select].color = new Color(1f, 1f, 0.5f);
        Menu[select].gameObject.transform.localScale = new Vector3(1.05f, 1.05f);
        curMenuSelect = select;
    }

    IEnumerator MenuInputDelay()
    {
        if (!inputBlocked)
        {
            inputBlocked = true;
            yield return new WaitForSeconds(inputDelay);
            inputBlocked = false;
        }
    }

    IEnumerator Fade()
    {
        for(; ;)
        {
            yield return new WaitForSeconds(7.8f + Random.value * 5f);
            GameManager.Instance.musicPlayer.PitchBend(0.04f, 0.5f);
            yield return new WaitForSeconds(0.2f);
            for (float i = 0.04f; i <= 1; i += 0.05f)
            {
                overlay.color = new Color(0f, 0f, 0f, i);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(0.1f);
            eye.color = Color.HSVToRGB(Random.value, 0.4f, 1f);
            yield return new WaitForSeconds(0.1f);
            for (float i = 0.96f; i >= 0; i -= 0.05f)
            {
                overlay.color = new Color(0f, 0f, 0f, i);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}