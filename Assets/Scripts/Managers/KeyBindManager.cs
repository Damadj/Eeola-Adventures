using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyBindManager : MonoBehaviour
{
    private static KeyBindManager _instance;
    public static KeyBindManager MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<KeyBindManager>();
            return _instance;
        }
    }
    
    public Dictionary<string, KeyCode> KeyBinds { get; set; }
    public Dictionary<string, KeyCode> ActionBinds { get; private set; }
    private string bindName;

    private void Start()
    {
        KeyBinds = new Dictionary<string, KeyCode>();
        ActionBinds = new Dictionary<string, KeyCode>();
        BindKey("ACT1", KeyCode.Q);
        BindKey("ACT2", KeyCode.W);
        BindKey("ACT3", KeyCode.E);
        BindKey("ACT4", KeyCode.R);
        BindKey("ACT5", KeyCode.T);
        BindKey("ACT6", KeyCode.Y);        
        BindKey("USE1", KeyCode.Alpha1);        
        BindKey("USE2", KeyCode.Alpha2);        
        BindKey("USE3", KeyCode.Alpha3);        
        BindKey("USE4", KeyCode.Alpha4);        
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = KeyBinds;
        if (key.Contains("ACT"))
        {
            currentDictionary = ActionBinds;
        }

        if (!currentDictionary.ContainsKey(key))
        {
            currentDictionary.Add(key, keyBind);
            UIManager.MyInstance.UpdateKeyText(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind))
        {
            var myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;
            currentDictionary[myKey] = KeyCode.None;
            UIManager.MyInstance.UpdateKeyText(myKey, KeyCode.None);
            UIManager.MyInstance.UpdateKeyText(key, KeyCode.None);
        }

        currentDictionary[key] = keyBind;
        UIManager.MyInstance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }

    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }

    private void OnGUI()
    {
        if (bindName != string.Empty)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
