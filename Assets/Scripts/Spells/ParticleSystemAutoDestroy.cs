using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private ParticleSystem explosion;
    public void Start()
    {
        explosion = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (explosion)
        {
            if (!explosion.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
