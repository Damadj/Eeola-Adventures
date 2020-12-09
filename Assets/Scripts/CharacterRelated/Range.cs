using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    private void Start()
    {
        parent = GetComponentInParent<Enemy>();
    }
    private Enemy parent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.Target = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        parent.Target = null;
    }
}
