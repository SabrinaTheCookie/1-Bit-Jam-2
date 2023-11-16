using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLootController : MonoBehaviour
{
    public List<Loot> lootpiles;
    public int totalLoot;
    public static event Action<int> OnLootUpdate;
    private void OnEnable()
    {
        Loot.OnLootCreated += AddLoot;
        Loot.OnLootChanged += LootChanged;
        Loot.OnLootEmpty += RemoveLoot;
    }

    private void OnDisable()
    {
        Loot.OnLootCreated -= AddLoot;
        Loot.OnLootChanged -= LootChanged;
        Loot.OnLootEmpty -= RemoveLoot;
    }

    void AddLoot(Loot newLoot)
    {
        if(!lootpiles.Contains(newLoot))
        {
            lootpiles.Add(newLoot);
        }
    }

    void LootChanged(Loot lootChanged)
    {
        RecalculateLoot();
    }

    void RemoveLoot(Loot lootToRemove)
    {
        if (lootpiles.Contains(lootToRemove))
        {
            lootpiles.Remove(lootToRemove);
        }
    }

    void RecalculateLoot()
    {
        int lootCount = 0;
        foreach(Loot loot in lootpiles)
        {
            lootCount += loot.lootValue;
        }
        if(totalLoot != lootCount)
        {
            OnLootUpdate?.Invoke(lootCount);
            totalLoot = lootCount;
        }
        
    }
}
