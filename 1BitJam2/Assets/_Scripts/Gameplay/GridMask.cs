using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMask : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 offset;

    private SpriteMask spriteMask;

    /*
    Script for revealing and Hiding the Grid for placement
    */

    void Start()
    {
        spriteMask = GetComponent<SpriteMask>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
        {
            spriteMask.enabled = true;
            Vector3 mousePosition = hit.point;
            transform.position = Vector3.Lerp(transform.position, mousePosition + offset, Time.deltaTime * moveSpeed);
        }
        else
        {
            spriteMask.enabled = false;
        }
    }
}
