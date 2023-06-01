using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject profileMenuContainer;
    public GameObject inventoryMenuContainer;
    public GameObject settingsMenuContainer;

    // Start is called before the first frame update
    void Start()
    {
        OnProfileClick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnProfileClick()
    {
        profileMenuContainer.SetActive(true);
        inventoryMenuContainer.SetActive(false);
        settingsMenuContainer.SetActive(false);
    }

    public void OnInventoryClick()
    {
        profileMenuContainer.SetActive(false);
        inventoryMenuContainer.SetActive(true);
        settingsMenuContainer.SetActive(false);

        Inventory.instance.UpdateUI();
    }

    public void OnSettingsClick()
    {
        profileMenuContainer.SetActive(false);
        inventoryMenuContainer.SetActive(false);
        settingsMenuContainer.SetActive(true);
    }
}
