using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHit : MonoBehaviour
{
    Turret Turret;
    private void Start()
    {
     Turret = transform.parent.GetComponent<Turret>();   
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ObjectHit"))
        {
            Turret.Hp -= other.transform.parent.GetComponent<BulletMove>().Damage;
        }
        if (other.CompareTag("towerstop"))
        {
            if (!Turret.Stop)
            {
                StartCoroutine(Turret.stoptower());
            }
        }
    }
}
