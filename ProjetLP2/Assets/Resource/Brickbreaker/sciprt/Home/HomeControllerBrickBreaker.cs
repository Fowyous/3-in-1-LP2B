using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the BrickBreaker "press any key to start" home screen: plays a
/// sound and transitions to the main game scene on the first key press.
/// </summary>
public class HomeControllerBrickBreaker : MonoBehaviour
{
    public AudioClip enterSong;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame && !Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneNavigator.Instance.LoadSceneWithSound(SceneNames.BRICK_BREAKER_GAME, enterSong);
        }
    }
}