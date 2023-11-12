using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{
    Camera _mainCamera;
    void Start()
    {
        _mainCamera = Camera.main;
    }
    void LateUpdate()
    {
        transform.LookAt(_mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }
}
