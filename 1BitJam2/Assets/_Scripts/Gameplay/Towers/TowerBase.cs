using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public int towerIndex;
    [Header("Componenets")] 
    public TowerRange towerRange;

    [Header("Tower Attributes")]
    public Enemy target;
    [Serializable] public enum TargetType { Closest, Furthest, HighestHealth, LowestHealth}
    public TargetType targetType;
    public float secondsBetweenAttacks;
    private float attackTimer;
    public float towerDamage;
    public float attackRadius;
    public float areaOfEffectRange;
    public int cost;

    void Awake()
    {
        towerRange.towerRange = attackRadius;
        attackTimer = secondsBetweenAttacks;
    }
    void Update()
    {
        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;

            if(attackTimer < 0)
            {
                Attack();
            }
        }
        else if(attackTimer < 0 && towerRange.validTargets.Count != 0)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        SelectTarget();
        //No target found? Dont attack.
        if (target == null || !towerRange.validTargets.Contains(target)) return;

        attackTimer = secondsBetweenAttacks;
        AudioManager.Instance.PlaySound($"TowerFire{towerIndex}");
        if(areaOfEffectRange == 0)
        {
            target.TakeDamage(towerDamage);
        }
        else
        {
            //Multi Target
            Collider[] hits = Physics.OverlapSphere(target.transform.position, areaOfEffectRange);
            foreach (var hit in hits)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.TakeDamage(towerDamage);
                }
            }
        }
        
    }

    void SelectTarget()
    {
        foreach(Enemy enemy in towerRange.validTargets)
        {
            if (!enemy) continue;
            if (!target) target = enemy;

            switch(targetType)
            {
                case TargetType.Closest:
                    //Is this enemy closer than the current target?
                    if(Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, target.transform.position))
                    {
                        target = enemy;
                    }
                    break;
                case TargetType.Furthest:
                    //Is this enemy further than the current target?
                    if (Vector3.Distance(transform.position, enemy.transform.position) > Vector3.Distance(transform.position, target.transform.position))
                    {
                        target = enemy;
                    }
                    break;
                case TargetType.HighestHealth:
                    //Does this enemy have higher health than the current target?
                    if(enemy.currentHealth > target.currentHealth)
                    {
                        target = enemy;
                    }
                    break;
                case TargetType.LowestHealth:
                    //Does this enemy have lower health than the current target?
                    if (enemy.currentHealth < target.currentHealth)
                    {
                        target = enemy;
                    }
                    break;
            }
        }
    }

    

}
