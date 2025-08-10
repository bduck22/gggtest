using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
            transform.GetChild(0).gameObject.SetActive(GameManager.Instance.player.GetComponent<PlayerMove>().item);
        transform.GetChild(1).gameObject.SetActive(GameManager.Instance.player.Invin);
    }
}
