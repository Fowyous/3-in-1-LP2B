using UnityEngine;
/// <summary>
/// Handles scene transitions for the Apple Catcher mini-game (home screen,
/// rules screen, and back to the global main menu). These public methods
/// are meant to be wired directly to UI button OnClick events.
/// </summary>
public class NavigationAppleCatcher : MonoBehaviour
{
    public void startAppleCatcher()
    {
        SceneNavigator.Instance.LoadScene(SceneNames.APPLE_CATCHER_HOME);
    }
    

    public void startAppleCatcherRule()
    {
        SceneNavigator.Instance.LoadScene(SceneNames.APPLE_CATCHER_RULE);
    }
    
    
    public void startHome()
    {
        SceneNavigator.Instance.LoadScene(SceneNames.MAIN_MENU);
    }
    
}
