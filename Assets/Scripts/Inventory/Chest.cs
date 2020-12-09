using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite openSprite, closedSprite;
    [SerializeField] private ChestScript chestScript;
    private bool isOpen;
    public void Interact()
    {
        if (isOpen)
        {
            StopInteract();
        }
        else
        {
            isOpen = true;            
            UIManager.MyInstance.CloseLeftSide(chestScript);
            InventoryScript.MyInstance.Open();
            CharacterPanel.MyInstance.Open();
            spriteRenderer.sprite = openSprite;
            chestScript.Open();
        }
    }

    public void StopInteract()
    {
        isOpen = false;
        InventoryScript.MyInstance.Close();
        CharacterPanel.MyInstance.Close();
        spriteRenderer.sprite = closedSprite;
        chestScript.Close();
    }
}
