using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasurePile : MonoBehaviour
{
 
    private int currentTreasure;
    [SerializeField] private int startingTreasure;

    private void Start()
    {
        currentTreasure = startingTreasure;
    }
}
