using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    public static Inventory instance {get; private set;} = null;

    public Transform itemsParent;

    private ItemSlot[] itemSlots;

    private const int MaxCount = 64;
    private const int MaxInvSize = 24;

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

        itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();

        Debug.Log("Successfully setup Inventory with " + itemSlots.Length + " ItemSlots");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        int n = 0;
        foreach(Item i in durabilityInventory)
        {
            itemSlots[n].AddItem(i, 1);
            n++;
        }

        foreach(Item i in singleUseInventory.Keys)
        {
            itemSlots[n].AddItem(i, singleUseInventory[i]);
            n++;
        }

        for (int i = n; i < MaxInvSize; i++)
        {
            itemSlots[i].ClearSlot();
        }
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
                durabilityInventory.Add((DurabilityItem) (o.Copy()));
            }

            Debug.Log("Added item " + o.name);

            UpdateUI();

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

                UpdateUI();

                return diff;
            }
            else
            {
                singleUseInventory[o] += c;
            }

            UpdateUI();

            return c;
        }
    }

    // Returns true if items used successfully. Otherwise, return false.
    public bool UseItem(Item o, int count = 1) 
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
                if (oDur.durability >= count)
                {
                    for (int i = 0; i < count; i++)
                    {
                        oDur.Use();
                    }
                }
                else 
                {
                    return false;
                }

                if (oDur.durability == 0)
                {
                    RemoveItem(oDur);
                }

                UpdateUI();

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
                RemoveItem(o);
            }
            else
            {
                singleUseInventory[o] -= count;
            }

            for (int i = 0; i < count; i++)
            {
                o.Use();
            }

            UpdateUI();

            return true;
        }
    }

    public bool RemoveItem(Item o) 
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

                UpdateUI();

                return true;
            }
        }

        else {
            if (!singleUseInventory.ContainsKey(o)) 
            {
                return false;
            }
            else
            {
                singleUseInventory.Remove(o);
            }

            UpdateUI();

            return true;
        }
    }

    public void AddTestDurabilityItem()
    {
        AddItem(new TestDurabilityItem());
    }

    public void AddTestSingleUseItem()
    {
        AddItem(new TestSingleUseItem());
    }
}
