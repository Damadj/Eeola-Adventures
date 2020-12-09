using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(Character character);
public class GameManager : MonoBehaviour
{
    public event KillConfirmed killConfirmedEvent;
    private static GameManager _instance;
    public static GameManager MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        ClickTarget();        
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
                Mathf.Infinity, 2048);
            if (hit.collider != null && (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Interactable")))
            {
                Eeola.MyInstance.Interact();
            }
            else
            {
                Eeola.MyInstance.Target = null;
            }
        }
    }

    public void OnKillConfirmed(Character character)
    {
        if (killConfirmedEvent != null)
        {
            killConfirmedEvent(character);
        }
    }
}
