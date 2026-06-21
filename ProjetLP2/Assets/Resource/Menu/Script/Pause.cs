using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Lets the player press Escape during gameplay to return to the main menu.
/// </summary>
public class Pause : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            startHome();
        }
    }

    public void startHome()
    {
        SceneNavigator.Instance.LoadScene(SceneNames.MAIN_MENU);
    }
}