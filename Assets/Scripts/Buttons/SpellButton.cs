using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string spellName;
    [SerializeField] private Text spellNameText;
    [SerializeField] private Text spellDescription;

    private void Start()
    {
        spellNameText.text = spellName;
        spellDescription.text = SpellBook.MyInstance.GetSpell(spellName).GetDescriptionForSpellBook();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandScript.MyInstance.TakeMovable(SpellBook.MyInstance.GetSpell(spellName));
        }
    }
}
