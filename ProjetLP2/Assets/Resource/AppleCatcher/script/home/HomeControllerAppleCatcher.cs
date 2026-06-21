using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the Apple Catcher "press any key to start" home screen: plays a
/// sound and transitions to the main game scene on the first key press.
/// </summary>
public class HomeControllerAppleCatcher : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame && !Keyboard.current.escapeKey.isPressed)
        {
            SceneNavigator.Instance.LoadScene(SceneNames.APPLE_CATCHER_GAME);
        }
    }
}
