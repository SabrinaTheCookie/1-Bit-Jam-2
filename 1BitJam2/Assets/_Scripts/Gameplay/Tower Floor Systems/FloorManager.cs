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
    
    private List<Floor> floors;
    public List<Floor> Floors => floors;


    private void Awake()
    {
        _floorBuilder = GetComponent<FloorBuilder>();
        floors = _floorBuilder.BuildFloor(floorsInTower);
        OrganizeFloors();

        _floorTraversal = GetComponent<FloorTraversal>();
        FindObjectOfType<EnemyWaveManager>().Init();
    }

    private void OrganizeFloors()
    {
        float currentHeight = 0;
        for (int i = 0; i < floors.Count; i++)
        {
            //Place the floor in the correct position
            floors[i].transform.position = Vector3.down * currentHeight;
            currentHeight += ySpaceBetweenFloors;
            if (currentHeight == ySpaceBetweenFloors) currentHeight *= 1.5f;

            floors[i].SetupFloor(this, i, floors.Count);
        }
    }
    
    

}
