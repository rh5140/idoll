using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal; // Light2D

public class Eyeballs : MonoBehaviour
{
    public const int NUM_EYES = 2;
    
    private SpriteRenderer sprite;
    private Tilemap tilemap;
    private TilemapRenderer tilemapRenderer;
    private bool isSprite;

    // Currently used to make sure frames update in the right order :(
    private int loadSceneWithFirstEyeballCounter = 0; // Temporary for Winter showcase. Switch to eyeball 1 when scene loads

    [SerializeField] private List<bool> eyes = new List<bool>(NUM_EYES);
    private int currentEye = GameManager.Instance.currentEye;

    // Start is called before the first frame update
    void Start()
    {
        if (sprite = GetComponent<SpriteRenderer>()) {
            isSprite = true;
        }
        else if (tilemapRenderer = GetComponent<TilemapRenderer>()) {
            isSprite = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (loadSceneWithFirstEyeballCounter <= 2) loadSceneWithFirstEyeballCounter++;

        if (Input.GetKeyDown("1"))
        {
            GameManager.Instance.currentEye = 1;
        }
        else if (Input.GetKeyDown("2"))
        {
            GameManager.Instance.currentEye = 2;   
        }

        if (GameManager.Instance.currentEye == 1 || loadSceneWithFirstEyeballCounter == 2) {
            if (eyes[0]) {
                if (isSprite) {
                    sprite.enabled = true;
                }
                else {
                    tilemapRenderer.enabled = true;
                }
            }
            else {
                if (isSprite) {

                    sprite.enabled = false;
                }
                else {
                    tilemapRenderer.enabled = false;
                }
            }
            GameManager.Instance.GetComponentInChildren<Light2D>().color = new Color32(255, 255, 255, 255);
        }
        else if (GameManager.Instance.currentEye == 2) {
            if (eyes[1]) {
                if (isSprite) {
                    sprite.enabled = true;
                }
                else {
                    tilemapRenderer.enabled = true;
                }
            }
            else {
                if (isSprite) {
                    sprite.enabled = false;
                }
                else {
                    tilemapRenderer.enabled = false;
                }
            }
            GameManager.Instance.GetComponentInChildren<Light2D>().color = new Color32(255, 150, 150, 255);
        }
    }
}