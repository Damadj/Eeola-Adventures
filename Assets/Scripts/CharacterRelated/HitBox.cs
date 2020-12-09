using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HitBox : MonoBehaviour
{
    private Enemy parent;
    void Start()
    {
        parent = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        var projectile = other.gameObject.GetComponent<Projectile>();
        var player = other.gameObject.GetComponent<Eeola>();
        if (player != null)
        {
            parent.Player = player;
            parent.IsColliding = true;
        }
        if (projectile != null)
        {
            parent.TakeDamageCoroutine = StartCoroutine(parent.TakeDamage(Random.Range(projectile.ProjectileMinDamage, projectile.ProjectileMaxDamage + 1)));
            parent.Target = GameObject.Find("Eeola").transform;
        }
    }
}
