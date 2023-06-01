using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeballSlot : MonoBehaviour
{
    public GameObject emptyEyeballContainer;
    public GameObject eyeballContainer;

    private bool playerHasEyeball;

    // Start is called before the first frame update
    void Start()
    {
        playerHasEyeball = false;
    }

    public void OnClickSlot()
    {
        if (playerHasEyeball)
        {
            //change eyeball to this eyeball
        }

        else
        {
            Debug.Log("Eyeball not collected yet");
        }
    }

    public void OnCollectEyeball()
    {
        playerHasEyeball = true;
        emptyEyeballContainer.SetActive(false);
        eyeballContainer.SetActive(true);
    }
}
