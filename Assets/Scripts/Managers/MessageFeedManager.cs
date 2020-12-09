using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageFeedManager : MonoBehaviour
{
    private static MessageFeedManager _instance;
    public static MessageFeedManager MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<MessageFeedManager>();
            return _instance;
        }
    }

    [SerializeField] private GameObject messagePrefab;

    public void WriteMessage(string message)
    {
        var go = Instantiate(messagePrefab, transform);
        go.GetComponent<Text>().text = message;
        go.transform.SetAsFirstSibling();
        Destroy(go, 2);
    }
}
