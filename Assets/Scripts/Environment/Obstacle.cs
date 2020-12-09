﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public SpriteRenderer MySpriteRenderer { get; set; }
    private Color defaultColor;
    private Color fadedColor;

    // public int CompareTo(Obstacle other)
    // {
    //     if(MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
    //         return 1;        
    //     else if (MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
    //         return -1;
    //     return 0;
    // }

    // Start is called before the first frame update
    void Start()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = MySpriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOut()
    {
        MySpriteRenderer.color = fadedColor;
    }

    public void FadeIn()
    {
        MySpriteRenderer.color = defaultColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ObstacleCollider")
        {
            FadeOut();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "ObstacleCollider")
        {
            FadeIn();
        }
    }
}