using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public int lootValue = 0;
    bool hasInit = false;

    public static event Action<Loot> OnLootCreated;
    public static event Action<Loot> OnLootChanged;
    public static event Action<Loot> OnLootEmpty;

    public void Init(int value)
    {
        if (hasInit) return;
        hasInit = true;

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

    private void Start()
    {
        if (!hasInit) Init(lootValue);
    }

    public void AddLoot(int amountToAdd)
    {
        lootValue += amountToAdd;
    }
    public int TakeLoot(int amountRequested)
    {
        if (amountRequested <= 0) return 0;
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
