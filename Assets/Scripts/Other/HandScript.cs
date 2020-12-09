using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript _instance;
    public static HandScript MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<HandScript>();
            return _instance;
        }
    }
    public IMovable MyMovable { get; set; }
    private Image icon;
    [SerializeField] private Vector3 offset;
    void Start()
    {
        icon = GetComponent<Image>();
    }

    void Update()
    {
        icon.transform.position = Input.mousePosition + offset;
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && MyMovable != null)
        {
           if (CharacterPanel.MyInstance.MySelectedButton != null) CharacterPanel.MyInstance.MySelectedButton.DequipArmor();
            DeleteItem();
        }
    }

    public void TakeMovable(IMovable movable)
    {
        MyMovable = movable;
        icon.sprite = movable.MyIcon;
        icon.color = Color.white;
    }

    public IMovable Put()
    {
        var tmp = MyMovable;
        MyMovable = null;
        icon.color = new Color(0,0,0,0);
        return tmp;
    }

    public void Drop()
    {
        MyMovable = null;
        icon.color = new Color(0,0,0,0);
        InventoryScript.MyInstance.MyFromSlot = null;
    }

    public void DeleteItem()
    {
        if (MyMovable is Item item && InventoryScript.MyInstance.MyFromSlot != null)
        {
            item.MySlot.Clear();
        }
        Drop();
        InventoryScript.MyInstance.MyFromSlot = null;
    }
}
