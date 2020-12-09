using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SCTTYPE {DAMAGE, HEAL}
public class CombatTextManager : MonoBehaviour
{
    private static CombatTextManager _instance;
    public static CombatTextManager MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<CombatTextManager>();
            return _instance;
        }
    }

    [SerializeField] private GameObject combatTextPrefab;

    public void CreateText(Vector2 position, string text, SCTTYPE type, bool crit)
    {
        position.y += 0.6f;
        var sct = Instantiate(combatTextPrefab, transform).GetComponent<Text>();
        sct.transform.position = position;
        var operation = string.Empty;
        switch (type)
        {
            case SCTTYPE.DAMAGE:                
                sct.color = Color.red;
                break;
            case SCTTYPE.HEAL:
                operation += "+";
                sct.color = Color.green;
                break;
        }

        sct.text = operation + text;
        if (crit)
        {
            sct.GetComponent<Animator>().SetBool("Crit", crit);
        }
    }
}
