using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretTarget : MonoBehaviour
{
    public Turret Turret;
    void Start()
    {
        Turret = transform.parent.GetComponent<Turret>();
    }

    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (Turret.Type == TowerType.Heal&&other.transform.parent)
        {
            if (other.transform.parent.GetComponent<Turret>())
            {
                foreach(Turret t in Turret.Turrets)
                {
                    if(t == other.transform.parent.GetComponent<Turret>())
                    {
                        return;
                    }
                }
                Turret.Turrets.Add(other.transform.parent.GetComponent<Turret>());
            }
        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                if (Turret.Target)
                {
                    if(Vector3.Distance(Turret.Target.transform.position, transform.position) > Vector3.Distance(other.transform.parent.position, transform.position))
                    {
                        Turret.lazertime = 0;
                        Turret.Target = other.transform.parent.GetComponent<EnemyBase>();
                    }
                }
                else Turret.Target = other.transform.parent.GetComponent<EnemyBase>();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Turret.Target = null;
        }
    }
}
