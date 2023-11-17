using System;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    EnemyWaveManager enemyWaveManager;

    EnemyBaseClass data;

    public float currentHealth;
    public int soulsOnDeath;
    public int currentLootHeld = 0;

    public float currentAttackPower;

    public float baseTickRate;
    public float currentTickRate;
    private float tickTimer;


    public Floor currentFloor;
    public Vector2 currentPosition;
    bool advancing = true;
    bool alive = true;
    public int maxJumpAttempts;
    private int currentJumpAttempts;

    public GameObject lootPrefab;

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
        tickTimer = currentTickRate;
    }


    private void Awake()
    {
        enemyWaveManager = FindObjectOfType<EnemyWaveManager>();
    }

    void Update()
    {
        if (!alive) return;

        if (tickTimer > 0)
        {
            tickTimer -= Time.deltaTime;

            if (tickTimer <= 0)
            {
                for (int i = 0; i < maxJumpAttempts; i++)
                {
                    if (AttemptToMove()) break;
                }
                tickTimer = currentTickRate / ActionPhase.TickMultiplier;
            }
        }
    }


    public void SetData(EnemyBaseClass dataToSet)
    {
        data = dataToSet;
        currentHealth = data.maxHealth;
        currentAttackPower = data.attackPower;
        baseTickRate = data.baseTickRate;
        currentTickRate = data.baseTickRate;
        soulsOnDeath = data.soulsOnDeath;

        //TODO Kris help :o
        int index = enemyWaveManager.enemyTypes.IndexOf(dataToSet);
        transform.GetChild(0).localScale = new Vector3(index * 0.025f + 0.2f, index * 0.025f + 0.2f, index * 0.025f + 0.2f);
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
            SplashTextManager.SpawnSplashText(SplashTextManager.SplashTextStyle.Damage, $"{damageAmount}", transform.position);
            hitParticles.Play();
        }
    }

    private void Defeated()
    {
        alive = false;
        DropLoot(currentLootHeld);
        AudioManager.Instance.PlaySound("UnitDeath");
        OnEnemyDefeated?.Invoke(this, false);
        /* Play Sound Effect or Particle ? */
        Destroy(gameObject);
    }

    public void DropLoot(int lootAmount)
    {
        if (lootAmount > 0)
        {
            Loot droppedLoot = Instantiate(lootPrefab, currentFloor.lootHolder).GetComponent<Loot>();
            droppedLoot.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            droppedLoot.Init(lootAmount);
        }
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
        currentTickRate *= scalar;
    }

    public void ResetMovementSpeed()
    {
        currentTickRate = data.baseTickRate;
    }
    
    public bool AttemptToMove()
    {
        /* Check if a move is possible, and then call the relevant function. */
        Vector2 desiredPosition = currentFloor.grid.FindEnemyNextPosition(currentPosition, advancing);

        if (desiredPosition == currentPosition) { AttemptToChangeFloor(); }
        else
        {
            GameObject cellOccupant = currentFloor.grid.GetCellOccupant(desiredPosition);
            if (cellOccupant == null) { Advance(desiredPosition); }
            else if (cellOccupant.GetComponent<Enemy>() != null)
            {
                Enemy occupant = cellOccupant.GetComponent<Enemy>();
                /* Swap places with another Enemy if this runs into another. */
                if (occupant.advancing != advancing || occupant.currentTickRate > currentTickRate) 
                {
                    currentFloor.grid.SetCellOccupant(currentPosition, null);
                    currentFloor.grid.SetCellOccupant(desiredPosition, null);
                    occupant.Advance(currentPosition);
                    Advance(desiredPosition);
                }

                /* Then Move again, basically leap-frogging if they touch another enemy. */
                return false;
            }
        }

        return true;
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
                currentLootHeld = currentFloor.lootPile.GetComponent<Loot>().TakeLoot(data.carryCapacity - currentLootHeld);
                AudioManager.Instance.PlaySound("GoldTaken");
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




}
