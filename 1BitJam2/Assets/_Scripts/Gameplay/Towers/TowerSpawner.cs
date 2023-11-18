using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private float towerTooCloseRange;
    [SerializeField] private float clutterTooCloseRange;

    public static event Action<string> OnTowerSelected;
    public static event Action<string> OnTowerPlacementMessage;
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
                TryPlaceTower();
            }
        }
    }

    private void TryPlaceTower()
    {
        //Raycast for Floor at pointer
        Ray ray = mainCam.ViewportPointToRay(InputManager.Instance.PointPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
        {
            //Hit the floor, get placement position
            Floor floor = hit.transform.parent.GetComponent<Floor>();
            Vector3 towerPosition = getGridPosition(hit.point);
            
            //Is it a valid placement? I.e. Not Overlapping
            bool validPlacement = true;
            RaycastHit[] hits = Physics.SphereCastAll(towerPosition, towerTooCloseRange, Vector3.down);
            foreach (var obj in hits)
            {
                if (obj.transform.CompareTag("Tower")) validPlacement = false;
            }
            hits = Physics.SphereCastAll(towerPosition, clutterTooCloseRange, Vector3.down);
            foreach (var obj in hits)
            {
                if (obj.transform.CompareTag("Clutter")) validPlacement = false;
            }

            //Return if not valid
            if (!validPlacement)
            {
                OnTowerPlacementMessage?.Invoke("Invalid Placement!");
                return;
            }

            TowerBase tower = towerPrefabs[(int)selectedType].GetComponent<TowerBase>();
            
            //Consume the number of souls
            if (CurrencyController.Instance.ConsumeSoul(tower.cost))
            {
                //Then place the tower :)
                GameObject towerInstance = Instantiate(tower.gameObject, towerPosition, Quaternion.identity);
                towerInstance.transform.SetParent(floor.towerHolder, true);
                floor.towersOnFloor.Add(towerInstance.GetComponent<TowerBase>());
                AudioManager.Instance.PlaySound($"PlaceTower{(int)selectedType}");
            }
            else
            {
                //Cannot afford it :( 
                OnTowerPlacementMessage?.Invoke("Not Enough Souls!");
                return;
            }
        }

        isPlacing = false;
        //Clear tower selected
        OnTowerSelected?.Invoke("");
    }

    private Vector3 getGridPosition(Vector3 position){
        return new Vector3(
            Mathf.Round((position.x * 2f)) / 2f,
            position.y,
            Mathf.Round((position.z * 2f)) / 2f
            );
    }

    public void selectType(int type){ //Used with buttons on the UI
        
        selectedType = (TURRET_TYPE)type;
        Debug.Log(towerPrefabs[(int)selectedType].GetComponent<TowerBase>().cost);
        if (CurrencyController.Instance.HasEnoughSouls(towerPrefabs[(int)selectedType].GetComponent<TowerBase>().cost))
        {
            isPlacing = true;
            OnTowerSelected?.Invoke(selectedType.ToString());
        }
        else
        {
            OnTowerPlacementMessage?.Invoke("Not Enough Souls!");
        }
    }

    public enum TURRET_TYPE {
        Arrow,
        AreaOfEffect,
        Sniper
    }
}
