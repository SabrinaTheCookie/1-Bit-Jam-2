using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private float towerTooCloseRange;

    public static event Action<string> OnTowerSelected;
    public static event Action OnInvalidPlacement;
    private bool isPlacing = false;
    private TURRET_TYPE selectedType;
    private Camera mainCam;
    

    private void Awake()
    {
        mainCam = Camera.main;
    }

    public void Update(){
        if(isPlacing) 
        {
            //TODO replace with InputManager events
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCam.ViewportPointToRay(InputManager.Instance.PointPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
                {
                    Floor floor = hit.transform.parent.GetComponent<Floor>();
                    Vector3 towerPosition = getGridPosition(hit.point);
                    bool validPlacement = true;
                    RaycastHit[] hits = Physics.SphereCastAll(towerPosition, towerTooCloseRange, Vector3.down);
                    foreach (var obj in hits)
                    {
                        if (obj.transform.CompareTag("Tower")) validPlacement = false;
                    }

                    if (!validPlacement)
                    {
                        Debug.LogWarning("Invalid Placement!");
                        OnInvalidPlacement?.Invoke();
                        return;
                    }
                    
                    GameObject tower = Instantiate(towerPrefabs[(int)selectedType], towerPosition, Quaternion.identity);
                    tower.transform.SetParent(floor.towerHolder, true);
                    floor.towersOnFloor.Add(tower);
                    
                }
                isPlacing = false;
                //Clear tower selected
                OnTowerSelected?.Invoke("");
                
            }
        }
    }

    private Vector3 getGridPosition(Vector3 position){
        return new Vector3(
            Mathf.Round((position.x * 2f)) / 2f,
            position.y,
            Mathf.Round((position.z * 2f)) / 2f
            );
    }

    public void selectType(int type){ //Used with buttons on the UI
        isPlacing = true;
        selectedType = (TURRET_TYPE)type;
        OnTowerSelected?.Invoke(selectedType.ToString());
    }

    public enum TURRET_TYPE {
        Arrow,
        AreaOfEffect,
        Sniper
    }
}
