using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    Player player;
    void Start()
    {
        player = transform.parent.GetComponent<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ObjectHit"))
        {
            player.Hp -= other.transform.parent.GetComponent<BulletMove>().Damage;
        }
        if (other.CompareTag("towerstop"))
        {
            player.Hp -= other.transform.GetComponent<EffectDestroyer>().Damage;
        }
    }
}
