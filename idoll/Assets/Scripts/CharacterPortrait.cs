using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class CharacterPortrait : MonoBehaviour
{


    [YarnCommand("hideface")]
    public void HideFace()
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.enabled = false;
        }

        GameObject box = gameObject.transform.parent.Find("Background_Character").gameObject;
        if (box != null)
        {
            box.SetActive(false);
        }
    }

    [YarnCommand("showface")]
    public void ShowFace(string name = "")
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.enabled = true;
            Sprite img = Resources.Load<Sprite>("Portraits/" + name);
            if (img == null)
            {
                Debug.Log("ReferenceError : " + name + " does not exist in Portraits");
                image.enabled = false;
            }

            image.sprite = img;
        }

        GameObject box = gameObject.transform.parent.Find("Background_Character").gameObject;
        if (box != null)
        {
            box.SetActive(true);
        }
    }

}
