using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class CharacterPortrait : MonoBehaviour
{

    [YarnCommand("updateface")]
    void UpdateCharacter(string name)
    {
        ShowFace();
        Image image = GetComponent<Image>();
        if (image != null)
        {
            Sprite img = Resources.Load<Sprite>("Portraits/"+name);
            if (img == null)
            {
                img = Resources.Load<Sprite>("Portraits/Default");
            }

            image.sprite = img;
        }
    }

    [YarnCommand("hideface")]
    void HideFace()
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.enabled = false;
        }
    }

    [YarnCommand("showface")]
    void ShowFace()
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.enabled = true;
        }
    }

}
