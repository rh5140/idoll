using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance {get; private set;} = null;

    private const int MaxCount = 64;

    private Dictionary<Item, int> singleUseInventory = new Dictionary<Item, int>();
    private List<DurabilityItem> durabilityInventory = new List<DurabilityItem>();

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

    public int Count(Item o)
    {
        if (o.hasDurability)
        {
            return durabilityInventory.IndexOf((DurabilityItem) o) == -1 ? 0 : 1;
        }

        else
        {
            if (singleUseInventory.ContainsKey(o))
            {
                return singleUseInventory[o];
            }

            else
            {
                return 0;
            }
        }
    }

    // Returns the count added to the inventory up to MAX_COUNT.
    public int AddItem(Item o, int count = 1) 
    {
        if (o.hasDurability) {
            for (int i = 0; i < count; i++)
            {
                durabilityInventory.Add(((DurabilityItem) o).Clone());
            }

            return count;
        }

        else {
            int c = Math.Min(count, MaxCount);

            if (!singleUseInventory.ContainsKey(o)) 
            {
                singleUseInventory.Add(o, c);
            }
            else if (singleUseInventory[o] + count > MaxCount) 
            {
                int diff = singleUseInventory[o] + count - MaxCount;
                singleUseInventory[o] = MaxCount;
                return diff;
            }
            else
            {
                singleUseInventory[o] += c;
            }

            return c;
        }
    }

    // Returns true if items removed successfully. Otherwise, return false.
    public bool RemoveItem(Item o, int count = 1) 
    {
        if (o.hasDurability)
        {
            DurabilityItem oDur = (DurabilityItem) o;

            if (durabilityInventory.IndexOf(oDur) == -1) 
            {
                return false;
            }

            else 
            {
                durabilityInventory.Remove(oDur);
                return true;
            }
        }

        else {
            if (!singleUseInventory.ContainsKey(o) || singleUseInventory[o] < count) 
            {
                return false;
            }
            else if (singleUseInventory[o] == count)
            {
                singleUseInventory.Remove(o);
            }
            else
            {
                singleUseInventory[o] -= count;
            }

            return true;
        }
    }
}
