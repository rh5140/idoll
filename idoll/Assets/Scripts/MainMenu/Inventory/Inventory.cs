using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory instance {get; private set;} = null;

    public Transform itemsParent;
    public DurabilityItem testDurabilityItem;
    public SingleUseItem testSingleUseItem;

    public TextMeshProUGUI descriptionBox;

    private ItemSlot[] itemSlots;

    private const int MaxInventorySize = 12;
    private int currentInventoryUsage = 0;

    private Dictionary<string, List<int>> inventory = new Dictionary<string, List<int>>();
    private Dictionary<string, Item> nameToItem = new Dictionary<string, Item>();

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null) 
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();

        foreach (ItemSlot slot in itemSlots)
        {
            slot.description = descriptionBox;
        }

        descriptionBox.text = "";

        Debug.Log("Successfully setup Inventory with " + itemSlots.Length + " ItemSlots");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        int itemSlotIndex = 0;
        
        foreach(string itemName in inventory.Keys)
        {
            List<int> stackCounts = inventory[itemName];
            int stackIndex = 0;

            foreach(int count in stackCounts)
            {
                itemSlots[itemSlotIndex].AddItem(nameToItem[itemName], count, stackIndex);
                itemSlotIndex++;
                stackIndex++;
            }
        }

        for (; itemSlotIndex < MaxInventorySize; itemSlotIndex++)
        {
            itemSlots[itemSlotIndex].ClearSlot();
        }
    }

    // Returns the count added to the inventory up to MAX_COUNT.
    public int AddItem(Item item, int numberToAdd = 1) 
    {
        string itemName = item.name;
        int numberAdded = 0;

        if (!nameToItem.ContainsKey(itemName))
        {
            nameToItem.Add(itemName, item);
        }

        if (item.hasDurability) {
            if (!inventory.ContainsKey(itemName))
            {
                inventory.Add(itemName, new List<int>());
            }

            List<int> durabilities = inventory[itemName];

            for (int i = 0; i < numberToAdd && currentInventoryUsage != MaxInventorySize; i++)
            {
                durabilities.Add(((DurabilityItem) item).maxDurability);

                currentInventoryUsage += 1;
                numberAdded++;
            }

            Debug.Log("Added item " + itemName + " " + numberAdded + " times");
        }

        else {
            int itemMaxCount = item.maxCount;

            if (!inventory.ContainsKey(itemName) && currentInventoryUsage < MaxInventorySize) 
            {
                List<int> countList = new List<int>();
                countList.Add(0);
                inventory.Add(itemName, countList);

                currentInventoryUsage++;
            }
            
            if (inventory.ContainsKey(itemName))
            {
                List<int> stackCounts = inventory[itemName];
                int lastElementIndex = stackCounts.Count - 1;
                int lastCount = stackCounts[lastElementIndex];

                if (lastCount + numberToAdd > item.maxCount)
                {
                    int diff = lastCount + numberToAdd - itemMaxCount;
                    stackCounts[lastElementIndex] = itemMaxCount;

                    numberAdded += itemMaxCount - lastCount;

                    if (currentInventoryUsage < MaxInventorySize)
                    {
                        // Add another stack then call recursively to add the rest
                        stackCounts.Add(0);

                        currentInventoryUsage++;

                        numberAdded += AddItem(item, diff);
                    }
                }

                else
                {
                    stackCounts[lastElementIndex] += numberToAdd;
                    numberAdded = numberToAdd;
                }
            }
        }

        UpdateUI();

        return numberAdded;
    }

    // Returns true if items used successfully. Otherwise, return false.
    public bool UseItem(Item item, int numberToUse = 1, int itemStackIndex = 1) 
    {
        string itemName = item.name;
        List<int> stackCounts = inventory[itemName];
        int numStacks = stackCounts.Count;
        int currentStack = stackCounts[numStacks - 1];
        int totalOfItem = (numStacks - 1) * item.maxCount + currentStack;

        Debug.Log("Total of " + itemName + " is " + totalOfItem);

        if (!inventory.ContainsKey(itemName))
        {
            return false;
        }

        if (totalOfItem < numberToUse)
        {
            return false;
        }

        else if (totalOfItem == numberToUse)
        {
                
            for (int i=0; i<numberToUse; i++)
            {
                if (!item.Use())
                {
                    return false;
                }
            }

            for (int i = 0; i < numStacks; i++)
            {
                RemoveItemStack(item);
            }


        }

        else if (currentStack < numberToUse)
        {
                

            for (int i = 0; i < currentStack; i++)
            {
                if (!item.Use()){ 
                    return false; 
                }
            }
            RemoveItemStack(item);
            UseItem(item, numberToUse - currentStack);
        }

        else 
        {
            for (int i = 0; i < numberToUse; i++)
            {
                if (!item.Use())
                {
                    return false;
                }
            }

            stackCounts[numStacks - 1] -= numberToUse;

                

            if (stackCounts[numStacks - 1] == 0)
            {
                RemoveItemStack(item);
            }
        }

        UpdateUI();

        return true;
    }

    public bool RemoveItemStack(Item item, int itemStackIndex = -1) 
    {
        // if (item.hasDurability)
        // {
        //     DurabilityItem durabilityItem = (DurabilityItem) item;


        //     if (!durabilityInventory.ContainsKey(durabilityItem)) 
        //     {
        //         return false;
        //     }

        //     List<int> durabilities = durabilityInventory[durabilityItem];

        //     durabilities.RemoveAt(itemStackIndex);
        //     currentInventoryUsage--;

        //     if (durabilities)
        // }

        //else {
            string itemName = item.name;

            if (!inventory.ContainsKey(itemName)) 
            {
                return false;
            }

            List<int> stackCounts = inventory[itemName];

            if (itemStackIndex == -1) 
            {
                stackCounts.RemoveAt(stackCounts.Count - 1);
            }

            else
            {
                stackCounts.RemoveAt(itemStackIndex);
            }

            currentInventoryUsage--;

            if (stackCounts.Count == 0)
            {
                inventory.Remove(itemName);
            }
        //}

        UpdateUI();

        return true;
    }

    public void AddTestDurabilityItem()
    {
        AddItem(testDurabilityItem);
    }

    public void AddTestSingleUseItem()
    {
        AddItem(testSingleUseItem);
    }
}
