using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartAppleCatcher()
    {
        StartCoroutine(LoadAppleCatcher());
    }
    
    private IEnumerator LoadAppleCatcher()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("AppleCatcherAcceuil");
        while (!load.isDone)
        {
            yield return null;
        }
    }
    
    public void StartAcceuil()
    {
        StartCoroutine(LoadAcceuil());
    }
    
    private IEnumerator LoadAcceuil()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("Acceuil");
        while (!load.isDone)
        {
            yield return null;
        }
    }
}
