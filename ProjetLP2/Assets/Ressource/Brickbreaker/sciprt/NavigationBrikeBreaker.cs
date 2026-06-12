using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationBrikeBreaker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartBrikeBreak()
    {
        StartCoroutine(LoadBrikeBreak());
    }
    
    private IEnumerator LoadBrikeBreak()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("BrikeBreakAcceuil");
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