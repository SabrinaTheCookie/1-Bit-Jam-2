using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private int _floorNumber;
    public GameObject stairUp;
    public GameObject stairDown;
    public GameObject spawner;
    public GameObject treasurePile;
    public List<Enemy> enemiesOnFloor;
    public List<GameObject> towersOnFloor = new List<GameObject>();

    public void SetupFloor(int floorNumber, int maxFloors)
    {
        _floorNumber = floorNumber;
        if (_floorNumber == 0)
        {
            //Is first floor, use spawner instead of stair up
            stairUp.SetActive(false);
        }
        else if (_floorNumber == maxFloors)
        {
            //Is last floor, replace stairs Down with loot
            stairDown.SetActive(false);
        }
        else
        {
            //standard floor, place stairs up and down.
            Vector3 stairPos = stairDown.transform.position;
            //Left, Right, Top, Bottom
            int side = Random.Range(0, 3);
            switch (side)
            {
                case 0: //Left
                    stairPos.x = -4.5f;
                    stairPos.z = Random.Range(-4.5f, 4.5f);
                    transform.Rotate(0, 90, 0);
                    break;
                case 1: //Right
                    break;
                case 2: //Top
                    break;
                case 3: //Bottom
                    break;
            }

            stairDown.transform.position = stairPos;
        }
    }
}
