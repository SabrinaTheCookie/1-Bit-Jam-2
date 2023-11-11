using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject[] towerPrefabs;

    private bool isPlacing = false;
    private TURRET_TYPE selectedType;

    public void Update(){
        if(isPlacing) 
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //Debug.Log("WAD");
                if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
                {

                    Vector3 towerPosition = getGridPosition(hit.point);

                    switch (selectedType)
                    {
                        case(TURRET_TYPE.Circle):
                            Instantiate(towerPrefabs[0], towerPosition, Quaternion.identity);
                            break;
                            
                        case(TURRET_TYPE.Square):
                            Instantiate(towerPrefabs[1], towerPosition, Quaternion.identity);
                            break;
                        
                        case(TURRET_TYPE.Hexagon):
                            Instantiate(towerPrefabs[2], towerPosition, Quaternion.identity);
                            break;
                        
                        default:
                            break;
                    }
                }
                isPlacing = false;

            }
        }
    }

    private Vector3 getGridPosition(Vector3 _posiiton){
        return new Vector3(
            Mathf.Round((_posiiton.x / 1f)) * 1f,
            _posiiton.y,
            Mathf.Round((_posiiton.z / 1f)) * 1f
            );

    }

    public void selectType(string type){ //Used with buttons on the UI
        isPlacing = true;
        switch (type)
        { //can't put enums into buttons OnClick() apparently?
            case("circle"):
                selectedType = TURRET_TYPE.Circle;
                break;

            case("square"):
                selectedType = TURRET_TYPE.Square;
                break;

            case("hexagon"):
                selectedType = TURRET_TYPE.Hexagon;
                break;

            default:
                break;
        }
    }

    public enum TURRET_TYPE {
        Circle,
        Hexagon,
        Square
    }
}
