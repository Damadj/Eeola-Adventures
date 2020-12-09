using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VendorItem
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity;
    [SerializeField] private bool unlimited;
    public Item MyItem { get => item; set => item = value; }
    public int MyQuantity { get => quantity; set => quantity = value; }
    public bool MyUnlimited { get => unlimited; }
}
