using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyWaveManager enemyWaveManager;

    EnemyBaseClass data;

    public float currentHealth;

    public float currentAttackPower;

    public float currentMovementSpeed;

    public int currentLootHeld = 0;

    public Floor currentFloor;
    public Vector2 currentPosition;
    bool advancing = true;
    bool alive = true;

    public ParticleSystem hitParticles;

    //<EnemyDefeated, escaped>
    public static event Action<Enemy, bool> OnEnemyDefeated;
    public static event Action<int, Enemy> OnEnemyChangedFloors;


    /* Essentially a constructor for the Enemy class, called by EnemySpawner */
    public void Init(Floor _currentFloor, Vector2 startingPosition, EnemyBaseClass enemyType)
    {
        currentFloor = _currentFloor;
        currentPosition = startingPosition;
        transform.SetParent(currentFloor.enemyHolder);
        SetData(enemyType);
        StartCoroutine(TurnTick());
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
            Defeated();
        }
        else
        {
            hitParticles.Play();
        }
    }

    private void Defeated()
    {
        enemyWaveManager.DropLoot(currentLootHeld);
        OnEnemyDefeated?.Invoke(this, false);
        /* Play Sound Effect or Particle ? */
        Destroy(gameObject);
    }


    public int CollectLoot(int lootAmount)
    {
        /* Take loot up to carryCapacity, return the leftovers. */

        int capacity = data.carryCapacity - currentLootHeld;

        if (capacity < lootAmount) { currentLootHeld += capacity; }
        else { currentLootHeld += lootAmount; lootAmount = 0; } // To Do: Destroy DroppedLoot if this function returns 0.

        advancing = false;

        return lootAmount;
    }


    public void ExitDungeon()
    {
        /* Enemy has escaped with your gold! */

        //Just call enemy defeated to tell the wave manager to remove you :)
        //You defeated them but they took yo stuff
        OnEnemyDefeated?.Invoke(this, true);
        /* Play Sound Effect or Particle ? */
        Destroy(gameObject);
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

        if (desiredPosition == currentPosition) { AttemptToChangeFloor(); }
        else
        {
            if (currentFloor.grid.GetCellOccupant(desiredPosition) == null) { Advance(desiredPosition); }
            // to do: swap places with another Enemy if this runs into another that's going in the opposite direction.
        }
    }
    
    public void Advance(Vector2 newPosition)
    {
        /* Move one cell forward along the path. */
        currentFloor.grid.SetCellOccupant(currentPosition, null);
        currentPosition = newPosition;

        // Change position in world
        transform.localPosition = Grid.ConvertGridToWorldPosition(currentPosition);

        currentFloor.grid.SetCellOccupant(currentPosition, gameObject);
    }
    
    public void AttemptToChangeFloor()
    {
        Floor nextFloor = currentFloor.GetNextFloor(advancing);
        if (advancing)
        {
            if (currentFloor.lastFloor)
            {
                currentFloor.treasurePile.GetComponent<Loot>().lootValue -= CollectLoot(data.carryCapacity);
                ExitDungeon(); // To Do? Remove this line.
            }
            else if (nextFloor.grid.GetCellOccupant(nextFloor.grid.path.startPos) == null)
            {
                ChangeFloor(nextFloor);
            }
        }
        else
        {
            if (currentFloor == FindObjectOfType<FloorManager>().Floors[0])
            {
                ExitDungeon();
            }
            else if (nextFloor.grid.GetCellOccupant(nextFloor.grid.path.endPos) == null)
            {
                ChangeFloor(nextFloor);
            }
        }
    }
    
    public void ChangeFloor(Floor nextFloor)
    {
        Floor oldFloor = currentFloor;
        currentFloor = nextFloor;

        transform.SetParent(currentFloor.enemyHolder);
        transform.localScale = Vector3.one;

        if (advancing)
        {
            Advance(currentFloor.grid.path.startPos);
            oldFloor.grid.SetCellOccupant(oldFloor.grid.path.endPos, null);
        }
        else
        {
            Advance(currentFloor.grid.path.endPos);
            oldFloor.grid.SetCellOccupant(oldFloor.grid.path.startPos, null);
        }
        
        OnEnemyChangedFloors?.Invoke(currentFloor.floorNumber, this);
    }
    
    public void Retreat()
    {
        /* Move one cell back along the path. */
    }

    IEnumerator TurnTick()
    {
        while (alive)
        {
            yield return new WaitForSeconds(ActionPhase.SecondsPerTick);
            AttemptToMove();
        }
    }
}
