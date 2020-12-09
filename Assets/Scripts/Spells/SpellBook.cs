using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    private static SpellBook _instance;
    public static SpellBook MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<SpellBook>();
            return _instance;
        }
    }
    
    [SerializeField] private Projectile[] spells;
    public Projectile[] Spells { get => spells; set => spells = value; }
    public CanvasGroup MyCanvasGroup { get; set; }

    public void Awake()
    {
        MyCanvasGroup = GetComponent<CanvasGroup>();
    }

    public Projectile GetSpell(string spellNameStr)
    {
        return Array.Find(spells, x => x.MyName == spellNameStr);
    }
    
    public void OpenClose()
    {
        if (Mathf.Approximately(MyCanvasGroup.alpha, 1)) Close();
        else Open();
    }
    public void Close()
    {
        MyCanvasGroup.alpha = 0;
        MyCanvasGroup.blocksRaycasts = false;
    }
    public void Open()
    {
        UIManager.MyInstance.CloseLeftSide(this);
        MyCanvasGroup.alpha = 1;
        MyCanvasGroup.blocksRaycasts = true;
    }
}
