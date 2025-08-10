using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Player Player;
    public float Speed;

    public bool item;
    void Start()
    {
        Player= GetComponent<Player>(); 
    }
    void Update()
    {
        Speed = Player.Speed;
        transform.Translate(Speed*Input.GetAxis("Horizontal")*Time.deltaTime*(item?1.5f:1), 0, Speed*Input.GetAxis("Vertical")*Time.deltaTime * (item ? 1.5f : 1));
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.GetChild(0).GetComponent<BillBoard>().Flip = true;
        }
        else
        {
            transform.GetChild(0).GetComponent<BillBoard>().Flip = false;
        }
    }
}
