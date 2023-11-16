using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public int lootValue = 0;

    public static event Action<Loot> OnLootCreated;
    public static event Action<Loot> OnLootChanged;
    public static event Action<Loot> OnLootEmpty;

    public void Init(int value)
    {
        RaycastHit hit;
        Physics.SphereCast(transform.position, 1, Vector3.down, out hit, gameObject.layer);
        if(hit.transform)
        {
            hit.transform.GetComponent<Loot>().AddLoot(value);
            Destroy(gameObject);
            return;
        }
        lootValue = value;
        OnLootCreated?.Invoke(this);
    }

    public void AddLoot(int amountToAdd)
    {
        lootValue += amountToAdd;
    }
    public int TakeLoot(int amountRequested)
    {
        amountRequested = Mathf.Min(amountRequested, lootValue);
        lootValue -= amountRequested;
        if(lootValue == 0)
        {
            LootEmpty();
        }
        else
        {
            OnLootChanged?.Invoke(this);
        }
        return (Mathf.Min(amountRequested, lootValue));
    }

    void LootEmpty()
    {
        OnLootEmpty?.Invoke(this);
    }

    
}
