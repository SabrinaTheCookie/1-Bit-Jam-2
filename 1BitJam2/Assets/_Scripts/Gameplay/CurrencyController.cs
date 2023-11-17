using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;

public class CurrencyController : Singleton<CurrencyController>
{
    [Header("Gold")]
    public List<Loot> lootpiles;
    public int totalLoot;
    public static event Action<int> OnLootUpdate;
    public static event Action<int> OnSoulUpdate;
    public int maxGold;

    [Header("Souls")] 
    public int currentSouls;

    private void OnEnable()
    {
        Loot.OnLootCreated += AddLoot;
        Loot.OnLootChanged += LootChanged;
        Loot.OnLootEmpty += RemoveLoot;
        Enemy.OnEnemyDefeated += HarvestSoul;
    }

    private void OnDisable()
    {
        Loot.OnLootCreated -= AddLoot;
        Loot.OnLootChanged -= LootChanged;
        Loot.OnLootEmpty -= RemoveLoot;
        Enemy.OnEnemyDefeated -= HarvestSoul;

    }

    #region Souls

    void HarvestSoul(Enemy soulToHarvest, bool escaped)
    {
        if (!escaped)
        {
            currentSouls += soulToHarvest.soulsOnDeath;
            OnSoulUpdate?.Invoke(currentSouls);
        }
    }

    public bool HasEnoughSouls(int requestedSouls)
    {
        return requestedSouls <= currentSouls;
    }
    public bool ConsumeSoul(int amountToConsume)
    {
        //Not enough souls!
        if (amountToConsume > currentSouls) return false;
        
        //Enough souls :3
        currentSouls -= amountToConsume;
        OnSoulUpdate?.Invoke(currentSouls);
        return true;
    }

    #endregion
    #region Loot & Gold
    
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
            if (totalLoot == 0)
            {
                AudioManager.Instance.PlayMusic("AdventureWin");
                GameStateManager.Instance.UpdateGameState(null, KillCounter.KillCount);
                GameStateManager.Instance.GameOver(false);
            }
            OnLootUpdate?.Invoke(lootCount);
        }
    }
    #endregion
}
