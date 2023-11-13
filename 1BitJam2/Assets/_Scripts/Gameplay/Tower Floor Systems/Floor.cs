using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private int _floorNumber;
    public Transform objectHolder;
    public GameObject stairUp;
    public GameObject stairDown;
    public GameObject spawner;
    public GameObject treasurePile;
    public List<Enemy> enemiesOnFloor;
    public List<GameObject> towersOnFloor = new List<GameObject>();

    public void SetupFloor(int floorNumber, int maxFloors)
    {
        _floorNumber = floorNumber;
        gameObject.name = $"Floor{_floorNumber}";
        if (_floorNumber == 0)
        {
            //Is first floor, use spawner instead of stair up
            PlaceStairsDown(false);

        }
        else if (_floorNumber == maxFloors - 1)
        {
            //Is last floor, replace stairs Down with loot
            PlaceStairsUp();
        }
        else
        {
            //standard floor, place stairs up and down.
            PlaceStairsDown(true);
        }
    }

    void PlaceStairsDown(bool placeUpwardsToo)
    {
        stairDown = Instantiate(stairDown, objectHolder);

        //Left, Right, Top, Bottom
        int side = Random.Range(0, 4);
        (Vector3, float) posRotPair = GetValidStairLocation(side);
        

        stairDown.transform.localPosition = posRotPair.Item1;
        stairDown.transform.Rotate(0,posRotPair.Item2,0);

        if(placeUpwardsToo) PlaceStairsUp(side);
    }

    void PlaceStairsUp(int pairSide=-1)
    {
        stairUp = Instantiate(stairUp, objectHolder);
        (Vector3, float) posRotPair;
        if (pairSide != -1)
        {
            //Get the opposite side to the pair
            pairSide = pairSide == 0 ? 1 : pairSide == 1 ? 0 : pairSide == 2 ? 3 : 2;
            posRotPair = GetValidStairLocation(pairSide);
        }
        else
        {
            //Not paired, place in random position.
            //Left, Right, Top, Bottom
            int side = Random.Range(0, 4);
            posRotPair = GetValidStairLocation(side);
        }
        stairUp.transform.localPosition = posRotPair.Item1;
        stairUp.transform.Rotate(0,posRotPair.Item2,0);
    }

    public (Vector3, float) GetValidStairLocation(int side)
    {
        Vector3 stairPos = Vector3.zero;
        float rotation = 0;
        //Place opposite side of the pair
        switch (side)
        {
            case 0: //Left
                stairPos.x = -4.5f;
                stairPos.z = Random.Range(-4.5f, 4.5f);
                rotation = -90;
                break;
            case 1: //Right
                stairPos.x = 4.5f;
                stairPos.z = Random.Range(-4.5f, 4.5f);
                rotation = 90;
                break;
            case 2: //Top
                stairPos.x = Random.Range(-4.5f, 4.5f);
                stairPos.z = 4.5f;
                break;
            case 3: //Bottom
                stairPos.x = Random.Range(-4.5f, 4.5f);
                stairPos.z = -4.5f;
                rotation = 180;
                break;
        }

        return (stairPos, rotation);
    }
}
