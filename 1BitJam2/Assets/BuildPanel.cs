using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildPanel : MonoBehaviour
{
    public TextMeshProUGUI towerInfoText;
    public string invalidPlacementMessage;
    public float messageDuration;

    private void OnEnable()
    {
        TowerSpawner.OnTowerSelected += UpdateTowerInfo;
        TowerSpawner.OnInvalidPlacement += InvalidPlacement;
    }
    
    private void OnDisable()
    {
        TowerSpawner.OnTowerSelected -= UpdateTowerInfo;
        TowerSpawner.OnInvalidPlacement -= InvalidPlacement;
    }

    void UpdateTowerInfo(string info)
    {
        StopCoroutine(InvalidPlacementMessage());
        towerInfoText.text = info;
    }

    void InvalidPlacement()
    {
        StartCoroutine(InvalidPlacementMessage());
    }

    IEnumerator InvalidPlacementMessage()
    {
        towerInfoText.text = invalidPlacementMessage;
        yield return new WaitForSeconds(messageDuration);
        towerInfoText.text = "";
        yield return null;
    }
}
