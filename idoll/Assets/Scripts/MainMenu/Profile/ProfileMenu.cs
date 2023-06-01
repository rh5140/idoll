using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileMenu : MonoBehaviour
{
    public GameObject eyeballContainer;
    public TextMeshProUGUI description;
    public Image polaroidImage;
    public TextMeshProUGUI polaroidName;
    
    private EyeballSlot[] eyeballSlots;

    private int eyesCollected;

    void Start()
    {
        eyeballSlots = eyeballContainer.GetComponentsInChildren<EyeballSlot>();
        eyesCollected = 0;
    }

    //call when adding an eyeball
    public void EyeballCollected()
    {
        eyeballSlots[eyesCollected].OnCollectEyeball();
        eyesCollected++;
    }
}
