using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLootController : Singleton<GameLootController>
{
    public List<Loot> lootpiles;
    public int totalLoot;
    public static event Action<int> OnLootUpdate;
    public int maxGold;

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
            RecalculateLoot();
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
            RecalculateLoot();
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
            totalLoot = lootCount;
            if (maxGold == 0) maxGold = totalLoot;
            OnLootUpdate?.Invoke(lootCount);
        }
        
    }
}
