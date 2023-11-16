using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectileBase : MonoBehaviour
{
    public Transform target;
    public Transform origin;

    public bool useArc;
    public float projectileFlightDuration = 1;
    float flightTime;
    public float damageRadius = 0;
    float damage;

    public void Init(Transform from, Transform to, float projectileDamage)
    {
        origin = from;
        target = to;
        damage = projectileDamage;
    }

    void Update()
    {
        if (!target || !origin)
        {
            Destroy(gameObject);
            return;
        }
        flightTime += Time.deltaTime;

        Vector3 currentPosition;
        if(useArc)
        {
            currentPosition = Vector3.Slerp(origin.position, target.position, flightTime / projectileFlightDuration);
        }
        else
        {
            currentPosition = Vector3.Lerp(origin.position, target.position, flightTime / projectileFlightDuration);
        }

        transform.position = currentPosition;

        if (flightTime >= projectileFlightDuration) Expire();
    }

    protected virtual void Expire()
    {
        if(damageRadius == 0)
        {
            //Single Target
            Enemy enemy = target.GetComponent<Enemy>();
            if(enemy)
                enemy.TakeDamage(damage);
        }
        else
        {
            //Multi Target
            Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius);
            foreach(var hit in hits)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if(enemy)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
        Destroy(gameObject);
    }
}
