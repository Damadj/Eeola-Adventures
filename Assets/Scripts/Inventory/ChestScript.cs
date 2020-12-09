using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private static ChestScript _instance;
    public static ChestScript MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<ChestScript>();
            return _instance;
        }
    }  
    private CanvasGroup canvasGroup;
    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
