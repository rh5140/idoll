using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal; // Light2D
using UnityEngine.InputSystem; // New Unity input system!

public class Eyeballs : MonoBehaviour
{
    private SpriteRenderer sprite;
    private TilemapRenderer tilemap;
    private bool isSprite;

    [SerializeField] private List<bool> eyes = new List<bool>();
    private List<Color32> screenTint = new List<Color32>();

    // Input System
    private Inputs inputs = null;
    private float input;

    private void Awake()
    {
        // inputs = new Inputs();
        // input = GameManager.Instance.CurrentEye;
    }

    #region InputSystem

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Eyeballs.performed += OnMovementPerformed;
        inputs.Player.Eyeballs.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        inputs.Disable();
        inputs.Player.Eyeballs.performed -= OnMovementPerformed;
        inputs.Player.Eyeballs.canceled -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        input = value.ReadValue<float>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        input = 0;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        inputs = new Inputs();
        input = GameManager.Instance.CurrentEye;
        
        if (sprite = GetComponent<SpriteRenderer>()) {
            isSprite = true;
        }
        else if (tilemap = GetComponent<TilemapRenderer>()) {
            isSprite = false;
        }

        // Screen Tints
        screenTint.Add(new Color32(255, 255, 255, 255)); // Eyeball 1
        screenTint.Add(new Color32(255, 150, 150, 255)); // Eyeball 2
    }

    // Update is called once per frame
    void Update()
    {
        // Check for valid input, set currentEye to the input
        if (input == 0)
        {
            return;
        }
        else if (input > eyes.Count || input > screenTint.Count)
        {
            Debug.Log("No eyeball #" + input + " exists!");
            return;
        }
        else
        {
            GameManager.Instance.CurrentEye = (int) input;
        }

        // Change visibility based on current eye
        if (eyes[GameManager.Instance.CurrentEye - 1])
        {
            SetVisible(true);
            GameManager.Instance.GetComponentInChildren<Light2D>().color = screenTint[GameManager.Instance.CurrentEye - 1];
        }
        else
        {
            SetVisible(false);
        }
    }

    private void SetVisible(bool b)
    {
        if (isSprite)
        {
            sprite.enabled = b;
        }
        else
        {
            tilemap.enabled = b;
        }
    }
}