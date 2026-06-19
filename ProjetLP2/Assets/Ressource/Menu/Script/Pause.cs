using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            StartAcceuil();
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
