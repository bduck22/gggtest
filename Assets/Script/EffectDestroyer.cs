using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{
    public float Damage;
    ParticleSystem ParticleSystem;
    void Start()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (!ParticleSystem.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
