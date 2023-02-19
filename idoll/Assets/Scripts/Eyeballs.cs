using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyeballs : MonoBehaviour
{
    public const int NUM_EYES = 2;
    
    private SpriteRenderer sprite;
    private Color thisColor;
    private Color transparent;

    [SerializeField] private List<bool> eyes = new List<bool>(NUM_EYES);

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        thisColor = sprite.material.GetColor("_Color");
        transparent = new Color (1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) {
            if (eyes[0]) {
                sprite.material.SetColor("_Color", thisColor);
            }
            else {
                sprite.material.SetColor("_Color", transparent);
            }
        }
        else if (Input.GetKeyDown("2")) {
            if (eyes[1]) {
                sprite.material.SetColor("_Color", thisColor);
            }
            else {
                sprite.material.SetColor("_Color", transparent);
            }
        }
    }
}