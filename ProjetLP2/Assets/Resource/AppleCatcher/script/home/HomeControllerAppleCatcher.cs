using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HomeControllerAppleCatcher : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame && !Keyboard.current.escapeKey.isPressed)
        {
            StartCoroutine(loadGameAppleCatcher());
        }
    }
    
    private IEnumerator loadGameAppleCatcher()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("gameAppleCatcher");
        while (!load.isDone)
        {
            yield return null;
        }
    }
}
