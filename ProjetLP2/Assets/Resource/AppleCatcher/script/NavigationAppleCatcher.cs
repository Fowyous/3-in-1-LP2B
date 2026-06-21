using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationAppleCatcher : MonoBehaviour
{
    public void startAppleCatcher()
    {
        StartCoroutine(loadAppleCatcher());
    }

    private IEnumerator loadAppleCatcher()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("homeAppleCatcher");
        while (!load.isDone)
        {
            yield return null;
        }
    }

    public void startAppleCatcherRule()
    {
        StartCoroutine(loadAppleCatcherRule());
    }
    
    private IEnumerator loadAppleCatcherRule()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("ruleAppleCatcher");
        while (!load.isDone)
        {
            yield return null;
        }
    }
    
    public void startHome()
    {
        StartCoroutine(loadHome());
    }
    
    private IEnumerator loadHome()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("Home");
        while (!load.isDone)
        {
            yield return null;
        }
    }
}
