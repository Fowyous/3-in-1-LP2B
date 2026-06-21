using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuleScript : MonoBehaviour
{
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
    
    public void startMiniUfoAttack()
    {
        StartCoroutine(loadMiniUfoAttack());
    }
    
    private IEnumerator loadMiniUfoAttack()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("Shooter");
        while (!load.isDone)
        {
            yield return null;
        }
    }
}
