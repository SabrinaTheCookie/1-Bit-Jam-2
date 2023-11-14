using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyWaveManager enemyWaveManager;

    EnemyBaseClass data;
    
    public float currentHealth;
    
    public float currentAttackPower;

    public float currentMovementSpeed;

    public float currentLootHeld = 0;

    public Floor currentFloor;
    public Vector2 currentPosition;
    bool advancing = true;


    /* Essentially a constructor for the Enemy class, called by EnemySpawner */
    public void Init(Floor _currentFloor, Vector2 startingPosition)
    {
        currentFloor = _currentFloor;
        currentPosition = startingPosition;
    }


    private void Awake()
    {
        enemyWaveManager = FindObjectOfType<EnemyWaveManager>();
    }


    public void SetData(EnemyBaseClass dataToSet)
    {
        data = dataToSet;
        currentHealth = data.maxHealth;
        currentAttackPower = data.attackPower;
        currentMovementSpeed = data.baseMovementSpeed;
    }


    public void TakeDamage(float damageAmount)
    {
        /* Reduce Health by damageAmount, kill this enemy instance if Health drops to/below zero. */

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            enemyWaveManager.DropLoot(currentLootHeld);
            enemyWaveManager.enemiesRemaining.Remove(this);
            /* Play Sound Effect or Particle ? */
            Destroy(gameObject);
        }
    }


    public float CollectLoot(float lootAmount)
    {
        /* Take loot up to carryCapacity, return the leftovers. */

        float capacity = data.carryCapacity - currentLootHeld;

        if (capacity < lootAmount) { lootAmount -= capacity; currentLootHeld += capacity; }
        else { currentLootHeld += lootAmount; lootAmount = 0; }

        return lootAmount;
    }


    



    public void ScaleMovementSpeed(float scalar)
    {
        currentMovementSpeed *= scalar;
    }

    public void ResetMovementSpeed()
    {
        currentMovementSpeed = data.baseMovementSpeed;
    }




    public void AttemptToMove()
    {
        /* Check if a move is possible, and then call the relevant function. */
        Vector2 desiredPosition = currentFloor.grid.FindEnemyNextPosition(currentPosition, advancing);

        Advance(desiredPosition);
    }


    public void Advance(Vector2 newPosition)
    {
        /* Move one cell forward along the path. */

        currentPosition = newPosition;

        // Change position in world
        transform.position = Grid.ConvertGridToWorldPosition(currentPosition);
    }

    public void Retreat()
    {
        /* Move one cell back along the path. */
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            AttemptToMove();
        }
    }
}
