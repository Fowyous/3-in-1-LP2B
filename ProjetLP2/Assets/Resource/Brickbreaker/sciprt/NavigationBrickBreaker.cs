using UnityEngine;

/// <summary>
/// Handles scene transitions for the BrickBreaker mini-game (home screen,
/// rules screen, and back to the global main menu). These public methods
/// are meant to be wired directly to UI button OnClick events.
/// </summary>
public class NavigationBrickBreaker : MonoBehaviour
{
    public void startBrickBreaker()
    {
        SceneNavigator.Instance.LoadScene(SceneNames.BRICK_BREAKER_HOME);
    }

    public void startBrickBreakerRule()
    {
        SceneNavigator.Instance.LoadScene(SceneNames.BRICK_BREAKER_RULE);
    }

    public void startHome()
    {
        SceneNavigator.Instance.LoadScene(SceneNames.MAIN_MENU);
    }
}