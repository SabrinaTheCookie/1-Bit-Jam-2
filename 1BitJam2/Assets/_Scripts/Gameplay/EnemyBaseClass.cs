using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Class", menuName = "ScriptableObjects/Enemies", order = 1)]
public class EnemyBaseClass : ScriptableObject
{
    public string enemyClass;

    public float maxHealth;

    public float attackPower;

    public float baseMovementSpeed;

    public float carryCapacity;

    public Sprite sprite;
}



