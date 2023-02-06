using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance {get; private set;} = null;

    private const int MaxCount = 64;

    private Dictionary<GameObject, int> inventory = new Dictionary<GameObject, int>();

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Returns the count added to the inventory up to MAX_COUNT.
    int Count(GameObject o)
    {
        return inventory[o];
    }

    int AddItem(GameObject o, int count = 1) 
    {
        int c = Math.Min(count, MaxCount);

        if (!inventory.ContainsKey(o)) 
        {
            inventory.Add(o, c);
        }
        else if (inventory[o] + count > MaxCount) 
        {
            int diff = inventory[o] + count - MaxCount;
            inventory[o] = MaxCount;
            return diff;
        }
        else
        {
            inventory[o] += c;
        }

        return c;
    }

    // Returns true if items removed successfully. Otherwise, return false.
    bool RemoveItem(GameObject o, int count = 1) 
    {
        if (!inventory.ContainsKey(o) || inventory[o] < count) 
        {
            return false;
        }
        else if (inventory[o] == count)
        {
            inventory.Remove(o);
        }
        else
        {
            inventory[o] -= count;
        }

        return true;
    }
}
