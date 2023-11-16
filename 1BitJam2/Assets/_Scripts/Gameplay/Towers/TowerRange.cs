using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour
{
    public List<Enemy> validTargets = new List<Enemy>();
    public float towerRange;
    private SphereCollider sphereCollider;
    private void Start()
    {
        sphereCollider = gameObject.GetComponent<SphereCollider>();
        sphereCollider.radius = towerRange;
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(!validTargets.Contains(enemy))
                validTargets.Add(enemy);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (validTargets.Contains(enemy))
                validTargets.Remove(enemy);
        }
    }
}
