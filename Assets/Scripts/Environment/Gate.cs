using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour, IInteractable
{
    [SerializeField] private int sceneIndexTo;
    [SerializeField] private int sceneIndexFrom;
    public void Interact()
    {
        if (sceneIndexTo == 1)
        {
            UIManager.MyInstance.MyCinemachines[sceneIndexTo].transform.position = new Vector3(-20.5f,10.5f, -10) ;
            UIManager.MyInstance.MyCinemachines[sceneIndexFrom].SetActive(false);
            // SceneManager.UnloadSceneAsync(2);
            Eeola.MyInstance.transform.position = new Vector2(-20.5f,10.5f);            
            UIManager.MyInstance.MyCinemachines[sceneIndexTo].SetActive(true);
        }
        else StartCoroutine(UIManager.MyInstance.LoadScene(sceneIndexTo, sceneIndexFrom));
    }

    public void StopInteract()
    {
        
    }
}
