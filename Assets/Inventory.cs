using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    [SerializeField] public int          weight;
    [SerializeField] private List<string> items = new();

    
    public string[] GetItems()
    {
        lock(items)
            return items.ToArray();
    }


    public bool AddItem(string item)
    {
        lock (items)
        {
            if (items.Count >= weight)
                return false;
            items.Add(item);
        }

        return true;
    }
}
