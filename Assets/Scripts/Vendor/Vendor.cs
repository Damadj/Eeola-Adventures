using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : NPC
{
    [SerializeField] private VendorItem[] items;
    public VendorItem[] MyItems { get => items; set => items = value; }
    
    public override void Interact()
    {
        InventoryScript.MyInstance.Open();
        base.Interact();
    }

    public override void StopInteract()
    {
        InventoryScript.MyInstance.Close();
        base.StopInteract();
    }
}
