using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationBrickBreaker : MonoBehaviour
{
    public void startBrickBreaker()
    {
        StartCoroutine(loadBrickBreaker());
    }
    
    private IEnumerator loadBrickBreaker()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("homeBrickBreaker");
        while (!load.isDone)
        {
            yield return null;
        }
    }
    
    public void startBrickBreakerRule()
    {
        StartCoroutine(loadBrickBreakerRule());
    }
    
    private IEnumerator loadBrickBreakerRule()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("ruleBrickBreaker");
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