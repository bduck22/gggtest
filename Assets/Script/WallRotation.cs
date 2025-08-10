using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRotation : MonoBehaviour
{
    public float RotateSpeed;
    void Start()
    {
        
    }
    void Update()
    {
        if (!transform.GetComponent<TowerSetting>())
        {
            Destroy(this);
        }
        transform.Rotate(0, Input.GetAxis("Mouse ScrollWheel")*RotateSpeed, 0);
    }
}
