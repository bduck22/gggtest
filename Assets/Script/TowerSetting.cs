using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSetting : MonoBehaviour
{
    public bool Ok=true;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9||other.gameObject.layer==8||other.gameObject.layer==10)
        {
            Ok = false;
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 8 || other.gameObject.layer == 10)
        {
            Ok = true;
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }
    public void Set()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        Destroy(transform.GetComponent<Rigidbody>());
        Destroy(this);
    }
}
