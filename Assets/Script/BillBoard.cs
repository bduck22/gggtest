using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform Target;
    public bool Flip;
    void Start()
    {
        
    }
    void Update()
    {
        if (Flip)
        {
            transform.localEulerAngles = new Vector3(42.184f, 0, 0);
        }
        else
        {
            transform.LookAt(Target);
        }
    }
}
