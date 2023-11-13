using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private int floorsInTower;
    [SerializeField] private int ySpaceBetweenFloors;
    public int YSpaceBetweenFloors => ySpaceBetweenFloors;
    private FloorBuilder _floorBuilder;
    private FloorTraversal _floorTraversal;
    
    private List<GameObject> floors;
    public List<GameObject> Floors => floors;


    private void Awake()
    {
        _floorBuilder = GetComponent<FloorBuilder>();
        floors = _floorBuilder.BuildFloor(floorsInTower);
        OrganizeFloors();

        _floorTraversal = GetComponent<FloorTraversal>();
    }

    private void OrganizeFloors()
    {
        float currentHeight = 0;
        for (int i = 0; i < floors.Count; i++)
        {
            floors[i].transform.position = Vector3.down * currentHeight;
            currentHeight += ySpaceBetweenFloors;
            if (currentHeight == ySpaceBetweenFloors) currentHeight *= 1.5f;
        }
    }
    
    

}
