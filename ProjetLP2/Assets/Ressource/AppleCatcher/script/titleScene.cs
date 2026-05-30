using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class titleScene : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartCoroutine(LoadMainGame());
        }
    }
    
    private IEnumerator LoadMainGame()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("AppleCatcherGame");
        while (!load.isDone)
        {
            yield return null;
        }
    }
}
