using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public Transform Target;
    public Vector3 TargetP;

    public float Speed;
    bool target;

    public bool Stop;

    public float Damage;
    void Start()
    {
        target = false;
    }

    void Update()
    {
        if (Stop)
        {
            return;
        }
        if (Target)
        {
            target = true;
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Speed*Time.deltaTime);
            if(transform.position == Target.position)
            {
                Destroy(gameObject);
            }
        }
        else if(!target)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetP, Speed*Time.deltaTime);
            if(transform.position == TargetP)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
