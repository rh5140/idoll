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
    private Color spriteColor;
    private Color mapColor;
    private Color transparent;
    private bool isSprite;

    [SerializeField] private List<bool> eyes = new List<bool>(NUM_EYES);

    // Start is called before the first frame update
    void Start()
    {
        if (sprite = GetComponent<SpriteRenderer>()) {
            isSprite = true;
            spriteColor = sprite.material.GetColor("_Color");
        }
        //else if (tilemap = GetComponent<Tilemap>()) {
        else if (tilemapRenderer = GetComponent<TilemapRenderer>()) {
            isSprite = false;
            mapColor = tilemap.color;
        }
        transparent = new Color (1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) {
            if (eyes[0]) {
                if (isSprite) {
                    //sprite.material.SetColor("_Color", spriteColor);
                    sprite.enabled = true;
                }
                else {
                    //tilemap.color = mapColor;
                    tilemapRenderer.enabled = true;
                }
            }
            else {
                if (isSprite) {
                    //sprite.material.SetColor("_Color", transparent);
                    sprite.enabled = false;
                }
                else {
                    //tilemap.color = transparent;
                    tilemapRenderer.enabled = false;
                }
            }
            GameManager.Instance.GetComponentInChildren<Light2D>().color = new Color32(255, 255, 255, 255);
        }
        else if (Input.GetKeyDown("2")) {
            if (eyes[1]) {
                if (isSprite) {
                    //sprite.material.SetColor("_Color", spriteColor);
                    sprite.enabled = true;
                }
                else {
                    //tilemap.color = mapColor;
                    tilemapRenderer.enabled = true;
                }
            }
            else {
                if (isSprite) {
                    //sprite.material.SetColor("_Color", transparent);
                    sprite.enabled = false;
                }
                else {
                    //tilemap.color = transparent;
                    tilemapRenderer.enabled = false;
                }
            }
            GameManager.Instance.GetComponentInChildren<Light2D>().color = new Color32(255, 150, 150, 255);
        }
    }
}