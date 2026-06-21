using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            startHome();
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
