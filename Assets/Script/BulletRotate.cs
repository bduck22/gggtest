using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotate : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        transform.Rotate(2, 1.6f, 0.6f);
    }
}
