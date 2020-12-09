using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    public Quest MyQuest { get; set; }

    private bool markedComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        GetComponent<Text>().color = Color.red;
        Questlog.MyInstance.ShowDescription(MyQuest);
    }

    public void DeSelect()
    {
        GetComponent<Text>().color = Color.white;
    }

    public void IsComplete()
    {
        if (MyQuest.IsComplete && !markedComplete)
        {
            markedComplete = true;
            GetComponent<Text>().text = "[" + MyQuest.MyLevel + "]" + MyQuest.MyTitle + "(Complete)";
            MessageFeedManager.MyInstance.WriteMessage(String.Format("{0} (Complete)", MyQuest.MyTitle));
        }
        else if (!MyQuest.IsComplete)
        {
            markedComplete = false;
            GetComponent<Text>().text = "[" + MyQuest.MyLevel + "]" + MyQuest.MyTitle;
        }
    }
}
