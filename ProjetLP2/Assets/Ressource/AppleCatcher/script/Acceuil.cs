using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Acceuil : MonoBehaviour
{
    public TextMeshPro appleCatcher;
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
}
