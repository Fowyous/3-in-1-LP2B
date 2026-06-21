using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene transitions for the BrickBreaker mini-game (home screen,
/// rules screen, and back to the global main menu). These public methods
/// are meant to be wired directly to UI button OnClick events.
/// </summary>
public class NavigationBrickBreaker : MonoBehaviour
{
    private const string BrickBreakerHomeScene = "homeBrickBreaker";
    private const string BrickBreakerRuleScene = "ruleBrickBreaker";
    private const string MainMenuScene         = "Home";

    public void startBrickBreaker()
    {
        StartCoroutine(LoadScene(BrickBreakerHomeScene));
    }

    public void startBrickBreakerRule()
    {
        StartCoroutine(LoadScene(BrickBreakerRuleScene));
    }

    public void startHome()
    {
        StartCoroutine(LoadScene(MainMenuScene));
    }

    /// <summary>Loads the given scene asynchronously and waits for it to finish.</summary>
    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        while (!load.isDone)
        {
            yield return null;
        }
    }
}