using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyeballs : MonoBehaviour
{
    public const int SHOW = 1;
    public const int HIDE = 0;
    private SpriteRenderer sprite;
    private Component defaultEyes;
    private Component redEyes;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        defaultEyes = GetComponent<DefaultEyes>();
        redEyes = GetComponent<RedEyes>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) {
            if (defaultEyes) {
                sprite.sortingOrder = SHOW;
            }
            else {
                sprite.sortingOrder = HIDE;
            }
        }
        else if (Input.GetKeyDown("2")) {
            if (redEyes) {
                sprite.sortingOrder = SHOW;
            }
            else {
                sprite.sortingOrder = HIDE;
            }
        }
    }
}